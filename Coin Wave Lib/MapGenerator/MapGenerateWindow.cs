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


        private readonly double[] _currentPosition =
        {
             0.9, -0.9, 0.0, 1.0, 0.0, // 1bottom right
            -0.9, -0.9, 0.0, 0.0, 0.0, // 2bottom left
            -0.9,  0.9, 0.0, 0.0, 1.0,  // 3top left
             0.9,  0.9, 0.0, 1.0, 1.0, // 0top right
        };

        private readonly uint[] _indicesCurrentPosition =
        {
            0, 1, 3,
            1, 2, 3
        };




        private int _elementBufferObject;

        private int _vertexBufferObject;

        private int _vertexArrayObject;

        private Shader _shader;

        // For documentation on this, check Texture.cs.
        private Texture _texture;
        private int _elementBufferObject2;

        private int _vertexBufferObject2;

        private int _vertexArrayObject2;

        private Shader _shader2;

        // For documentation on this, check Texture.cs.
        private Texture _texture2;

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
            //LoadEmptyMap();
            MapGenerate mg = new(_width, _height);
            mg.GeneratePoints();
            _emptyElement = mg.GetPoints();

            emptyElements = new(_emptyElement,
                @"D:\Семестр 4 (полигон)\Курсовая работа\coin wave\Coin Wave (sln)\Coin Wave Lib\data\image\sqrt.png");
            currentElement = new(_currentPosition,
                @"D:\Семестр 4 (полигон)\Курсовая работа\coin wave\Coin Wave (sln)\Coin Wave Lib\data\image\redsqrt.png");
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
