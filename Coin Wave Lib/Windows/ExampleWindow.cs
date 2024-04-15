using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Coin_Wave_Lib.Objects.InterfaceObjects;

namespace Coin_Wave_Lib
{
    public class ExampleWindow : GameWindow
    {
        // размер карты 34 на 15
        KeyboardState lastKeyboardState, currentKeyboardState;
        private float frameTime = 0.0f;
        private int fps = 0;
        private (int widht, int hidth) sides = (32, 18);
        private (int x, int y) _numObj = (0, 0);
        private (GameObject[,] first, GameObject[,] second) layers;
        Player player;
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

            List<GameObject> gameObjects = new List<GameObject>();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            Texture textureMap = Texture.LoadFromFile(@"data\textureForGame\texMap.png");
            List<GameObject> first = GameObjectsList.CreateListForXml(FileRead.DeserializeObjectsToXml(@"data\maps\lvl1\first.xml"), textureMap);
            List<GameObject> second = GameObjectsList.CreateListForXml(FileRead.DeserializeObjectsToXml(@"data\maps\lvl1\second.xml"), textureMap);
            layers.first = new GameObject[sides.hidth, sides.widht];
            layers.second = new GameObject[sides.hidth, sides.widht];
            foreach (GameObject obj in first)
            {
                layers.first[obj.Index.y, obj.Index.x] = obj;
            }
            foreach (GameObject obj in second)
            {
                
                if (obj.Name == typeof(Player).Name) player = new Player
                                                              (
                                                                  obj.RectangleWithTexture,
                                                                  obj.Texture,
                                                                  obj.Index
                                                              );
                else layers.second[obj.Index.y, obj.Index.x] = obj;
            }

            player.SetUnit
                (
                    player.RectangleWithTexture.Rectangle.GetWidth(),
                    player.RectangleWithTexture.Rectangle.GetHeight(),
                    40
                );
            _numObj = player.Index;
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

            if (player.FrameTime == player.Time)
            {
                (int x, int y) numObjFuture = _numObj;
                switch (true)
                {
                    case var _ when KeyboardState.IsKeyDown(Keys.W):
                        numObjFuture.y -= 1;
                        break;
                    case var _ when KeyboardState.IsKeyDown(Keys.A):
                        numObjFuture.x -= 1;
                        break;
                    case var _ when KeyboardState.IsKeyDown(Keys.S):
                        numObjFuture.y += 1;
                        break;
                    case var _ when KeyboardState.IsKeyDown(Keys.D):
                        numObjFuture.x += 1;
                        break;
                }
                if (numObjFuture.y >= 0 &&
                numObjFuture.x >= 0 &&
                    numObjFuture.x < layers.first.GetLength(1) &&
                    numObjFuture.y < layers.first.GetLength(0) &&
                    !layers.first[numObjFuture.y, numObjFuture.x].IsSolid )
                    _numObj = numObjFuture;
                else
                    numObjFuture = _numObj;
            }
        
            if (layers.first[_numObj.y, _numObj.x] != null)
            {
                player.Move(layers.first[_numObj.y, _numObj.x]);
                player.UpdateDate(player.GetVertices());
            }
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            // Alpha-chanal support
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            foreach (var gameObject in layers.first) if (gameObject != null)  gameObject.Render();
            foreach (var gameObject in layers.second) if (gameObject != null)  gameObject.Render();
            player.Render();

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
