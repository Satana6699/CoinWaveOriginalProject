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

namespace Coin_Wave_Lib
{
    public class ExampleWindow : GameWindow
    {
        KeyboardState lastKeyboardState, currentKeyboardState;
        private float frameTime = 0.0f;
        private int fps = 0;
        PlayerFloat player;
        // Because we're adding a texture, we modify the vertex array to include texture coordinates.
        // Texture coordinates range from 0.0 to 1.0, with (0.0, 0.0) representing the bottom left, and (1.0, 1.0) representing the top right.
        // The new layout is three floats to create a vertex, then two floats to create the coordinates.
        private readonly float[] _vertices =
        {
            // Position         Texture coordinates
             0.1f,  0.1f, 0.0f, 1.0f, 1.0f, // top right
             0.1f, -0.1f, 0.0f, 1.0f, 0.0f, // bottom right
            -0.1f, -0.1f, 0.0f, 0.0f, 0.0f, // bottom left
            -0.1f,  0.1f, 0.0f, 0.0f, 1.0f  // top left
        };

        private readonly float[] _vertices2 =
        {
            // Position         Texture coordinates
            -0.7f,  0.1f, 0.0f, 1.0f, 1.0f, // top right
            -0.7f, -0.1f, 0.0f, 1.0f, 0.0f, // bottom right
            -0.9f, -0.1f, 0.0f, 0.0f, 0.0f, // bottom left
            -0.9f,  0.1f, 0.0f, 0.0f, 1.0f,  // top left
            // Position         Texture coordinates
             0.1f,  0.1f, 0.0f, 1.0f, 1.0f, // top right
             0.1f, -0.1f, 0.0f, 1.0f, 0.0f, // bottom right
            -0.1f, -0.1f, 0.0f, 0.0f, 0.0f, // bottom left
            -0.1f,  0.1f, 0.0f, 0.0f, 1.0f  // top left
        };

        private readonly uint[] _indices2 =
        {
            0, 1, 3,
            1, 2, 3,
            4, 5, 7,
            5, 6, 7
        };
        private readonly uint[] _indices =
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

        public ExampleWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            NameExampleWindow = "Coin Wave";
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
            player = new(_vertices, 5, 0.1f, 0.1f);

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            AllLoad();
        }

        

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            // Сохраняем предыдущее состояние клавиатуры
            lastKeyboardState = currentKeyboardState;
            // Получаем текущее состояние клавиатуры
            currentKeyboardState = KeyboardState.GetSnapshot();

            frameTime += (float)args.Time;
            fps++;
            if (frameTime >= 1.0f)
            {
                Title = $"OpenTK {NameExampleWindow} : FPS - {fps}";
                frameTime = 0.0f;
                fps = 0;
            }
            player.GetPosition()[0] += 0.001f;
            player.GetPosition()[5] += 0.001f;
            player.GetPosition()[10] += 0.001f;
            player.GetPosition()[15] += 0.001f;
            Click(currentKeyboardState);
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.BufferData(BufferTarget.ArrayBuffer, player.GetPosition().Length * sizeof(float), player.GetPosition(), BufferUsageHint.DynamicDraw);

            GL.BindVertexArray(_vertexArrayObject);

            _texture.Use(TextureUnit.Texture0);
            _shader.Use();

            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

            //-------

            GL.BindVertexArray(_vertexArrayObject2);

            _texture2.Use(TextureUnit.Texture0);
            _shader2.Use();

            GL.DrawElements(PrimitiveType.Triangles, _indices2.Length, DrawElementsType.UnsignedInt, 0);

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
        private void Click(KeyboardState currentKeyboardState)
        {
            var key = KeyboardState;
            switch (true)
            {
                case var _ when currentKeyboardState.IsKeyDown(Keys.W):
                    player.Movement(directionFloat.Up);
                    break;
                case var _ when currentKeyboardState.IsKeyDown(Keys.A):
                    player.Movement(directionFloat.Left);
                    break;
                case var _ when currentKeyboardState.IsKeyDown(Keys.S):
                    player.Movement(directionFloat.Down);
                    break;
                case var _ when currentKeyboardState.IsKeyDown(Keys.D):
                    player.Movement(directionFloat.Right);
                    break;
                default:
                    break;
            }
        }

        private void AllLoad()
        {
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, player.GetPosition().Length * sizeof(float), player.GetPosition(), BufferUsageHint.DynamicDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.DynamicDraw);

            // The shaders have been modified to include the texture coordinates, check them out after finishing the OnLoad function.
            _shader = new Shader(@"data\shaders\shader.vert", @"data\shaders\shader.frag");
            _shader.Use();

            // Because there's now 5 floats between the start of the first vertex and the start of the second,
            // we modify the stride from 3 * sizeof(float) to 5 * sizeof(float).
            // This will now pass the new vertex array to the buffer.
            var vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            // Next, we also setup texture coordinates. It works in much the same way.
            // We add an offset of 3, since the texture coordinates comes after the position data.
            // We also change the amount of data to 2 because there's only 2 floats for texture coordinates.
            var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            _texture = Texture.LoadFromFile(@"data\image\trava.png");
            _texture.Use(TextureUnit.Texture0);


            //////////////////////////////////////////



            _vertexArrayObject2 = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject2);

            _vertexBufferObject2 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject2);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices2.Length * sizeof(float), _vertices2, BufferUsageHint.DynamicDraw);

            _elementBufferObject2 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject2);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices2.Length * sizeof(uint), _indices2, BufferUsageHint.DynamicDraw);

            // The shaders have been modified to include the texture coordinates, check them out after finishing the OnLoad function.
            _shader2 = new Shader(@"data\shaders\shader.vert", @"data\shaders\shader.frag");
            _shader2.Use();

            // Because there's now 5 floats between the start of the first vertex and the start of the second,
            // we modify the stride from 3 * sizeof(float) to 5 * sizeof(float).
            // This will now pass the new vertex array to the buffer.
            var vertexLocation2 = _shader2.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            // Next, we also setup texture coordinates. It works in much the same way.
            // We add an offset of 3, since the texture coordinates comes after the position data.
            // We also change the amount of data to 2 because there's only 2 floats for texture coordinates.
            var texCoordLocation2 = _shader2.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            _texture2 = Texture.LoadFromFile(@"data\image\gamer.png");
            _texture2.Use(TextureUnit.Texture0);
        }
    }
}
