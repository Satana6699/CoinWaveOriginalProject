using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Coin_Wave_Lib.Objects.Factories;
using Coin_Wave_Lib.Objects.Factories.ConcreteFactory;

namespace Coin_Wave_Lib
{
    public class GameObjectsList
    {
        public static List<GameObject> CreateListForXml(GameObjectData[] gameObjectDatas, Texture textureMap)
        {
            List<GameObject> gameObjects = new List<GameObject>(0);

            foreach (var gameObjectData in gameObjectDatas) 
            {
                if (gameObjectData != null)
                {
                    GameObjectFactory objectFactoy = GetFactoy
                        (
                            gameObjectData.Name,
                            new RectangleWithTexture
                                (
                                    gameObjectData.RectangleWithTexture.Rectangle,
                                    gameObjectData.RectangleWithTexture.TexturePoints
                                ),
                            textureMap,
                            gameObjectData.Index
                        );

                    GameObject gameObject = objectFactoy.GetGameObject();
                    gameObjects.Add( gameObject );
                }
            }

            return gameObjects;
        }

        private static GameObjectFactory GetFactoy(string n, RectangleWithTexture rwc, Texture t, (int x, int y) i) =>
            n switch
            {
                "Player" => new PlayerFactory(n, rwc, t, i),
                "ExitDoor" => new ExitDoorFactory(n, rwc, t, i),
                "StartDoor" => new StartDoorFactory(n, rwc, t, i),
                "SolidWall" => new SolidWallFactory(n, rwc, t, i),

                "BackWall" => new BackWallFactory(n, rwc, t, i),
                "Coin" => new CoinFactory(n, rwc, t, i),
                "Chest" => new ChestFactory(n, rwc, t, i),
                "Stone" => new StoneFactory(n, rwc, t, i),
                "Air" => new AirFactory(n, rwc, t, i),
                _ => new AirFactory(n, rwc, t, i),
            };

        public static List<GameObject> CreateListForXml(GameObjectData[] gameObjectDatas)
        {
            List<GameObject> gameObjects = new List<GameObject>(0);

            foreach (var gameObjectData in gameObjectDatas)
            {
                if (gameObjectData != null)
                {
                    GameObjectFactory objectFactoy = GetFactoy
                        (
                            gameObjectData.Name,
                            new RectangleWithTexture
                                (
                                    gameObjectData.RectangleWithTexture.Rectangle,
                                    gameObjectData.RectangleWithTexture.TexturePoints
                                ),
                            gameObjectData.Index
                        );

                    GameObject gameObject = objectFactoy.GetGameObjectNoTexture();
                    gameObjects.Add(gameObject);
                }
            }

            return gameObjects;
        }

        private static GameObjectFactory GetFactoy(string n, RectangleWithTexture rwc, (int x, int y) i) =>
            n switch
            {
                "Player" => new PlayerFactory(n, rwc, i),
                "ExitDoor" => new ExitDoorFactory(n, rwc, i),
                "StartDoor" => new StartDoorFactory(n, rwc, i),
                "SolidWall" => new SolidWallFactory(n, rwc, i),

                "BackWall" => new BackWallFactory(n, rwc, i),
                "Coin" => new CoinFactory(n, rwc, i),
                "Chest" => new ChestFactory(n, rwc, i),
                "Stone" => new StoneFactory(n, rwc, i),
                "Air" => new AirFactory(n, rwc, i),
                _ => new AirFactory(n, rwc, i),
            };
    }
}
