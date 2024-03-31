﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Coin_Wave_Lib
{
    public abstract class Obj
    {
        public string Name{ get; set; } = typeof(Obj).Name;
        public double[] Vertices { get; set; }
        [XmlElement(ElementName = nameof(Rectangle))]
        public Rectangle Rectangle { get; set; } = new Rectangle(new Point(0, 0), 0, 0);
        [XmlElement(ElementName = nameof(Point))]
        public TexturePoint[] TexturePoints { get; set; } = { new TexturePoint(0, 0), new TexturePoint(0, 0), new TexturePoint(0, 0), new TexturePoint(0, 0) };
        public Obj(Rectangle rectangle, TexturePoint[] texturePoints)
        {
            Rectangle = rectangle;
            TexturePoints = texturePoints;
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
            int offset = 5;
            double[] vertices = new double[offset * Rectangle.Points.Length];
            for (int i = 0, j = 0; i < vertices.Length || j < this.Rectangle.Points.Length || j < TexturePoints.Length; i += offset, j++)
            {
                vertices[i] = Rectangle.Points[j].X;
                vertices[i + 1] = Rectangle.Points[j].Y;
                vertices[i + 2] = 0.0;
                vertices[i + 3] = TexturePoints[j].S;
                vertices[i + 4] = TexturePoints[j].T;
            }
            Vertices = vertices;
            return vertices;
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
