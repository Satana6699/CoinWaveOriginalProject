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

        private uint[] _indicesEmptyElement;
        /*{
            0, 1, 3,
            1, 2, 3
        };*/

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
            
            AllLoad();
        }

        void LoadEmptyMap()
        {
            int wertexes = 4;
            int pointsOfWertex = 5;
            int offset = wertexes * pointsOfWertex;
            /*double sizeX = (double)2 / _width;
            double sizeY = (double)2 / _height;*/
            double sizeX = (double)2 / _width*20;
            double sizeY = (double)2 / _height*20;
            double x = -1.0;
            double y =  1.0;
            _emptyElement = new double[_width * _height * wertexes * pointsOfWertex];
            for (int i = 0; i < _emptyElement.Length; i += offset)
            {
                if (i == 0)
                {

                }
                else
                {

                }
                // Point 0
                _emptyElement[i+5]  = x + sizeX;
                _emptyElement[i+6]  = y;
                _emptyElement[i+7]  = 0.0;
                _emptyElement[i+8]  = 1.0;
                _emptyElement[i+9]  = 0.0;
                // Point 1
                _emptyElement[i+10] = x + sizeX;
                _emptyElement[i+11] = y - sizeY;
                _emptyElement[i+12] = 0.0;
                _emptyElement[i+13] = 0.0;
                _emptyElement[i+14] = 0.0;
                // Point 2
                _emptyElement[i+15] = x;
                _emptyElement[i+16] = y - sizeY;
                _emptyElement[i+17] = 0.0;
                _emptyElement[i+18] = 0.0;
                _emptyElement[i+19] = 1.0;
                // Point 3
                _emptyElement[i]    = x;
                _emptyElement[i+1]  = y;
                _emptyElement[i+2]  = 0.0;
                _emptyElement[i+3]  = 1.0;
                _emptyElement[i+4]  = 1.0;
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
            Click(currentKeyboardState);
            GL.BufferData(BufferTarget.ArrayBuffer, _currentPosition.Length * sizeof(double), _currentPosition, BufferUsageHint.DynamicDraw);
            if (currentKeyboardState.IsKeyDown(Keys.Escape)) Close();
            if (lastKeyboardState != null && lastKeyboardState.IsKeyDown(Keys.LeftControl) && currentKeyboardState.IsKeyDown(Keys.S)) Close();
            
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

            GL.BindVertexArray(_vertexArrayObject);
            _texture.Use(TextureUnit.Texture0);
            _shader.Use();
            GL.DrawArrays(PrimitiveType.Quads, 0, _emptyElement.Length);

            //-------

            GL.BindVertexArray(_vertexArrayObject2);
            _texture2.Use(TextureUnit.Texture0);
            _shader.Use();
            GL.DrawArrays(PrimitiveType.Quads, 0, _currentPosition.Length);

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
        private void AllLoad()
        {
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _emptyElement.Length * sizeof(double), _emptyElement, BufferUsageHint.DynamicDraw);
            
            _shader = new Shader(@"data\shaders\shader.vert", @"data\shaders\shader.frag");
            _shader.Use();
            var vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Double, false, 5 * sizeof(double), 0);

            var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Double, false, 5 * sizeof(double), 3 * sizeof(double));

            _texture = Texture.LoadFromFile(@"D:\Семестр 4 (полигон)\Курсовая работа\coin wave\Coin Wave (sln)\Coin Wave Lib\data\image\sqrt.png");
            _texture.Use(TextureUnit.Texture0);


            //////////////////////////////////////////


            _vertexArrayObject2 = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject2);

            _vertexBufferObject2 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject2);
            GL.BufferData(BufferTarget.ArrayBuffer, _currentPosition.Length * sizeof(double), _currentPosition, BufferUsageHint.DynamicDraw);

            _shader.Use();

            var vertexLocation2 = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Double, false, 5 * sizeof(double), 0);

            var texCoordLocation2 = _shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Double, false, 5 * sizeof(double), 3 * sizeof(double));

            _texture2 = Texture.LoadFromFile(@"D:\Семестр 4 (полигон)\Курсовая работа\coin wave\Coin Wave (sln)\Coin Wave Lib\data\image\redsqrt.png");
            _texture2.Use(TextureUnit.Texture0);
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
