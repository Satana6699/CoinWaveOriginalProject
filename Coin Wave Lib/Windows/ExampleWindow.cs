using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Coin_Wave_Lib.Objects.InterfaceObjects;
using Coin_Wave_Lib.Objects.GameObjects;
using Coin_Wave_Lib.Objects.GameObjects.DynamicEntity;

namespace Coin_Wave_Lib
{
    public class ExampleWindow : GameWindow
    {
        KeyboardState lastKeyboardState, currentKeyboardState;
        private float frameTime = 0.0f;
        private int fps = 0;
        private (int widht, int hidth) sides = (32, 18);
        private (int x, int y) _numObj = (0, 0);
        private (GameObject[,] first, GameObject[,] second) layers;
        TextureMap textureMap;
        Texture textureForMap;
        Player player;
        List<DynamicObject> dynamicObjects = new List<DynamicObject>();
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

            MonsterMove();

            // Проверка на пересечение с монстрами для получения игроком урона от них
            DamagePlayer();

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
                            textureMap.GetTexturePoints(Resources.Air)
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
                    GameObjectFactory objFactory = ObjectFactory.GetFactory
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
                    if (gameObject is Stone) 
                        gameObject.SetSpeed(speedObj);
                    else if (gameObject is FireWheel) 
                        gameObject.SetSpeed(speedObj);
                    else if (gameObject is Monster) 
                        gameObject.SetSpeed(speedObj * 3);
                    dynamicObjects.Add(gameObject);

                    layers.second[obj.Index.y, obj.Index.x] = new Air
                        (
                            new RectangleWithTexture
                            (
                                obj.RectangleWithTexture.Rectangle,
                                textureMap.GetTexturePoints(Resources.Air)
                            ),
                            textureForMap,
                            obj.Index
                        )
                    {
                        Name = typeof(Air).Name
                    };
                }
            }

            return dynamicObjects;
        }
        private void PlayerSetTexture()
        {

            foreach (var obj in dynamicObjects)
            {
                if (player.Index == (obj.Index.x, obj.Index.y + 1) && obj is Stone)
                {
                    player.SetTexturePoints(textureMap.GetTexturePoints(Resources.PlayerAndStone));
                    break;
                }
                else 
                    player.SetTexturePoints(textureMap.GetTexturePoints(Resources.PlayerDefault));
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

                    foreach (var dynamicObj in dynamicObjects)
                    {
                        if (dynamicObj.Index == (numObjFuture.x, numObjFuture.y) && dynamicObj is Stone)
                        {
                            colisionDynamicSolid = true;
                            break;
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
                player.MoveInOneFrame(layers.first[_numObj.y, _numObj.x]);
                player.UpdateDate(player.GetVertices());
            }
        }
        private void AllRender()
        {
            foreach (var gameObject in layers.first) if (gameObject != null) gameObject.Render();
            foreach (var gameObject in layers.second) if (gameObject != null) gameObject.Render();
            foreach (var obj in dynamicObjects) obj.Render();
            player.Render();
            healthPanel.Render();
        }
        private void FallStone()
        {
            for (int i = 0; i < dynamicObjects.Count; i++)
            {
                if (dynamicObjects[i].ContinueMove() &&
                    dynamicObjects[i].Index.y + 1 < layers.second.GetLength(0) &&
                    dynamicObjects[i] is Stone &&
                    layers.second[dynamicObjects[i].Index.y + 1, dynamicObjects[i].Index.x] is not null &&
                    !layers.first[dynamicObjects[i].Index.y + 1, dynamicObjects[i].Index.x].IsSolid &&
                    !layers.second[dynamicObjects[i].Index.y + 1, dynamicObjects[i].Index.x].IsSolid)
                {

                    dynamicObjects[i].Index = (dynamicObjects[i].Index.x, dynamicObjects[i].Index.y + 1);

                    for (int j = 0; j < dynamicObjects.Count; j++)
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
                        textureMap.GetTexturePoints(Resources.Air)
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
            foreach (var thisObj in dynamicObjects)
            {
                if (thisObj is Stone &&
                    numObjFuture == thisObj.Index &&
                    (numObjFuture.x - 1, numObjFuture.y) == _numObj &&
                    !layers.second[thisObj.Index.y, thisObj.Index.x+1].IsSolid &&
                    !layers.first[thisObj.Index.y, thisObj.Index.x + 1].IsSolid)
                {
                    bool nextOdjSolid = false;
                    foreach(var nextObj in dynamicObjects)
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
            foreach (var thisObj in dynamicObjects)
            {
                if (thisObj is Stone &&
                    numObjFuture == thisObj.Index &&
                    (numObjFuture.x + 1, numObjFuture.y) == _numObj &&
                    !layers.second[thisObj.Index.y, thisObj.Index.x - 1].IsSolid &&
                    !layers.first[thisObj.Index.y, thisObj.Index.x - 1].IsSolid)
                {
                    bool nextOdjSolid = false;
                    foreach (var nextObj in dynamicObjects)
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

            textureMap = new TextureMap(Resources.textureMap.Width, Resources.textureMap.Height, 4, textureForMap);

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
                    textureMap.GetTexturePoints(Resources.HealthPanel)
                    ),
                textureForMap
            );


            dynamicObjects = SearchDynamicObjects(layers.second);
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
            foreach (var obj in dynamicObjects)
            {
                obj.Buffer.Dispouse();
            }
            player.Buffer.Dispouse();
            textureForMap.Dispouse();
            StartGame();
        }

        public void MonsterMove()
        {
            
            foreach(var obj in dynamicObjects)
            {

                if (obj is Monster monster)
                {
                    monster.Move(layers, dynamicObjects);
                }

                if (obj is Monkey monkey)
                {
                    switch (monkey.viewDirection)
                    {
                        case MoveHelper.Up:
                            monkey.SetTexturePoints(textureMap.GetTexturePoints(Resources.MonkeyUp));
                            break;
                        case MoveHelper.Down:
                            monkey.SetTexturePoints(textureMap.GetTexturePoints(Resources.MonkeyDown));
                            break;
                    }
                }

                if (obj is Dragon dragon)
                {
                    switch (dragon.viewDirection)
                    {
                        case MoveHelper.Left:
                            dragon.SetTexturePoints(textureMap.GetTexturePoints(Resources.DragonLeft));
                            break;
                        case MoveHelper.Right:
                            dragon.SetTexturePoints(textureMap.GetTexturePoints(Resources.DragonRight));
                            break;
                    }
                }

                obj.MoveInOneFrame(layers.second[obj.Index.y, obj.Index.x]);
                obj.UpdateDate(obj.GetVertices());
            }
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

        private void DamagePlayer()
        {
            if (layers.second[player.Index.y, player.Index.x] is Thorn thorn)
            {
                player.Damage(thorn.Damage());
            }
            foreach (var obj in dynamicObjects)
            {
                if (obj is Monster && player.RectangleWithTexture.Rectangle.Intersects(obj.RectangleWithTexture.Rectangle))
                {
                    player.Damage(1);
;               }
            }
        }
    }
}
