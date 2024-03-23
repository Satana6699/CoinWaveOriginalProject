using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public abstract class GameObject
    {
        public int Index { get; set; }
        public string path;
        public Rctngl Rectangle {  get; set; }
        public GameObject(Rctngl rectangle)
        {
            Rectangle = rectangle;
            this.path = path;
        }
        public GameObject(int index, Rctngl rectangle)
        {
            Index = index;
            Rectangle = rectangle;
            this.path = path;
        }
        public void NewPoints(Rctngl rctng)
        {
            Rectangle = rctng;
        }
        public void NewPoints(Pnt leftTop, double width, double heidth)
        {
            Rectangle = new Rctngl(leftTop, width, heidth);
        }
        public double[] GetVertices()
        {
            return Rectangle.GetVertieces();
        }

        public static double[] GetVertices(GameObject[] gameObjects)
        {
            int offset = 5;
            double[] vertices = new double[gameObjects.Length * 4 * offset];
            for (int i = 0; i < gameObjects.Length; i++)
            {
                for (int j = 0; j < gameObjects[i].Rectangle.GetVertieces().Length; j++)
                {
                    vertices[i * 4 * offset + j] = gameObjects[i].Rectangle.GetVertieces()[j];
                }
            }
            return vertices;
        }
        public static double[] GetVertices(List<GameObject> gameObjects)
        {
            return GetVertices(gameObjects.ToArray());
        }
    }
}
