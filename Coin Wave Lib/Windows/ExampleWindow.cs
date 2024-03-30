using System;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static Coin_Wave_Lib.Player;
using System;
using System.Collections.Generic;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace Coin_Wave_Lib
{
    public class ExampleWindow : GameWindow
    {
        // размер карты 34 на 15
        KeyboardState lastKeyboardState, currentKeyboardState;
        private float frameTime = 0.0f;
        private int fps = 0;
        List<GameObject> gameObjects = new List<GameObject>();
        BufferManager bufferGameObject;

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

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            var path = @"data\maps\lvl1.xml";
            gameObjects = GameObjectsList.CreateListForXml(FileRead.DeserializeObjectsToXml(path));
            bufferGameObject = new(Obj.GetVertices(gameObjects.ToArray(), 5), @"data\textureForGame\texMap.png");
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


        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            // Alpha-chanal support
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            bufferGameObject.Render();

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


    }
}
