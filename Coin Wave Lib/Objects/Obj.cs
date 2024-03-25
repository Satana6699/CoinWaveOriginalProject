using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public abstract class Obj
    {
        public static string Name
        {
            get { return typeof(Obj).Name; }
        }
        public Rectangle Rectangle { get; private set; }
        public TexturePoint[] TexturePoints { get; private set; }
        protected IGetVertices _getVertices;
        public Obj(Rectangle rectangle, TexturePoint[] texturePoints, IGetVertices getVertices)
        {
            Rectangle = rectangle;
            TexturePoints = texturePoints;
            _getVertices = getVertices;
        }
        public void NewPoints(Rectangle rctng)
        {
            Rectangle = rctng;
        }
        public void NewPoints(Point leftTop, double width, double heidth)
        {
            Rectangle = new Rectangle(leftTop, width, heidth);
        }
        public void NewTexturePoints(TexturePoint[] texturePoints)
        {
            TexturePoints = texturePoints;
        }
        public double[] GetVertices()
        {
            return _getVertices.GetVertices(Rectangle.Points, TexturePoints, 5);
        }
        public static double[] GetVertices(Obj[] objects, int offset)
        {
            double[] vertices = new double[offset * objects.Length * 4];
            for (int j = 0; j < objects.Length; j++)
            {
                vertices = vertices.Concat(objects[j].GetVertices()).ToArray();
            }
            return vertices;
        }
        public static double[] GetVertices(List<Obj> objects, int offset)
        {
            return GetVertices(objects.ToArray(), offset);
        }
    }
}
