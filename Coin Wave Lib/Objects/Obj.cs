using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Coin_Wave_Lib
{
    public abstract class Obj
    {
        public static string Name
        {
            get { return typeof(Obj).Name; }
        }
        public double[] Vertices { get; set; }
        [XmlElement(ElementName = nameof(Rectangle))]
        public Rectangle Rectangle { get; set; } = new Rectangle(new Point(0, 0), 0, 0);
        [XmlElement(ElementName = nameof(Point))]
        public TexturePoint[] TexturePoints { get; set; } = { new TexturePoint(0, 0), new TexturePoint(0, 0), new TexturePoint(0, 0), new TexturePoint(0, 0) };
        protected IGetVertices _getVertices;
        public Obj(Rectangle rectangle, TexturePoint[] texturePoints, IGetVertices getVertices)
        {
            Rectangle = rectangle;
            TexturePoints = texturePoints;
            _getVertices = getVertices;
        }
        public Obj() { }
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
        public Rectangle GetRectangle()
        {
            return Rectangle;
        }
        public TexturePoint[] GetTexturePoints()
        {
            return TexturePoints;
        }
    }
}
