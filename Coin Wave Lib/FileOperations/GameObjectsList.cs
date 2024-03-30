﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Coin_Wave_Lib
{
    public class GameObjectsList
    {
        public static List<GameObject> CreateListForXml(GameObject[] gameObjectDatas)
        {
            /* Если будет внешняя библиотека
             * Assembly externalAssembly = Assembly.LoadFrom("путь_к_вашей_библиотеке.dll");
             * var types = externalAssembly.GetTypes();*/

            var types = Assembly.GetExecutingAssembly().GetTypes();

            // Фильтруем типы, чтобы найти те, которые являются наследниками GameObject
            var gameObjectTypes = types.Where(t => t.IsSubclassOf(typeof(GameObject)));

            types = gameObjectTypes.ToArray();

            List<GameObject> gameObjects = new List<GameObject>();
            for (int i = 0; i < gameObjectDatas.Length; i++)
            {
                for (int j = 0; j < types.Length; j++)
                {
                    if (gameObjectDatas[i].Name == types[j].Name)
                    {
                        gameObjects.Add((GameObject)Activator.CreateInstance(types[j], new object[]
                        {gameObjectDatas[i].Rectangle, gameObjectDatas[i].TexturePoints, gameObjectDatas[i].Index }));
                        break;
                    }
                }
            }
            return gameObjects;
        }
    }
}