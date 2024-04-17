using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Coin_Wave_Lib.Objects.InterfaceObjects;
using Coin_Wave_Lib.Objects.GameObjects;

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
        TextureMap textureMap;
        Texture textureForMap;
        Player player;
        int indexTextureAir = 24;
        List<DynamicObject> dynamicObjects = new List<DynamicObject>();
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
            textureForMap = Texture.LoadFromFile(@"data\textureForGame\texMap.png");
            List<GameObject> first = GameObjectsList.CreateListForXml(FileRead.DeserializeObjectsToXml(@"data\maps\lvl1\first.xml"), textureForMap);
            List<GameObject> second = GameObjectsList.CreateListForXml(FileRead.DeserializeObjectsToXml(@"data\maps\lvl1\second.xml"), textureForMap);
            layers.first = new GameObject[sides.hidth, sides.widht];
            layers.second = new GameObject[sides.hidth, sides.widht];
            textureMap = new TextureMap(5, 5, 4, textureForMap);


            foreach (GameObject obj in first)
            {
                layers.first[obj.Index.y, obj.Index.x] = obj;
            }

            player = CreateSecondLayer(second);
            if (player is null) Close(); // Загрыть игру если нет игрока

            int speedPlayer = 15; //чем меньше значение тем быстрее игрок (так как это не прямая скорость, а частота обновления позиции)
            player.SetUnit
                (
                    player.RectangleWithTexture.Rectangle.GetWidth(),
                    player.RectangleWithTexture.Rectangle.GetHeight(),
                    speedPlayer
                );
            _numObj = player.Index;



            dynamicObjects = SearchDynamicObjects(layers.second);
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

            if (player.ContinueMove())
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

                if
                (
                    numObjFuture.y >= 0 &&
                    numObjFuture.x >= 0 &&
                    numObjFuture.x < layers.first.GetLength(1) &&
                    numObjFuture.y < layers.first.GetLength(0) &&
                    !layers.first[numObjFuture.y, numObjFuture.x].IsSolid &&
                    !layers.second[numObjFuture.y, numObjFuture.x].IsSolid ||
                    layers.second[numObjFuture.y, numObjFuture.x] is ICollectable
                )
                {
                    bool colisionDynamicSolid = true;

                    foreach (var dynamicObj in dynamicObjects)
                    {
                        if (dynamicObj.Index == (numObjFuture.x, numObjFuture.y))
                        {
                            colisionDynamicSolid = false;
                            break;
                        }
                    }
                    if(colisionDynamicSolid)
                    {
                        _numObj = numObjFuture;
                        player.Index = numObjFuture;
                    }
                }
                else
                    numObjFuture = _numObj;


            }
        
            // Дижение игрока
            if (layers.first[_numObj.y, _numObj.x] != null)
            {
                player.Move(layers.first[_numObj.y, _numObj.x]);
                player.UpdateDate(player.GetVertices());
            }

            // Собрать монетку
            if (layers.second[_numObj.y, _numObj.x].GetType().Name == typeof(Coin).Name)
            {
                player.ColletCoins(1);
                layers.second[_numObj.y, _numObj.x] = new Air
                (
                    new RectangleWithTexture
                    (
                        layers.second[_numObj.y, _numObj.x].RectangleWithTexture.Rectangle,
                        textureMap.GetTexturePoints(indexTextureAir)
                    ),
                    textureMap.Texture,
                    (_numObj.y, _numObj.x)
                )
                {
                    Name = typeof(Air).Name,
                };

             
            }




            //Камень упасть
            for (int i = 0; i < dynamicObjects.Count; i++)
            {
                if (dynamicObjects[i].ContinueMove() &&
                    dynamicObjects[i].Index.y+1 < layers.second.GetLength(0) &&
                    dynamicObjects[i].GetType().Name == typeof(Stone).Name &&
                    layers.second[dynamicObjects[i].Index.y+1, dynamicObjects[i].Index.x] is not null &&
                    !layers.first[dynamicObjects[i].Index.y+1, dynamicObjects[i].Index.x].IsSolid &&
                    !layers.second[dynamicObjects[i].Index.y+1, dynamicObjects[i].Index.x].IsSolid)
                {

                    dynamicObjects[i].Index = (dynamicObjects[i].Index.x, dynamicObjects[i].Index.y+1);

                    for(int j = 0; j < dynamicObjects.Count; j++)
                    {
                        if ((dynamicObjects[i].Index == dynamicObjects[j].Index &&
                             dynamicObjects[i] != dynamicObjects[j]) ||
                             dynamicObjects[i].Index == player.Index)
                        {
                            dynamicObjects[i].Index = (dynamicObjects[i].Index.x, dynamicObjects[i].Index.y - 1);
                            break;
                        }
                    }
                }

                dynamicObjects[i].Move(layers.second[dynamicObjects[i].Index.y, dynamicObjects[i].Index.x]);
                dynamicObjects[i].UpdateDate(dynamicObjects[i].GetVertices());
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
            foreach (var obj in dynamicObjects) obj.Render();
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

        private Player CreateSecondLayer(List<GameObject> second)
        {
            Player player = null;
            foreach (GameObject obj in second)
            {

                if (obj.Name == typeof(Player).Name)
                {
                    player = new Player
                    (
                        obj.RectangleWithTexture,
                        obj.Texture,
                        obj.Index
                    );


                    layers.second[obj.Index.y, obj.Index.x] = new Air
                    (
                        new RectangleWithTexture
                        (
                            obj.RectangleWithTexture.Rectangle,
                            textureMap.GetTexturePoints(indexTextureAir)
                        ),
                        textureForMap,
                        obj.Index
                    );
                }
                else layers.second[obj.Index.y, obj.Index.x] = obj;
            }

            return player;
        }
        private List<DynamicObject> SearchDynamicObjects(GameObject[,] gameObjects)
        {
            List<DynamicObject> dynamicObjects = new List<DynamicObject>();

            foreach (GameObject obj in gameObjects)
            {
                if (obj is IMoveable)
                {
                    Stone stone = new Stone
                        (
                            new RectangleWithTexture
                            (
                                obj.RectangleWithTexture.Rectangle,
                                obj.RectangleWithTexture.TexturePoints
                            ),
                            obj.Texture,
                            obj.Index
                        )
                    {
                        Name = typeof(Stone).Name
                    };
                    stone.SetUnit
                    (
                        stone.RectangleWithTexture.Rectangle.GetWidth(),
                        stone.RectangleWithTexture.Rectangle.GetHeight(),
                        20
                    );
                    dynamicObjects.Add
                    (
                        stone
                    );
                    layers.second[obj.Index.y, obj.Index.x] = new Air
                    (
                        new RectangleWithTexture
                        (
                            obj.RectangleWithTexture.Rectangle,
                            textureMap.GetTexturePoints(indexTextureAir)
                        ),
                        textureForMap,
                        obj.Index
                    )
                    {
                        Name = typeof(Air).Name
                    };
                    /*dynamicObjects.Add
                    (
                        new DynamicObject
                        (
                            obj
                        )
                    );*/
                }
            }

            return dynamicObjects;
        }
    }
}
