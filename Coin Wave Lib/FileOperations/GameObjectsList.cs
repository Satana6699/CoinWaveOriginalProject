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
                    GameObjectFactory objectFactoy = ObjectFactory.GetFactoy
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
    }
}
