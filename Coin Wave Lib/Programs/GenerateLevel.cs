using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.Programs
{
    public class GenerateLevel
    {
        public Texture textureForMap;
        public string filePathFirstLayer;
        public string filePathSecondLayer;
        public (GameObject[,] first, GameObject[,] second) layers;
        public (int widht, int hidth) sides;
        public TextureMap textureMap;
        public Player player;
        public string pathTexture;
        public List<DynamicObject> dynamicObjects = new List<DynamicObject>();
        public int speedObj;
        public GenerateLevel(string filePathFirstLayer, string filePathSecondLayer, string pathTexture, int speedObj, (int widht, int hidth) sides)
        {
            this.filePathFirstLayer = filePathFirstLayer;
            this.filePathSecondLayer = filePathSecondLayer;
            this.pathTexture = pathTexture;
            this.speedObj = speedObj;
            this.sides = sides;
            GenerateWithTexture();
        }

        public GenerateLevel(string filePathFirstLayer, string filePathSecondLayer, int speedObj, (int widht, int hidth) sides)
        {
            this.filePathFirstLayer = filePathFirstLayer;
            this.filePathSecondLayer = filePathSecondLayer;
            this.speedObj = speedObj;
            this.sides = sides;
            Generate();
        }
        private void Generate()
        {
            List<GameObject> first = GameObjectsList.CreateListForXml(FileRead.DeserializeObjectsToXml(filePathFirstLayer));
            List<GameObject> second = GameObjectsList.CreateListForXml(FileRead.DeserializeObjectsToXml(filePathSecondLayer));

            layers.first = new GameObject[sides.hidth, sides.widht];
            layers.second = new GameObject[sides.hidth, sides.widht];

            foreach (GameObject obj in first)
            {
                layers.first[obj.Index.y, obj.Index.x] = obj;
            }

            player = CreateSecondLayer(second);
            dynamicObjects = DynamicObject.SearchDynamicObjects(layers.second, textureMap, speedObj);
        }

        private void GenerateWithTexture()
        {
            textureForMap = Texture.LoadFromFile(pathTexture);

            List<GameObject> first = GameObjectsList.CreateListForXml(FileRead.DeserializeObjectsToXml(filePathFirstLayer), textureForMap);
            List<GameObject> second = GameObjectsList.CreateListForXml(FileRead.DeserializeObjectsToXml(filePathSecondLayer), textureForMap);

            layers.first = new GameObject[sides.hidth, sides.widht];
            layers.second = new GameObject[sides.hidth, sides.widht];

            textureMap = new TextureMap(Resources.textureMap.Width, Resources.textureMap.Height, 4, textureForMap);

            foreach (GameObject obj in first)
            {
                layers.first[obj.Index.y, obj.Index.x] = obj;
            }

            player = CreateSecondLayerForTexture(second);
            dynamicObjects = DynamicObject.SearchDynamicObjects(layers.second, textureMap, speedObj);
        }

        private Player CreateSecondLayerForTexture(List<GameObject> second)
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
        private Player CreateSecondLayer(List<GameObject> second)
        {
            Player player = null;
            foreach (GameObject obj in second)
            {

                if (obj.Name == typeof(Player).Name)
                {
                    player = new Player()
                    {
                        RectangleWithTexture = obj.RectangleWithTexture,
                        Index = obj.Index
                    };


                    layers.second[obj.Index.y, obj.Index.x] = new Air
                    {
                        RectangleWithTexture = new RectangleWithTexture
                        (
                            obj.RectangleWithTexture.Rectangle,
                            TexturePoint.Default()
                        ),
                        Index = obj.Index
                    };
                }
                else layers.second[obj.Index.y, obj.Index.x] = obj;
            }

            return player;
        }
    }
}
