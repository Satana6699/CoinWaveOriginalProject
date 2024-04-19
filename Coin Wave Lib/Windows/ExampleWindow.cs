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
        List<DynamicObject> stones = new List<DynamicObject>();
        int speedObj = 15;
        HealthPanel healthPanel;
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

            StartGame();
        }

        

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            // Сохраняем предыдущее состояние клавиатуры
            lastKeyboardState = currentKeyboardState;
            // Получаем текущее состояние клавиатуры
            currentKeyboardState = KeyboardState.GetSnapshot();

            // Обновить FPS
            UpdateFPS(args);

            // Обновить позицию игрока
            PlayerMove();

            // Обновить состояние игрока (его текстуру)
            PlayerSetTexture();

            // Собрать монетку
            CollectMoney();

            //Камень упасть
            FallStone();

            Monsters();

            UpdateHealthPanel();
            // Проверка на проигрыш
            GameOver();

            // Новое движение
            {
                FoolMove();
            }
            if (currentKeyboardState.IsKeyPressed(Keys.Escape)) Close();
            if (currentKeyboardState.IsKeyPressed(Keys.R)) RestarrtGame();
        }


        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            // Alpha-chanal support
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            AllRender();

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
                if (obj is IDynamic)
                {
                    GameObjectFactory objFactory = ObjectFactory.GetFactoy
                        (
                            obj.Name,
                                new RectangleWithTexture
                                    (
                                        obj.RectangleWithTexture.Rectangle,
                                        obj.RectangleWithTexture.TexturePoints
                                    ),
                            textureForMap,
                            obj.Index
                        );

                    DynamicObject gameObject = (DynamicObject)objFactory.GetGameObject();
                    gameObject.SetSpeed(speedObj);
                    dynamicObjects.Add(gameObject);

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
                }


                /*if (obj is Stone)
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
                    stone.SetSpeed
                    (
                        speedObj
                    );
                    dynamicObjects.Add
                    (
                        stone
                    );
                    
                }*/
            }

            return dynamicObjects;
        }
        private void PlayerSetTexture()
        {
            int playerHoldingStoneTextureIndex = 18;
            int playerNotHoldingStoneTextureIndex = 15;

            foreach (var obj in stones)
            {
                if (player.Index == (obj.Index.x, obj.Index.y + 1) && obj is Stone)
                {
                    player.SetTexturePoints(textureMap.GetTexturePoints(playerHoldingStoneTextureIndex));
                    break;
                }
                else 
                    player.SetTexturePoints(textureMap.GetTexturePoints(playerNotHoldingStoneTextureIndex));
            }

        }

        private void PlayerMove()
        {
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
                        MoveStoneLeft(numObjFuture); 
                        break;
                    case var _ when KeyboardState.IsKeyDown(Keys.S):
                        numObjFuture.y += 1;
                        break;
                    case var _ when KeyboardState.IsKeyDown(Keys.D):
                        numObjFuture.x += 1;
                        MoveStoneRight(numObjFuture);
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
                    bool colisionDynamicSolid = false;

                    foreach (var dynamicObj in stones)
                    {
                        if (dynamicObj.Index == (numObjFuture.x, numObjFuture.y) && dynamicObj is Stone)
                        {
                            colisionDynamicSolid = true;
                            break;
                        }
                        else if (dynamicObj.Index == (numObjFuture.x, numObjFuture.y) && dynamicObj is Monster)
                        {
                            player.Damage(1);
                        }
                    }
                    if (!colisionDynamicSolid)
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
        }
        private void AllRender()
        {
            foreach (var gameObject in layers.first) if (gameObject != null) gameObject.Render();
            foreach (var gameObject in layers.second) if (gameObject != null) gameObject.Render();
            foreach (var obj in stones) obj.Render();
            player.Render();
            healthPanel.Render();
        }
        private void FallStone()
        {
            for (int i = 0; i < stones.Count; i++)
            {
                if (stones[i].ContinueMove() &&
                    stones[i].Index.y + 1 < layers.second.GetLength(0) &&
                    stones[i].GetType().Name == typeof(Stone).Name &&
                    layers.second[stones[i].Index.y + 1, stones[i].Index.x] is not null &&
                    !layers.first[stones[i].Index.y + 1, stones[i].Index.x].IsSolid &&
                    !layers.second[stones[i].Index.y + 1, stones[i].Index.x].IsSolid)
                {

                    stones[i].Index = (stones[i].Index.x, stones[i].Index.y + 1);

                    for (int j = 0; j < stones.Count; j++)
                    {
                        if ((stones[i].Index == stones[j].Index &&
                             stones[i] != stones[j]) ||
                             stones[i].Index == player.Index)
                        {
                            stones[i].Index = (stones[i].Index.x, stones[i].Index.y - 1);
                            break;
                        }
                    }
                }

                stones[i].Move(layers.second[stones[i].Index.y, stones[i].Index.x]);
                stones[i].UpdateDate(stones[i].GetVertices());
            }
        }
        private void CollectMoney()
        {
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
        }

        private void UpdateFPS(FrameEventArgs args)
        {
            frameTime += (float)args.Time;
            fps++;
            if (frameTime >= 1.0f)
            {
                Title = $"OpenTK {NameExampleWindow} : FPS - {fps}";
                frameTime = 0.0f;
                fps = 0;
            }
        }

        private void MoveStoneRight((int x, int y) numObjFuture)
        {
            foreach (var thisObj in stones)
            {
                if (thisObj is Stone &&
                    numObjFuture == thisObj.Index &&
                    (numObjFuture.x - 1, numObjFuture.y) == _numObj &&
                    !layers.second[thisObj.Index.y, thisObj.Index.x+1].IsSolid &&
                    !layers.first[thisObj.Index.y, thisObj.Index.x + 1].IsSolid)
                {
                    bool nextOdjSolid = false;
                    foreach(var nextObj in stones)
                    {
                        if ((thisObj.Index.x + 1, thisObj.Index.y) == nextObj.Index)
                        {
                            nextOdjSolid = true;
                            break;
                        }
                    }
                    if (!nextOdjSolid)
                        thisObj.Index = (thisObj.Index.x + 1, thisObj.Index.y);
                }
            }
        }
        private void MoveStoneLeft((int x, int y) numObjFuture)
        {
            foreach (var thisObj in stones)
            {
                if (thisObj is Stone &&
                    numObjFuture == thisObj.Index &&
                    (numObjFuture.x + 1, numObjFuture.y) == _numObj &&
                    !layers.second[thisObj.Index.y, thisObj.Index.x - 1].IsSolid &&
                    !layers.first[thisObj.Index.y, thisObj.Index.x - 1].IsSolid)
                {
                    bool nextOdjSolid = false;
                    foreach (var nextObj in stones)
                    {
                        if ((thisObj.Index.x - 1, thisObj.Index.y) == nextObj.Index)
                        {
                            nextOdjSolid = true;
                            break;
                        }
                    }
                    if (!nextOdjSolid)
                        thisObj.Index = (thisObj.Index.x - 1, thisObj.Index.y);
                }
            }
        }

        private void StartGame()
        {
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

            player.SetSpeed
                (
                    speedObj
                );
            _numObj = player.Index;


            healthPanel = new HealthPanel
            (
                new RectangleWithTexture
                (
                    new Rectangle
                    (
                        new Point(-0.9, 0.89, 0),
                        0.4,
                        0.06
                    ),
                    textureMap.GetTexturePoints(8)
                    ),
                textureForMap
            );


            stones = SearchDynamicObjects(layers.second);
        }

        private void RestarrtGame()
        {
            for (int i = 0; i < layers.first.GetLength(0); i++)
                for(int j = 0; j < layers.first.GetLength(1); j++)
                {
                    layers.first[i, j].Buffer.Dispouse();
                    layers.first[i, j].Texture.Dispouse();
                    layers.second[i, j].Buffer.Dispouse();
                    layers.second[i,j].Texture.Dispouse();
                    
                }
            foreach (var obj in stones)
            {
                obj.Buffer.Dispouse();
            }
            player.Buffer.Dispouse();
            textureForMap.Dispouse();
            StartGame();
        }

        public void Monsters()
        {

        }

        private void FoolMove()
        {
            
        }

        private void GameOver()
        {
            if(player.HealthPoint <= 0)
            {
                Close();
            }
        }

        private void UpdateHealthPanel()
        {
            int percanteHealthPoint = Convert.ToInt32(  ((double)player.HealthPoint / player.MaxHealthPoint) * 100);
            healthPanel.RealTexturePoints(percanteHealthPoint);
            healthPanel.UpdateDate(healthPanel.GetVertices());
        }
    }
}
