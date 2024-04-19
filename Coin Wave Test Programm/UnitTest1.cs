using Coin_Wave_Lib;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using static OpenTK.Graphics.OpenGL.GL;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace Coin_Wave_Test_Programm
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestIndexArray()
        {
            (GameObject[,] first, GameObject[,] second) layers;
            List<GameObject> first = GameObjectsList.CreateListForXml(FileRead.DeserializeObjectsToXml(@"D:\Семестр 4 (полигон)\Курсовая работа\coin wave\Coin Wave (sln)\Coin Wave\bin\Debug\net8.0-windows\data\maps\lvl1\first.xml"));
            List<GameObject> second = GameObjectsList.CreateListForXml(FileRead.DeserializeObjectsToXml(@"D:\Семестр 4 (полигон)\Курсовая работа\coin wave\Coin Wave (sln)\Coin Wave\bin\Debug\net8.0-windows\data\maps\lvl1\second.xml"));
            layers.first = new GameObject[18, 32];
            layers.second = new GameObject[18, 32];


            foreach (GameObject obj in second)
            {
                layers.first[obj.Index.y, obj.Index.x] = obj;
            }

            foreach (GameObject obj in second)
            {
                layers.second[obj.Index.y, obj.Index.x] = obj;
            }

            for (int i = 0; i < layers.first.GetLength(0); i++)
                for (int j = 0;  j < layers.first.GetLength(1); j++)
                {
                    var expectedIndex = layers.first[i,j].Index;
                    (int x, int y) actualIndex = (j, i);

                    Assert.AreEqual(actualIndex, expectedIndex);
                    Assert.AreEqual(layers.first[i, j].Index, layers.second[i, j].Index);
                }

            for (int i = 0; i < layers.second.GetLength(0); i++)
                for (int j = 0;  j < layers.second.GetLength(1); j++)
                {
                    var expectedIndex = layers.second[i,j].Index;
                    (int x, int y) actualIndex = (j, i);

                    Assert.AreEqual(actualIndex, expectedIndex);
                    Assert.AreEqual(layers.second[i, j].Index, layers.first[i, j].Index);
                }
        }
    }
}