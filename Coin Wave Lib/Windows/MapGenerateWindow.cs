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
using System.Numerics;
using Coin_Wave_Lib.Objects.InterfaceObjects;
using System.Diagnostics;

namespace Coin_Wave_Lib
{
    public class MapGenerateWindow : GameWindow
    {
        KeyboardState lastKeyboardState, currentKeyboardState;
        private float frameTime = 0.0f;
        private float _time = 0.0f;
        private int fps = 0;
        int frameCounter = 0;

        // Создание всех полей для буферов тут
        BufferManager bufferEmptyElements;
        BufferManager bufferCurrentElement;
        BufferManager bufferWindowBlocksPanel;
        BufferManager bufferGameObj;
        BufferManager bufferBlockPanel;
        BufferManager bufferViborPanel;
        BufferManager bufferSave;
        // размер карты 34 на 18 и разрешение экрана 1920 на 1080
        /*private readonly int _width = 32;
        private readonly int _height = 20;*/
        private readonly int _width = 32;
        private readonly int _height = 18;
        private bool IsTherePlayer = false;

        // Создание всех списков и массивов тут
        List<GameObjectData> gameObjectDataList = new List<GameObjectData>(0);
        List<GameObjectData>[,] gameObjects;
        int[,] f = new int[1,1];
        private double[] _currentPosition;
        private double[] _emptyElement;


        private float timeOfMoment;
        private int _numObj = 0;
        int currentIndex = 0;
        int index = 0;
        MapGenerate mg;
        BlocksPanel blocksPanel;
        TextureMap textureMap;
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

            gameObjects = new List<GameObjectData>[_width,_height];

            for (int i = 0; i < gameObjects.GetLength(0); i++)
            {
                for (int j = 0; j < gameObjects.GetLength(1); j++)
                {
                    gameObjects[i, j] = new List<GameObjectData>();
                }
            }

            textureMap = new TextureMap(5, 5, 4, @"data\textureForGame\texMap.png");
            save = new InterfaceConcreteObj(new Rectangle(new Point(-1,1), 0.3, 0.1), TexturePoint.Default());
            mg = new(_width, _height, 0.02, 0.1, 0.01);
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
                    10,
                    textureMap
                );
            blocksPanel.GenerateMenuElement(typeof(StartDoor).Name, 7);
            blocksPanel.GenerateMenuElement(typeof(ExitDoor).Name, 7);
            blocksPanel.GenerateMenuElement(typeof(SolidWall).Name, 0);
            blocksPanel.GenerateMenuElement(typeof(Player).Name, 5);
            blocksPanel.GenerateMenuElement(typeof(BackWall).Name, 3);
            blocksPanel.GenerateTexturViborObj(@"data\textureForInterface\redsqry.png");
            bufferSave = new(save.GetVertices(), @"data\textureForInterface\save.png");
            bufferEmptyElements = new(_emptyElement, @"data\textureForInterface\empty.png");
            bufferCurrentElement = new(_currentPosition, @"data\textureForInterface\redsqrt.png");
            bufferWindowBlocksPanel = new(blocksPanel.GetVertices(), @"data\textureForInterface\bluesqrt.png");
            bufferViborPanel = new(blocksPanel.choiceObj.GetVertices(), @"data\textureForInterface\redsqrt.png");
           
            bufferGameObj = new(Obj.GetVertices(gameObjectDataList.ToArray(), 5), textureMap.TexturePath);
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
            bufferViborPanel.UpdateDate(blocksPanel.choiceObj.GetVertices());

            if (currentKeyboardState.IsKeyDown(Keys.Delete)) ClickDelete();
            ClickWASD(currentKeyboardState);
            ClickShift();
            if (currentKeyboardState.IsKeyDown(Keys.Enter)) ClickEnter();
            if (currentKeyboardState.IsKeyDown(Keys.Escape)) Close();
            if (lastKeyboardState != null &&
                lastKeyboardState.IsKeyDown(Keys.LeftControl) &&
                currentKeyboardState.IsKeyDown(Keys.S))
            {
                ifSaved = FileSave.SerializeObjectsToXml(gameObjectDataList, @"data\maps\lvl1.xml");
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
            int operationFrequency = 6;
            frameCounter ++;
            if (currentKeyboardState.IsKeyDown(Keys.D) || currentKeyboardState.IsKeyDown(Keys.S) ||
                currentKeyboardState.IsKeyDown(Keys.A) || currentKeyboardState.IsKeyDown(Keys.W))
                if (_time - timeOfMoment >= pressingTime && currentKeyboardState.IsKeyDown(Keys.W) && frameCounter >= operationFrequency)
                {
                    numObjFuture -= _width;
                    frameCounter = 0;
                }
                else if (_time - timeOfMoment >= pressingTime && currentKeyboardState.IsKeyDown(Keys.A) && frameCounter >= operationFrequency)
                {
                    numObjFuture -= 1;
                    frameCounter = 0;
                }
                else if (_time - timeOfMoment >= pressingTime && currentKeyboardState.IsKeyDown(Keys.S) && frameCounter >= operationFrequency)
                {
                    numObjFuture += _width;
                    frameCounter = 0;
                }
                else if (_time - timeOfMoment >= pressingTime && currentKeyboardState.IsKeyDown(Keys.D) && frameCounter >= operationFrequency)
                {
                    numObjFuture += 1;
                    frameCounter = 0;
                }


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
                GameObjectData ob = new GameObjectData
                {
                    Name = blocksPanel.MenuElements[currentIndex].Name,
                    Index = _numObj,
                    Rectangle = new Rectangle(mg.mainPoints[_numObj], mg._unitX, mg._unitY),
                    TexturePoints = textureMap.GetTexturePoints(blocksPanel.MenuElements[currentIndex].IndexTexture),
                };

                // Добавление обьектов методом конкатенации массивов с целью оптимизации программы,
                // Так как конвертировать массив игровых обьектов каждый раз не выгодно
                bufferGameObj.UpdateDate(bufferGameObj.vertices.Concat(ob.GetVertices()).ToArray());
                gameObjectDataList.Add(ob);
  
        }
        private void ClickDelete()
        {
            for (int i = 0; i < gameObjectDataList.Count; i++)
            {
                if (gameObjectDataList[i].Index == _numObj)
                {
                    gameObjectDataList.RemoveAt(i);
                    bufferGameObj.UpdateDate(Obj.GetVertices(gameObjectDataList.ToArray(), 5));
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
