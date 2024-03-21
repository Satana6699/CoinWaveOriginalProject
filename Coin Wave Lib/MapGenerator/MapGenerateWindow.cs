using System;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static Coin_Wave_Lib.PlayerDouble;
using System;
using System.Collections.Generic;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;
using static Coin_Wave_Lib.PlayerFloat;
using Coin_Wave_Lib.MapGenerator;
using System.Numerics;
using Coin_Wave_Lib.ObjCS;

namespace Coin_Wave_Lib
{
    public class MapGenerateWindow : GameWindow
    {
        // размер карты 34 на 15 и разрешение экрана 1920 на 1080
        private readonly int _width = 34;
        private readonly int _height = 18;
        private int _numObj = 0;
        KeyboardState lastKeyboardState, currentKeyboardState;
        private float frameTime = 0.0f;
        private int fps = 0;
        private double[] _emptyElement;
        private float _time = 0.0f;
        private float timeOfMoment;
        BufferManager emptyElements;
        BufferManager currentElement;
        BufferManager windowBlocksPanel;
        BlocksPanel blocksPanel;
        BufferManager viborPanel;
        BufferManager[] bufs = new BufferManager[8];

        int index = 0;

        private double[] _currentPosition;
        private double[] _vertBlocksPanel;
        

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
            MapGenerate mg = new(_width, _height);
            mg.GeneratePoints();
            _emptyElement = mg.GetPoints();
            _currentPosition = new double[20]; //20 так в 4 вершинах по 5 позиций
            for (int i = 0, j = _numObj * 20; i < _currentPosition.Length; i++, j++)
            {
                _currentPosition[i] = _emptyElement[j];
            }
            _vertBlocksPanel = new[]{
             0.8, -0.6, 0.0, 1.0, 0.0,
            -0.8, -0.6, 0.0, 0.0, 0.0,
            -0.8,  0.6, 0.0, 0.0, 1.0,
             0.8,  0.6, 0.0, 1.0, 1.0,
            };
            Pnt pntPoh = new(0.0, 0.0, 0.0, 0.0,0.0);
            List<GameObject> pnts = new List<GameObject>(0);
            pnts.Add(new BackWall(pntPoh, 0, 0, @"data\image\sqrt.png"));
            pnts.Add(new Coin(pntPoh, 0, 0, @"data\image\gamer.png"));
            pnts.Add(new EnterDoor(pntPoh, 0, 0, @"data\image\redsqrt.png"));
            pnts.Add(new ExitDoor(pntPoh, 0, 0, @"data\image\stone.png"));
            pnts.Add(new SolidWall(pntPoh, 0, 0, @"data\image\trava.png"));
            pnts.Add(new EnterDoor(pntPoh, 0, 0, @"data\image\redsqrt.png"));
            pnts.Add(new ExitDoor(pntPoh, 0, 0, @"data\image\stone.png"));
            pnts.Add(new SolidWall(pntPoh, 0, 0, @"data\image\trava.png"));
            blocksPanel = new(new Pnt(-0.8, 0.6, 0.0, 1.0, 0.0),
                              1.6, 1.2, @"data\image\bluesqrt.png", 10);
            blocksPanel.AddGameObject(pnts);
            blocksPanel.GenerateTexturViborObj(@"data\image\redsqrt.png");
            emptyElements = new(_emptyElement,
                @"data\image\sqrt.png");
            currentElement = new(_currentPosition,
                @"data\image\redsqrt.png");
            windowBlocksPanel = new(blocksPanel.GetVertices(),
                blocksPanel.path);
            viborPanel = new(blocksPanel.viborObj.GetVertices(), blocksPanel.viborObj.path);
            int k = 0;
            foreach (GameObject go in blocksPanel.gameObjects)
            {
                bufs[k] = new(go.GetVertices(), go.path);
                k ++;
            }
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
            if (currentKeyboardState.IsKeyPressed(Keys.Tab) && index < blocksPanel.gameObjects.Count-1) index++;
            else if (!currentKeyboardState.IsKeyPressed(Keys.LeftShift) && 
                     index >= blocksPanel.gameObjects.Count - 1 &&
                     currentKeyboardState.IsKeyPressed(Keys.Tab)) index = 0;
            blocksPanel.ObjVibor(index);
            viborPanel.UpdateDate(blocksPanel.viborObj.GetVertices());
            Click(currentKeyboardState);
            currentElement.UpdateDate(_currentPosition);
            if (currentKeyboardState.IsKeyDown(Keys.Escape)) Close();
            if (lastKeyboardState != null && lastKeyboardState.IsKeyDown(Keys.LeftControl) && currentKeyboardState.IsKeyDown(Keys.S)) Close();
            
            
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            emptyElements.Render();
            currentElement.Render();
            if (currentKeyboardState.IsKeyDown(Keys.LeftShift))
            {
                windowBlocksPanel.Render();
                viborPanel.Render();
                for(int i = 0; i < bufs.Length; i++)
                {
                    bufs[i].Render();
                }
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
        private void Click(KeyboardState keyboardState)
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
        }
    }
}
