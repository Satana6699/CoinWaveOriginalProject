using System;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Coin_Wave_Lib;
using System;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;
using Coin_Wave_Lib.MapGenerator;
using System.Numerics;
using Coin_Wave_Lib.ObjCS;
using Coin_Wave_Lib.Objects.InterfaceObjects;
   
namespace Coin_Wave_Lib
{
    class ThisElement
    {
        public int index;
        public bool element;
        public ThisElement(int index)
        {
            this.index = index;
            element = false;
        }
        public bool Get()
        {
            return element;
        }
    }
    public class MapGenerateWindow : GameWindow
    {
        KeyboardState lastKeyboardState, currentKeyboardState;
        private float frameTime = 0.0f;
        private float _time = 0.0f;
        private int fps = 0;

        // Создание всех полей для буферов тут
        BufferManager bufferEmptyElements;
        BufferManager bufferCurrentElement;
        BufferManager bufferWindowBlocksPanel;
        BufferManager bufferGameObj;
        BufferManager bufferBlockPanel;
        BufferManager bufferViborPanel;
        BufferManager bufferSave;

        // Создание всех списков и массивов тут
        List<GameObject> gameObjects = new List<GameObject>(0);
        ThisElement[] thisElements;
        private double[] _currentPosition;
        private double[] _emptyElement;

        // размер карты 34 на 18 и разрешение экрана 1920 на 1080
        private readonly int _width = 34;
        private readonly int _height = 18;
        private bool IsTherePlayer = false;

        private float timeOfMoment;
        private int _numObj = 0;
        int currentIndex = 0;
        int index = 0;
        MapGenerate mg;
        BlocksPanel blocksPanel;
        TextureMap textureMap;
        DoublePoints doublePoints = new DoublePoints();
        InterfaceConcreteObj save;
        bool ifSaved;

        public MapGenerateWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            NameExampleWindow = "Coin Wave Map Generator";
            Title = NameExampleWindow;

            Console.WriteLine(GL.GetString(StringName.Version));
            Console.WriteLine(GL.GetString(StringName.Vendor));
            Console.WriteLine(GL.GetString(StringName.Renderer));
            Console.WriteLine(GL.GetString(StringName.ShadingLanguageVersion));

            VSync = VSyncMode.On;
        }
        public string NameExampleWindow { private set; get; }


        protected override void OnLoad()
        {
            
            base.OnLoad();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            textureMap = new TextureMap(6, 6, 4, @"data\image\texMap.png");
            save = new InterfaceConcreteObj(new Rectangle(new Point(-1,1), 0.3, 0.1), TexturePoint.Default(), doublePoints);
            thisElements = new ThisElement[_height * _width];
            for(int i = 0; i < thisElements.Length; i++) thisElements[i] = new ThisElement(i);
            mg = new(_width, _height);
            mg.GeneratePoints();
            _emptyElement = mg.GetPoints();
            _currentPosition = new double[20]; //20 так в 4 вершинах по 5 позиций
            for (int i = 0, j = _numObj * 20; i < _currentPosition.Length; i++, j++)
            {
                _currentPosition[i] = _emptyElement[j];
            }
                
            blocksPanel = new(new Rectangle
                (
                    new Point(-0.8, 0.6), 1.6, 1.2),
                    new TexturePoint[] { new TexturePoint(0, 1), new TexturePoint(1, 1), new TexturePoint(1, 0), new TexturePoint(0, 0)}, 
                    doublePoints,
                    10,
                    textureMap
                );
            blocksPanel.GenerateMenuElement("Coin", 8);
            blocksPanel.GenerateMenuElement("Coin", 7);
            blocksPanel.GenerateMenuElement("Coin", 9);
            blocksPanel.GenerateMenuElement("Coin", 1);
            blocksPanel.GenerateMenuElement("Coin", 19);
            blocksPanel.GenerateMenuElement("Coin", 4);
            blocksPanel.GenerateMenuElement("Coin", 20);
            blocksPanel.GenerateMenuElement("Coin", 21);
            blocksPanel.GenerateMenuElement("Coin", 22);
            blocksPanel.GenerateTexturViborObj(@"data\image\redsqry.png");
            bufferSave = new(save.GetVertices(), @"data\image\save.png");
            bufferEmptyElements = new(_emptyElement, @"data\image\empty.png");
            bufferCurrentElement = new(_currentPosition, @"data\image\redsqrt.png");
            bufferWindowBlocksPanel = new(blocksPanel.GetVertices(), @"data\image\bluesqrt.png");
            bufferViborPanel = new(blocksPanel.viborObj.GetVertices(), @"data\image\redsqrt.png");
           
            bufferGameObj = new(Obj.GetVertices(gameObjects.ToArray(), 5), textureMap.TexturePath);
            bufferBlockPanel = new(Obj.GetVertices(blocksPanel.MenuElements.ToArray(), 5), textureMap.TexturePath);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            lastKeyboardState = currentKeyboardState;
            currentKeyboardState = KeyboardState.GetSnapshot();

            frameTime += (float)args.Time;
            _time += (float)args.Time;
            fps++;
            if (frameTime >= 1.0f)
            {
                Title = $"OpenTK {NameExampleWindow} : FPS - {fps}";
                frameTime = 0.0f;
                fps = 0;
            }

            
            blocksPanel.ObjVibor(index);
            bufferViborPanel.UpdateDate(blocksPanel.viborObj.GetVertices());

            if (currentKeyboardState.IsKeyDown(Keys.Delete) && thisElements[_numObj].Get()) ClickDelete();
            ClickWASD(currentKeyboardState);
            ClickShift();
            if (currentKeyboardState.IsKeyDown(Keys.Enter)) ClickEnter();
            if (currentKeyboardState.IsKeyDown(Keys.Escape)) Close();
            if (lastKeyboardState != null &&
                lastKeyboardState.IsKeyDown(Keys.LeftControl) &&
                currentKeyboardState.IsKeyDown(Keys.S))
            {
                ifSaved = FileSave.SerializeObjectsToXml(gameObjects, @"data\maps\lvl1.xml");
                //ifSaved = true;
            }
            if (ifSaved) ifSaved = save.IsLive((float)args.Time);
        }


        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

            // Alpha-chanal support
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            bufferEmptyElements.Render();
            bufferGameObj.Render();
            bufferCurrentElement.Render();
            if (currentKeyboardState.IsKeyDown(Keys.LeftShift))
            {
                bufferWindowBlocksPanel.Render();
                bufferViborPanel.Render();
                bufferBlockPanel.Render();
            }
            if (ifSaved)
            {
                bufferSave.Render();
            }

            SwapBuffers();
        }

        protected override void OnUnload()
        {
            base.OnUnload();
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }
        private async void ClickWASD(KeyboardState keyboardState)
        {
            float pressingTime = 0.6f;
            int numObjFuture = _numObj;

            
            switch (true)
            {
                case var _ when currentKeyboardState.IsKeyPressed(Keys.W):
                    numObjFuture -= _width;
                    timeOfMoment = _time;
                    break;
                case var _ when currentKeyboardState.IsKeyPressed(Keys.A):
                    numObjFuture -= 1;
                    timeOfMoment = _time;
                    break;
                case var _ when currentKeyboardState.IsKeyPressed(Keys.S):
                    numObjFuture += _width;
                    timeOfMoment = _time;
                    break;
                case var _ when currentKeyboardState.IsKeyPressed(Keys.D):
                    numObjFuture += 1;
                    timeOfMoment = _time;
                    break;
                default:
                    break;
            }
            if (currentKeyboardState.IsKeyDown(Keys.D) || currentKeyboardState.IsKeyDown(Keys.S) ||
                currentKeyboardState.IsKeyDown(Keys.A) || currentKeyboardState.IsKeyDown(Keys.W))
            if (_time - timeOfMoment >= pressingTime && currentKeyboardState.IsKeyDown(Keys.W)) numObjFuture -= _width;
            else if (_time - timeOfMoment >= pressingTime && currentKeyboardState.IsKeyDown(Keys.A)) numObjFuture -= 1;
            else if (_time - timeOfMoment >= pressingTime && currentKeyboardState.IsKeyDown(Keys.S)) numObjFuture += _width;
            else if (_time - timeOfMoment >= pressingTime && currentKeyboardState.IsKeyDown(Keys.D)) numObjFuture += 1;
            if (numObjFuture >= 0 && numObjFuture < _emptyElement.Length / 20)
                _numObj = numObjFuture;
            else
                numObjFuture = _numObj;
            for (int i = 0, j = _numObj * 20; i < _currentPosition.Length; i++, j++)
            {
                _currentPosition[i] = _emptyElement[j];
            }
            bufferCurrentElement.UpdateDate(_currentPosition);
        }

        private void ClickEnter()
        {
            if (thisElements[_numObj].Get() == false)
            {
                Coin ob = (new Coin
                    (
                        new Rectangle(mg.mainPoints[_numObj], mg._sizeX, mg._sizeY),
                        textureMap.GetTexturePoints(blocksPanel.MenuElements[currentIndex].IndexTexture),
                        doublePoints,
                        _numObj
                    ));
                thisElements[_numObj].element = true;

                // Добавление обьектов методом конкатенации массивов с целью оптимизации программы,
                // Так как конвертировать массив игровых обьектов каждый раз не выгодно
                bufferGameObj.UpdateDate(bufferGameObj.vertices.Concat(ob.GetVertices()).ToArray());
                gameObjects.Add(ob);
            }
        }
        private void ClickDelete()
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i].Index == _numObj)
                {
                    gameObjects.RemoveAt(i);
                    thisElements[_numObj].element = false;
                    bufferGameObj.UpdateDate(Obj.GetVertices(gameObjects.ToArray(), 5));
                    break;
                }
            }
        }
        private void ClickShift()
        {
            if (
                    currentKeyboardState.IsKeyPressed(Keys.Tab) &&
                    lastKeyboardState.IsKeyDown(Keys.LeftShift) &&
                    index < blocksPanel.MenuElements.Count - 1
                    )
            {
                index++;
                currentIndex = index;
            }
            else if (
                    (!currentKeyboardState.IsKeyPressed(Keys.LeftShift) &&
                    index >= blocksPanel.MenuElements.Count - 1 &&
                    currentKeyboardState.IsKeyPressed(Keys.Tab)) ||
                    !currentKeyboardState.IsKeyDown(Keys.LeftShift)
                    )
            {
                index = 0;
            }
            else if (currentKeyboardState.IsKeyPressed(Keys.LeftShift))
            {
                index = 0;
                currentIndex = index;
            }
        }
    }
}
