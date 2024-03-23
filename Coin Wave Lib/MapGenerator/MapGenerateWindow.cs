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
        /*void ЗаполнитьВсеПоле()
        {
            for (int i = 0; i < _width * _height; i++)
            {
                gameObjects.Add(new Coin(
                _numObj, new Rctngl(mg.mainPoints[_numObj],
                        mg._sizeX, mg._sizeY).CopyTextureCoords(blocksPanel.gameObjects[currentIndex].Rectangle)));
                thisElements[_numObj].element = true;
            }
        }*/
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

        // Создание всех списков и массивов тут
        List<GameObject> gameObjects = new List<GameObject>(0);
        ThisElement[] thisElements;
        private double[] _currentPosition;
        private double[] _emptyElement;

        // размер карты 34 на 15 и разрешение экрана 1920 на 1080
        private readonly int _width = 34;
        private readonly int _height = 18;

        private float timeOfMoment;
        private int _numObj = 0;
        int currentIndex = 0;
        int index = 0;
        MapGenerate mg;
        BlocksPanel blocksPanel;
        TextureMap textureMap;


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
            textureMap = new TextureMap(6, 6, @"data\image\texMap.png");
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
            Pnt НеИмеетХначенияЧтоТут = new(0.0, 0.0, 0.0, 0.0, 0.0);
            blocksPanel = new(new Rctngl(new Pnt(-0.8, 0.6, 0.0, 1.0, 0.0), 1.6, 1.2), 10);
            blocksPanel.AddGameObject(new BackWall(textureMap.NewTextureCoords(new Rctngl(new(0.0, 0.0, 0.0, 0.0, 0.0), 0, 0), 8)));
            blocksPanel.AddGameObject((new Coin(textureMap.NewTextureCoords(new Rctngl(new(0.0, 0.0, 0.0, 0.0, 0.0), 0, 0), 22))));
            blocksPanel.AddGameObject((new EnterDoor(textureMap.NewTextureCoords(new Rctngl(new(0.0, 0.0, 0.0, 0.0, 0.0), 0, 0), 19))));
            blocksPanel.AddGameObject(new ExitDoor(textureMap.NewTextureCoords(new Rctngl(new(0.0, 0.0, 0.0, 0.0, 0.0), 0, 0), 19)));
            blocksPanel.AddGameObject(new SolidWall(textureMap.NewTextureCoords(new Rctngl(new(0.0, 0.0, 0.0, 0.0, 0.0), 0, 0), 1)));
            blocksPanel.AddGameObject(new EnterDoor(textureMap.NewTextureCoords(new Rctngl(new(0.0, 0.0, 0.0, 0.0, 0.0), 0, 0), 2)));
            blocksPanel.AddGameObject(new ExitDoor(textureMap.NewTextureCoords(new Rctngl(new(0.0, 0.0, 0.0, 0.0, 0.0), 0, 0), 4)));
            blocksPanel.AddGameObject(new SolidWall(textureMap.NewTextureCoords(new Rctngl(new(0.0, 0.0, 0.0, 0.0, 0.0), 0, 0), 5)));
            blocksPanel.PlacingObjectsInPanel();
            blocksPanel.GenerateTexturViborObj(@"data\image\redsqrt.png");
            bufferEmptyElements = new(_emptyElement, @"data\image\sqrt.png");
            bufferCurrentElement = new(_currentPosition, @"data\image\redsqrt.png");
            bufferWindowBlocksPanel = new(blocksPanel.GetVertices(), @"data\image\bluesqrt.png");
            bufferViborPanel = new(blocksPanel.viborObj.GetVertices(), @"data\image\redsqrt.png");
            bufferGameObj = new(GameObject.GetVertices(gameObjects), textureMap.TexturePath);
            //ЗаполнитьВсеПоле();/////////////////////////////
            bufferBlockPanel = new(GameObject.GetVertices(blocksPanel.gameObjects), textureMap.TexturePath);
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
            if (
                    currentKeyboardState.IsKeyPressed(Keys.Tab) &&
                    lastKeyboardState.IsKeyDown(Keys.LeftShift) &&
                    index < blocksPanel.gameObjects.Count - 1
                    )
            {
                index++;
                currentIndex = index;
            }
            else if (
                    (!currentKeyboardState.IsKeyPressed(Keys.LeftShift) &&
                    index >= blocksPanel.gameObjects.Count - 1 &&
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
            blocksPanel.ObjVibor(index);
            bufferViborPanel.UpdateDate(blocksPanel.viborObj.GetVertices());

            
            if (currentKeyboardState.IsKeyDown(Keys.Delete) && thisElements[_numObj].Get() == true)
            {
            
                if (gameObjects.Count == 1) bufferGameObj = new(GameObject.GetVertices(gameObjects), textureMap.TexturePath);
                else
                    for (int i = 0; i < gameObjects.Count; i++)
                    {
                        if (gameObjects[i].Index == _numObj)
                        {
                            gameObjects.RemoveAt(i);
                            thisElements[_numObj].element = false;
                            bufferGameObj.UpdateDate(GameObject.GetVertices(gameObjects));
                            break;
                        }
                    }
            }



            ClickWASD(currentKeyboardState);
            if (currentKeyboardState.IsKeyDown(Keys.Enter))  ClickEnter();
            
            if (currentKeyboardState.IsKeyDown(Keys.Escape)) Close();
            if (lastKeyboardState != null &&
                lastKeyboardState.IsKeyDown(Keys.LeftControl) &&
                currentKeyboardState.IsKeyDown(Keys.S))
                    Close();
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
        private void ClickWASD(KeyboardState keyboardState)
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
            /*for (int i = 0; i < gameObjects.Count && thisElements[_numObj].Get() == true; i++)
            {
                if (gameObjects[i].Index == _numObj)
                {
                    gameObjects.RemoveAt(i);
                    thisElements[_numObj].element = false;
                    break;
                }
            }*/
            if (thisElements[_numObj].Get() == false)
            {
                gameObjects.Add(new Coin(
                    _numObj, new Rctngl(mg.mainPoints[_numObj],
                    mg._sizeX, mg._sizeY).CopyTextureCoords(blocksPanel.gameObjects[currentIndex].Rectangle)));
                thisElements[_numObj].element = true;
                bufferGameObj = new(GameObject.GetVertices(gameObjects), textureMap.TexturePath);
            }
        }
    }
}
