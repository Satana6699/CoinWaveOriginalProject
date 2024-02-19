using System;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static Coin_Wave_Lib.Player;

namespace Coin_Wave_Lib
{
    public class ExampleWindow : GameWindow
    {
        KeyboardState lastKeyboardState, currentKeyboardState;
        private float frameTime = 0.0f;
        private int fps = 0;
        Player player;

        uint[] indexes = new uint[] {
                0, 1, 2,
                0, 2, 3,
                3, 2, 4,
                3, 4, 5,
                5, 4, 6,
                5, 6, 7,

                1, 8, 9,
                1, 9, 2,
                2, 9, 10,
                2, 10, 4,
                4, 10, 11,
                4, 11, 6
        };

        double[] player_position = new double[]
        {
                // vertices           // colosrs 
                -0.8f,  0.6f, 0.0f,   1.0f, 0.0f, 0.0f, 1.0f,
                -0.8f,  0.0f, 0.0f,   0.0f, 1.0f, 0.0f, 1.0f,
                -0.2f,  0.0f, 0.0f,   0.0f, 0.0f, 1.0f, 1.0f,
                -0.2f,  0.6f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f,
                 0.2f,  0.0f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f,
                 0.2f,  0.6f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f,
                 0.8f,  0.0f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f,
                 0.8f,  0.6f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f,

                -0.8f,  -0.6f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f,
                -0.2f,  -0.6f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f,
                 0.2f,  -0.6f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f,
                 0.8f,  -0.6f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f
        };

        float[] vert_colors = new float[]
        {
                // vertices           // colosrs 
                -0.8f,  0.6f, 0.0f,   1.0f, 0.0f, 0.0f, 1.0f,
                -0.8f,  0.0f, 0.0f,   0.0f, 1.0f, 0.0f, 1.0f,
                -0.2f,  0.0f, 0.0f,   0.0f, 0.0f, 1.0f, 1.0f,
                -0.2f,  0.6f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f,
                 0.2f,  0.0f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f,
                 0.2f,  0.6f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f,
                 0.8f,  0.0f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f,
                 0.8f,  0.6f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f,

                -0.8f,  -0.6f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f,
                -0.2f,  -0.6f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f,
                 0.2f,  -0.6f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f,
                 0.8f,  -0.6f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f
        };

        private ShaderProgram shaderProgram;
        private ArrayObject vao;
        private BufferObject vboVC;
        private BufferObject ebo;

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

            GL.ClearColor(173 / 255.0f, 216 / 255.0f, 230 / 255.0f, 255 / 255.0f);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            // GL.PolygonMode(MaterialFace.Front, PolygonMode.Line);
            // GL.PolygonMode(MaterialFace.Back, PolygonMode.Point);

            shaderProgram = new ShaderProgram(@"Assets\shaders\shader_base.vert", @"Assets\shaders\shader_base.frag");
            double velocity = 0.003f;
            player = new(player_position, 12, velocity, velocity);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
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
            
            Click(currentKeyboardState);
            
            CreateVAO(player.GetPosition(), indexes);

            base.OnUpdateFrame(args);
        }

        private void CreateVAO(double[] vert_colors, uint[] indexes)
        {
            vboVC = new BufferObject(BufferType.ArrayBuffer);
            vboVC.SetData(vert_colors, BufferHint.StaticDraw);
            //vboVC.SetData(player_position, BufferHint.StaticDraw);

            ebo = new BufferObject(BufferType.ElementBuffer);
            ebo.SetData(indexes, BufferHint.StaticDraw);

            int VertexArray = shaderProgram.GetAttribProgram("aPosition");
            int ColorArray = shaderProgram.GetAttribProgram("aColor");

            vao = new ArrayObject();
            vao.Activate();

            vao.AttachBuffer(ebo);
            vao.AttachBuffer(vboVC);

            vao.AttribPointer(VertexArray, 3, AttribType.Double, 7 * sizeof(double), 0);
            vao.AttribPointer(ColorArray, 4, AttribType.Double, 7 * sizeof(double), 3 * sizeof(double));

            vao.Deactivate();
            vao.DisableAttribAll();
        }

        private void Draw()
        {
            shaderProgram.ActiveProgram();
            vao.Activate();
            vao.DrawElements(0, indexes.Length, ElementType.UnsignedInt);

            vao.Dispose();
            shaderProgram.DeactiveProgram();
        }

        //-----------------------------------------------------------------------

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            Draw();

            SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnUnload()
        {
            shaderProgram.DeleteProgram();
            base.OnUnload();
        }
        public void Click(KeyboardState currentKeyboardState)
        {
            var key = KeyboardState;
            switch (true)
            {
                case var _ when currentKeyboardState.IsKeyDown(Keys.W):
                    Console.WriteLine("Нажата клавиша W");
                    player.Movement(direction.Up);
                    break;
                case var _ when currentKeyboardState.IsKeyDown(Keys.A):
                    Console.WriteLine("Нажата клавиша A");
                    player.Movement(direction.Left);
                    break;
                case var _ when currentKeyboardState.IsKeyDown(Keys.S):
                    Console.WriteLine("Нажата клавиша S");
                    player.Movement(direction.Down);
                    break;
                case var _ when currentKeyboardState.IsKeyDown(Keys.D):
                    Console.WriteLine("Нажата клавиша D");
                    player.Movement(direction.Right);
                    break;
                default:
                    Console.WriteLine("Нажата другая клавиша");
                    break;
            }
        }
    }
}
