using System;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Coin_Wave_Lib;
using System;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Numerics;
using Coin_Wave_Lib.Objects.InterfaceObjects;
using System.Diagnostics;
using Coin_Wave_Lib.Objects.GameObjects.DynamicEntity;
using Coin_Wave_Lib.Programs;
using Buffer = Coin_Wave_Lib.Programs.Buffer;
using Coin_Wave_Lib.Objects.GameObjects.Boneses;
using Coin_Wave_Lib.Objects.GameObjects.NoSolidObjects;
using Coin_Wave_Lib.Objects.GameObjects.Traps;
using Coin_Wave_Lib.Objects.GameObjects.Player;
using Coin_Wave_Lib.Objects.GameObjects.SolidObjects;

namespace Coin_Wave_Lib.Windows
{
    /// <summary>
    /// Класс описывающий окно, которое позволяет пользователю генерировать игровую карту
    /// </summary>
    public class MapGenerateWindow : GameWindow
    {
        /// <summary>
        /// Статус нажатий клавиш клавиатуры
        /// </summary>
        public (KeyboardState last, KeyboardState current) keyboardState;

        /// <summary>
        /// Время пройденое от показа одного кадра до другого
        /// </summary>
        private float frameTime = 0.0f;

        /// <summary>
        /// Время, которое необходимо для подсчета количества кадров в секунду
        /// </summary>
        private float _time = 0.0f;

        /// <summary>
        /// Количество fps
        /// </summary>
        private int fps = 0;

        /// <summary>
        /// Счётчик кадров
        /// </summary>
        int frameCounter = 0;

        /// <summary>
        /// Активный слой в данный момент
        /// </summary>
        int layer = 1;
        
        /// <summary>
        /// Размер карты
        /// </summary>
        private (int width, int height) sidesMaps = (32, 18);

        /// <summary>
        /// Переменная отвечающая за то, стоит ли игрок на игровой карте
        /// </summary>
        private bool IsTherePlayer = false;

        /// <summary>
        /// Массивы слоёв карты
        /// </summary>
        private (GameObjectData[,] first, GameObjectData[,] second) layers;

        /// <summary>
        /// Лист для объектов, которые не являются объектами 
        /// игровой карты и которые описывают игровой интерфейс
        /// </summary>
        private List<InterfaceObject> interfaceObjects = new List<InterfaceObject>(0);

        /// <summary>
        /// Объект, описывающий объекта-выбора на панели редактора карты
        /// </summary>
        private CurrentPositionElement _currentPosition;

        /// <summary>
        /// Текстура для объекта-выбора
        /// </summary>
        Texture _textureCurrentPosition;

        /// <summary>
        /// Текстура для текстурной карты
        /// </summary>
        Texture _textureForMap;

        /// <summary>
        /// Массив пустых элементов
        /// </summary>
        private EmptyElement[,] _emptyElements;


        private float timeOfMoment;

        /// <summary>
        /// Текущее местоположения объекта-выбора относительно индекса игровой карты
        /// </summary>
        private (int x, int y) _numObj = (0, 0);

        /// <summary>
        /// Текущий индекс для выбора объекта в выборочной панели
        /// </summary>
        int currentIndex = 0;
        int index = 0;

        /// <summary>
        /// Объект, упрощающий создание игровой карты
        /// </summary>
        MapGenerate mg;

        /// <summary>
        /// Панель, описывающая панель, которая предоставляет выбор объекта. который далее можно 
        /// будет установить на какой либо слой игровой карты
        /// </summary>
        BlocksPanel blocksPanel;

        /// <summary>
        /// Объект для генерации текстурной карты, которая 
        /// позволяет получить текстурные координаты по индексу текстуры
        /// </summary>
        TextureMap textureMap;

        /// <summary>
        /// Объект, отображающийся на экране, если игровая карта 
        /// была успешно сохранена в файл
        /// </summary>
        InterfaceConcreteObj save;

        /// <summary>
        /// Объект, отображающийся на экране, 
        /// который показывает активный слов
        /// </summary>
        LayerInterface layerInterface;

        /// <summary>
        /// Текстурная карта для объектов конкретного слоя
        /// </summary>
        TextureMap textureMapLayerInt;

        /// <summary>
        /// Текстура
        /// </summary>
        Texture textureLayerInt;

        /// <summary>
        /// Переменная которая описывает успешное
        /// либо нет сохранения файла
        /// </summary>
        bool ifSaved;

        /// <summary>
        /// Значение описывающее оставшее количество 
        /// жизненных очков игрока в процентах
        /// </summary>
        int procentHealth = 100;

        /// <summary>
        /// Путь к XML файлу для первого слоя.
        /// Файл по этому пути будет полностью перезаписан
        /// или создан при его отсутствии
        /// </summary>
        string filePathFirstLayer;

        /// <summary>
        /// Путь к XML файлу для второго слоя.
        /// Файл по этому пути будет полностью перезаписан
        /// или создан при его отсутствии
        /// </summary>
        string filePathSecondLayer;

        /// <summary>
        /// Конструктор для генерации игрового окна
        /// </summary>
        /// <param name="gameWindowSettings"> Настройки игрового окна </param>
        /// <param name="nativeWindowSettings"> Параметры игрового окна </param>
        /// <param name="filePathFirstLayer"> Путь к файлу, в котором хранится первый слой карты </param>
        /// <param name="filePathSecondLayer"> Путь к файлу, в котором хранится второй слой карты </param>
        public MapGenerateWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings, string filePathFirstLayer, string filePathSecondLayer)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            NameExampleWindow = "Coin Wave Map Generator";
            Title = NameExampleWindow;

            Console.WriteLine(GL.GetString(StringName.Version));
            Console.WriteLine(GL.GetString(StringName.Vendor));
            Console.WriteLine(GL.GetString(StringName.Renderer));
            Console.WriteLine(GL.GetString(StringName.ShadingLanguageVersion));

            VSync = VSyncMode.On;
            this.filePathFirstLayer = filePathFirstLayer;
            this.filePathSecondLayer = filePathSecondLayer;
        }
        /// <summary>
        /// Имя окна
        /// </summary>
        public string NameExampleWindow { private set; get; }

        /// <summary>
        /// Метод, отвечающий за загрузку всех необходимых ресурсов
        /// </summary>
        protected override void OnLoad()
        {
            
            base.OnLoad();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            layers.first = new GameObjectData[sidesMaps.height, sidesMaps.width];
            layers.second = new GameObjectData[sidesMaps.height, sidesMaps.width];
            _textureForMap = Texture.LoadFromFile(@"data\textureForGame\texMap.png");
            save = new InterfaceConcreteObj
                (
                    new RectangleWithTexture
                    (
                        new Rectangle(new Point(-0.99,0.9, 0), 0.3, 0.1),
                        TexturePoint.Default()
                    ),
                    Texture.LoadFromFile(@"data\textureForInterface\save.png")
                );
            textureLayerInt = Texture.LoadFromFile(@"data\textureForInterface\layers.png");
            textureMapLayerInt = new TextureMap(2, 1, 4, textureLayerInt);
            layerInterface = new LayerInterface
                (
                    new RectangleWithTexture
                    (
                        new Rectangle(new Point(-1, 0.9, 0), 0.07, 0.08),
                        textureMapLayerInt.GetTexturePoints(0)
                    ),
                    textureLayerInt
                );
            mg = new(sidesMaps.width, sidesMaps.height, 0.08, 0.2, 0.01);
            _emptyElements = new EmptyElement[sidesMaps.height, sidesMaps.width];
            Texture emptyTexture = Texture.LoadFromFile(@"data\textureForInterface\empty.png");
            for (int i = 0; i < _emptyElements.GetLength(0); i++)
                for (int j = 0; j < _emptyElements.GetLength(1); j++)
                {
                    _emptyElements[i,j] = new(mg.RectangleWithTextures[i, j], emptyTexture);
                }
            _currentPosition = new(_emptyElements[0,0].RectangleWithTexture, Texture.LoadFromFile(@"data\textureForInterface\redsqrt.png"));
                


            textureMap = new TextureMap(Resources.textureMap.Width, Resources.textureMap.Height, 4, _textureForMap);
            blocksPanel = new
            (
                new RectangleWithTexture
                (
                new Rectangle
                (
                    new Point(-0.9, 0.7, 0),
                    1.8, 
                    1.3),
                    new TexturePoint[] { new TexturePoint(0, 1), new TexturePoint(1, 1), new TexturePoint(1, 0), new TexturePoint(0, 0)}
                ),
                16,
                3,
                Texture.LoadFromFile(@"data\textureForInterface\bluesqrt.png"),
                textureMap
            );

            HealthPanel healthPanel = new HealthPanel
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
                _textureForMap
            );

            healthPanel.RealTexturePoints(100);
            healthPanel.UpdateDate(healthPanel.GetVertices());
            interfaceObjects.Add(healthPanel);

            // Стены и камни
            blocksPanel.GenerateMenuElement(typeof(SolidWall).Name, Resources.SolidWall);
            blocksPanel.GenerateMenuElement(typeof(BackWall).Name, Resources.BackWall);
            blocksPanel.GenerateMenuElement(typeof(Stone).Name, Resources.Stone);

            // Не знаю как класифицировать
            blocksPanel.GenerateMenuElement(typeof(Player).Name, Resources.PlayerDefault);
            blocksPanel.GenerateMenuElement(typeof(Coin).Name, Resources.Coin);

            // Монстры
            blocksPanel.GenerateMenuElement(typeof(Dragon).Name, Resources.DragonRight);
            blocksPanel.GenerateMenuElement(typeof(FireWheel).Name, Resources.FireWhell);
            blocksPanel.GenerateMenuElement(typeof(Monkey).Name, Resources.MonkeyUp);


            // Ловушки
            blocksPanel.GenerateMenuElement(typeof(Thorn).Name, Resources.ActiveThorn);
            blocksPanel.GenerateMenuElement(typeof(TrapFire).Name, Resources.TrapFire);

            // Бонусы
            blocksPanel.GenerateMenuElement(typeof(HealthUpBonus).Name, Resources.HealthBonus);
            blocksPanel.GenerateMenuElement(typeof(SpeedUpBonus).Name, Resources.SpeedUpBonus);
            blocksPanel.GenerateMenuElement(typeof(SpeedDownBonus).Name, Resources.SpeedDownBonus);


            _textureCurrentPosition = Texture.LoadFromFile(@"data\textureForInterface\redsqrt.png");
            blocksPanel.GenerateTexturViborObj(_textureCurrentPosition);

        }

        /// <summary>
        /// Метод, который циклически вызывается.
        /// В данном методе реализована вся математическая логика
        /// </summary>
        /// <param name="args"> Аргументы игрового окна, необходимые для коректной работы данного метода </param>
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            keyboardState.last = keyboardState.current;
            keyboardState.current = KeyboardState.GetSnapshot();

            frameTime += (float)args.Time;
            _time += (float)args.Time;
            fps++;
            if (frameTime >= 1.0f)
            {
                Title = $"OpenTK {NameExampleWindow} : FPS - {fps}";
                frameTime = 0.0f;
                fps = 0;
            }

            
            blocksPanel.ObjVibor(index);
            blocksPanel.choiceObj.UpdateDate(blocksPanel.choiceObj.GetVertices());

            
            ClickWASD(keyboardState.current);  // Утечка памяти
            ClickShift();

            
            switch (keyboardState.current)
            {
                case var _ when keyboardState.current.IsKeyDown(Keys.D1):
                    layer = 1;
                    layerInterface.SetTexturePoints(textureMapLayerInt.GetTexturePoints(layer - 1));
                    layerInterface.UpdateDate(layerInterface.GetVertices());
                    break;
                case var _ when keyboardState.current.IsKeyPressed(Keys.D2):
                    layer = 2;
                    layerInterface.SetTexturePoints(textureMapLayerInt.GetTexturePoints(layer - 1));
                    layerInterface.UpdateDate(layerInterface.GetVertices());
                    break;
            }
            if (layer == 1)
            {
                if (keyboardState.current.IsKeyDown(Keys.Enter)) ClickEnter(layers.first);
                if (keyboardState.current.IsKeyDown(Keys.Delete)) ClickDelete(layers.first);
            }
            if (layer == 2)
            {
                if (keyboardState.current.IsKeyDown(Keys.Enter)) ClickEnter(layers.second);
                if (keyboardState.current.IsKeyDown(Keys.Delete)) ClickDelete(layers.second);
            }
            if (keyboardState.current.IsKeyDown(Keys.Escape)) Close();
            if (keyboardState.last != null &&
                keyboardState.last.IsKeyDown(Keys.LeftControl) &&
                keyboardState.current.IsKeyDown(Keys.S) && 
                IsTherePlayer)
            {
                // Создать промежуточные массивы
                (GameObjectData[,] first, GameObjectData[,] second) temporaryLayers;
                temporaryLayers.first = new GameObjectData[layers.first.GetLength(0), layers.first.GetLength(1)];
                temporaryLayers.second = new GameObjectData[layers.second.GetLength(0), layers.second.GetLength(1)];
                // Если имеются в массиве незаполненные элементы массива, то заполнить их объктом воздух
                // Для того, чтобы в массиве не было null объектов
                for (int i = 0; i < temporaryLayers.first.GetLength(0); i++)
                    for (int j = 0; j < temporaryLayers.first.GetLength(1); j++)
                    {
                        GameObjectData ob = new GameObjectData
                        {
                            RectangleWithTexture = new RectangleWithTexture
                                (
                                    new Rectangle(mg.RectangleWithTextures[i, j].Rectangle.TopLeft, mg.units.X, mg.units.Y),
                                    textureMap.GetTexturePoints(Resources.Air)
                                ),
                            Index = (j,i),
                            Name = typeof(Air).Name,
                            Texture = _textureForMap
                        };


                        if (layers.first[i,j] is null)
                        {
                            temporaryLayers.first[i, j] = ob;
                        }
                        else
                        {
                            temporaryLayers.first[i, j] = layers.first[i, j];
                        }

                        if (layers.second[i, j] is null)
                        {
                            temporaryLayers.second[i, j] = ob;
                        }
                        else
                        {
                            temporaryLayers.second[i, j] = layers.second[i, j];
                        }
                    }

                ifSaved = FileSave.SerializeObjectsToXml(temporaryLayers.first.Cast<GameObjectData>().ToArray(), filePathFirstLayer);
                if (ifSaved)
                    ifSaved = FileSave.SerializeObjectsToXml(temporaryLayers.second.Cast<GameObjectData>().ToArray(), filePathSecondLayer);
               
            }
            if (ifSaved) ifSaved = save.IsLive((float)args.Time);


            // Обновлять HealthPanel для проверки корректности ее работы
            UpdateHealth();

        }

        /// <summary>
        /// Метод, вызывающий циклически после метода OnLoadFrame.
        /// Данный метод отвечает за отрисовку объектов в каждом кадре
        /// </summary>
        /// <param name="args"></param>
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

            // Alpha-chanal support
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            foreach (EmptyElement eE in _emptyElements) eE.Render();
            foreach (var gO in layers.first) if (gO != null) gO.Render();
            foreach (var gO in layers.second) if (gO != null) gO.Render();
            //foreach (var interfaceObj in interfaceObjects) if (interfaceObj != null) interfaceObj.Render();
            _currentPosition.Render();

            if (keyboardState.current.IsKeyDown(Keys.LeftShift))
            {
                blocksPanel.Render();
                blocksPanel.choiceObj.Render();
                foreach(var v in blocksPanel.MenuElements) v.Render();
            }
            if (ifSaved)
            {
                save.Render();
            }
            layerInterface.Render();
            SwapBuffers();
        }

            /// <summary>
            /// Здесь проиходит выгрузка всех ресурсов.
            /// Метод вызывается во время закрытия окна
            /// </summary>
        protected override void OnUnload()
        {
            base.OnUnload();
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        /// <summary>
        /// Метод, отслеживающий нажатие клавиш, для управления объектом выбора
        /// </summary>
        /// <param name="keyboardState"></param>
        private async void ClickWASD(KeyboardState keyboardState)
        {
            float pressingTime = 0.6f;
            (int x, int y) numObjFuture = _numObj;

            
            switch (true)
            {
                case var _ when keyboardState.IsKeyPressed(Keys.W):
                    numObjFuture.y -= 1;
                    timeOfMoment = _time;
                    break;
                case var _ when keyboardState.IsKeyPressed(Keys.A):
                    numObjFuture.x -= 1;
                    timeOfMoment = _time;
                    break;
                case var _ when keyboardState.IsKeyPressed(Keys.S):
                    numObjFuture.y += 1;
                    timeOfMoment = _time;
                    break;
                case var _ when keyboardState.IsKeyPressed(Keys.D):
                    numObjFuture.x += 1;
                    timeOfMoment = _time;
                    break;
                default:
                    break;
            }
            int operationFrequency = 6;
            frameCounter ++;
            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.S) ||
                keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.W))
                if (_time - timeOfMoment >= pressingTime && keyboardState.IsKeyDown(Keys.W) && frameCounter >= operationFrequency)
                {
                    numObjFuture.y -= 1;
                    frameCounter = 0;
                }
                else if (_time - timeOfMoment >= pressingTime && keyboardState.IsKeyDown(Keys.A) && frameCounter >= operationFrequency)
                {
                    numObjFuture.x -= 1;
                    frameCounter = 0;
                }
                else if (_time - timeOfMoment >= pressingTime && keyboardState.IsKeyDown(Keys.S) && frameCounter >= operationFrequency)
                {
                    numObjFuture.y += 1;
                    frameCounter = 0;
                }
                else if (_time - timeOfMoment >= pressingTime && keyboardState.IsKeyDown(Keys.D) && frameCounter >= operationFrequency)
                {
                    numObjFuture.x += 1;
                    frameCounter = 0;
                }


            if 
            (
                numObjFuture.y >= 0 &&
                numObjFuture.x >= 0 &&
                numObjFuture.x < _emptyElements.GetLength(1) &&
                numObjFuture.y < _emptyElements.GetLength(0)
            )
                _numObj = numObjFuture;
            else
                numObjFuture = _numObj;


            Rectangle newRect = mg.RectangleWithTextures[_numObj.y, _numObj.x].Rectangle;
            _currentPosition.RectangleWithTexture = new RectangleWithTexture(newRect, mg.RectangleWithTextures[_numObj.y, _numObj.x].TexturePoints);
            _currentPosition.UpdateDate(_currentPosition.GetVertices());
        }

        /// <summary>
        /// Метод, который отслеживает нажатие клавиши Enter
        /// В данном методе прописана логика, которая будет исполена, 
        /// если клавиша Enter, будет нажата
        /// </summary>
        /// <param name="gameObjectData"> Массив игровых объекто, которые лежат в каком либо слое </param>
        private void ClickEnter(GameObjectData[,] gameObjectData)
        {
            GameObjectData ob = new GameObjectData
            {
                RectangleWithTexture = new RectangleWithTexture
                    (
                        new Rectangle(mg.RectangleWithTextures[_numObj.y, _numObj.x].Rectangle.TopLeft, mg.units.X, mg.units.Y),
                        textureMap.GetTexturePoints(blocksPanel.MenuElements[currentIndex].IndexTexture)
                    ),
                Index = _numObj,
                Name = blocksPanel.MenuElements[currentIndex].Name,
                Texture = _textureForMap
            };
            if (blocksPanel.MenuElements[currentIndex].Name != typeof(Player).Name)
            {
                ob.SetBuffer(new Buffer(ob.GetVertices()));
                gameObjectData[_numObj.y, _numObj.x] = ob;
            }
            else if (blocksPanel.MenuElements[currentIndex].Name == typeof(Player).Name && !IsTherePlayer)
            {
                ob.SetBuffer(new Buffer(ob.GetVertices()));
                gameObjectData[_numObj.y, _numObj.x] = ob;
                IsTherePlayer = true;
            }
        }

        /// <summary>
        /// Метод, который отслеживает нажатие клавиши Delete
        /// В данном методе прописана логика, которая будет исполена, 
        /// если клавиша Delete, будет нажата
        /// </summary>
        /// <param name="gameObjectData"> Массив игровых объекто, которые лежат в каком либо слое </param>
        private void ClickDelete(GameObjectData[,] gameObjectData)
        {
            if (gameObjectData[_numObj.y, _numObj.x] != null &&
                gameObjectData[_numObj.y, _numObj.x].Name == typeof(Player).Name && 
                IsTherePlayer)
            {
                IsTherePlayer = false;
            }
            gameObjectData[_numObj.y, _numObj.x] = null;
        }

        /// <summary>
        /// Метод, который отслеживает нажатие клавиши Shift
        /// В данном методе прописана логика, которая будет исполена, 
        /// если клавиша Shift, будет нажата
        /// </summary>
        private void ClickShift()
        {
            if (
                    keyboardState.current.IsKeyPressed(Keys.Tab) &&
                    keyboardState.last.IsKeyDown(Keys.LeftShift) &&
                    index < blocksPanel.MenuElements.Count - 1
                    )
            {
                index++;
                currentIndex = index;
            }
            else if (
                    (!keyboardState.current.IsKeyPressed(Keys.LeftShift) &&
                    index >= blocksPanel.MenuElements.Count - 1 &&
                    keyboardState.current.IsKeyPressed(Keys.Tab)) ||
                    !keyboardState.current.IsKeyDown(Keys.LeftShift)
                    )
            {
                index = 0;
            }
            else if (keyboardState.current.IsKeyPressed(Keys.LeftShift))
            {
                index = 0;
                currentIndex = index;
            }
        }

        /// <summary>
        /// Метод отвечающий за обновление полоски прогресса,
        /// которая показывает текущие жизненные очки персонажа
        /// </summary>
        public void UpdateHealth()
        {
            if (frameTime >= 0.0f && frameTime <= 0.05)
                procentHealth--;
            if ( procentHealth < 0 )
            {
                procentHealth = 100;
            }

            foreach (var obj in interfaceObjects)
            {
                if (obj is HealthPanel healthPanel)
                {
                    ((HealthPanel)interfaceObjects[0]).RealTexturePoints(procentHealth);
                    interfaceObjects[0].UpdateDate(interfaceObjects[0].GetVertices());
                }
            }
        }
    }
}