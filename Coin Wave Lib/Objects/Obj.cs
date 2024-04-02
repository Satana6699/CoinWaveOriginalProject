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
        public abstract string Name{ get; set; }
        public double[] Vertices { get; set; }
        [XmlElement(ElementName = nameof(RectangleWithTexture))]
        public RectangleWithTexture RectangleWithTexture { get; set; }
        [XmlIgnore]
        public Texture Texture {  get; set; }
        private Buffer _buffer;
        
        public Obj(RectangleWithTexture rectangleWithTexture, Texture texture)
        {
            this.RectangleWithTexture = rectangleWithTexture;
            _buffer = new(GetVertices());
            Texture = texture;
            _buffer = new Buffer(GetVertices());
        }
        public Obj() { }
        public void SetPoints(Rectangle rctng)
        {
            RectangleWithTexture.Rectangle = rctng;
        }
        public void SetPoints(Point leftTop, double width, double heidth)
        {
            RectangleWithTexture.Rectangle = new Rectangle(leftTop, width, heidth);
        }
        public void SetTexturePoints(TexturePoint[] texturePoints)
        {
            RectangleWithTexture.TexturePoints = texturePoints;
        }
        public double[] GetVertices()
        {
            int offset = 5;
            double[] vertices = new double[offset * RectangleWithTexture.Rectangle.Points.Length];
            for (
                int i = 0,
                j = 0; i < vertices.Length ||
                j < this.RectangleWithTexture.Rectangle.Points.Length ||
                j < RectangleWithTexture.TexturePoints.Length;
                i += offset, j++)
            {
                vertices[i] = RectangleWithTexture.Rectangle.Points[j].X;
                vertices[i + 1] = RectangleWithTexture.Rectangle.Points[j].Y;
                vertices[i + 2] = RectangleWithTexture.Rectangle.Points[j].Z;
                vertices[i + 3] = RectangleWithTexture.TexturePoints[j].S;
                vertices[i + 4] = RectangleWithTexture.TexturePoints[j].T;
            }
            Vertices = vertices;
            return vertices;
        }
        public static double[] GetVertices(Obj[] objects, int offset)
        {
            // Предварительно вычисляем общее количество вершин
            int totalVertices = objects.Sum(obj => obj.RectangleWithTexture.Rectangle.Points.Length);

            // Создаем один массив для всех вершин
            double[] vertices = new double[totalVertices * offset];

            // Индекс для отслеживания текущей позиции в массиве вершин
            int vertexIndex = 0;

            foreach (var obj in objects)
            {
                int numPoints = obj.RectangleWithTexture.Rectangle.Points.Length;
                for (int i = 0; i < numPoints; i++)
                {
                    vertices[vertexIndex] = obj.RectangleWithTexture.Rectangle.Points[i].X;
                    vertices[vertexIndex + 1] = obj.RectangleWithTexture.Rectangle.Points[i].Y;
                    vertices[vertexIndex + 2] = obj.RectangleWithTexture.Rectangle.Points[i].Z;
                    vertices[vertexIndex + 3] = obj.RectangleWithTexture.TexturePoints[i].S;
                    vertices[vertexIndex + 4] = obj.RectangleWithTexture.TexturePoints[i].T;
                    vertexIndex += offset;
                }
            }

            return vertices;
        }



        public Rectangle GetRectangle()
        {
            return RectangleWithTexture.Rectangle;
        }
        public TexturePoint[] GetTexturePoints()
        {
            return RectangleWithTexture.TexturePoints;
        }
        public void Render()
        {
            _buffer.Render(Texture);
        }
        public void UpdateDate(double[] vertieces)
        {
            _buffer.UpdateDate(vertieces);
        }
        public void SetBuffer(Buffer buffer)
        {
            _buffer = buffer;
        }
    }
}
