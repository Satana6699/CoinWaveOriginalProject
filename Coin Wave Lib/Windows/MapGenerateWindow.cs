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

namespace Coin_Wave_Lib
{
    public class MapGenerateWindow : GameWindow
    {
        public (KeyboardState last, KeyboardState current) keyboardState;
        private float frameTime = 0.0f;
        private float _time = 0.0f;
        private int fps = 0;
        int frameCounter = 0;
        int layer = 1;
        // размер карты 34 на 18 и разрешение экрана 1920 на 1080
        private (int width, int height) sidesMaps = (32, 18);
        private bool IsTherePlayer = false;

        // Создание всех списков и массивов тут
        private (GameObjectData[,] first, GameObjectData[,] second) layers;
        private List<InterfaceObject> interfaceObjects = new List<InterfaceObject>(0);

        private CurrentPositionElement _currentPosition;
        Texture _textureCurrentPosition;
        Texture _textureForMap;
        private EmptyElement[,] _emptyElements;


        private float timeOfMoment;
        private (int x, int y) _numObj = (0, 0);
        int currentIndex = 0;
        int index = 0;
        MapGenerate mg;
        BlocksPanel blocksPanel;
        TextureMap textureMap;
        InterfaceConcreteObj save;
        LayerInterface layerInterface;
        TextureMap textureMapLayerInt;
        Texture textureLayerInt;
        bool ifSaved;
        int procentHealth = 100;
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

            // Doors
            blocksPanel.GenerateMenuElement(typeof(StartDoor).Name, Resources.StartDoor);
            blocksPanel.GenerateMenuElement(typeof(ExitDoor).Name, Resources.ExitDoor);

            // Ловушки
            blocksPanel.GenerateMenuElement(typeof(Thorn).Name, Resources.ActiveThorn);
            blocksPanel.GenerateMenuElement(typeof(TrapFire).Name, Resources.TrapFire);

            // Просто так
            blocksPanel.GenerateMenuElement(typeof(Player).Name, Resources.SolidWall);
            blocksPanel.GenerateMenuElement(typeof(Coin).Name, Resources.SolidWall);
            blocksPanel.GenerateMenuElement(typeof(ExitDoor).Name, Resources.SolidWall);
            blocksPanel.GenerateMenuElement(typeof(Dragon).Name, Resources.SolidWall);
            blocksPanel.GenerateMenuElement(typeof(Monkey).Name, Resources.SolidWall);

            _textureCurrentPosition = Texture.LoadFromFile(@"data\textureForInterface\redsqrt.png");
            blocksPanel.GenerateTexturViborObj(_textureCurrentPosition);

        }

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

                ifSaved = FileSave.SerializeObjectsToXml(temporaryLayers.first.Cast<GameObjectData>().ToArray(), @"data\maps\lvl1\first.xml");
                if (ifSaved)
                    ifSaved = FileSave.SerializeObjectsToXml(temporaryLayers.second.Cast<GameObjectData>().ToArray(), @"data\maps\lvl1\second.xml");
               
            }
            if (ifSaved) ifSaved = save.IsLive((float)args.Time);


            // Обновлять HealthPanel для проверки корректности ее работы
            UpdateHealth();

        }


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
            foreach (var interfaceObj in interfaceObjects) if (interfaceObj != null) interfaceObj.Render();
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

        protected override void OnUnload()
        {
            base.OnUnload();
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }
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