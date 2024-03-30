using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class DoublePoints : IGetVertices
    {
        public double[] GetVertices(Point[] points, TexturePoint[] texturePoints, int offset)
        {
            double[] vertices = new double[offset * points.Length];
            for (int i = 0, j = 0; i < vertices.Length || j < points.Length || j < texturePoints.Length; i += offset, j++)
            {
                vertices[i] = points[j].X;
                vertices[i + 1] = points[j].Y;
                vertices[i + 2] = 0.0;
                vertices[i + 3] = texturePoints[j].S;
                vertices[i + 4] = texturePoints[j].T;
            }
            return vertices;
        }

        public double[] GetVertices(Obj[] objects, int offset)
        {
            double[] vertices = new double[offset * (objects.Length)];
            for (int j = 0; j < objects.Length; j++)
            {
                vertices = vertices.Concat(GetVertices(objects[j].GetRectangle().Points, objects[j].GetTexturePoints(), offset)).ToArray();
            }
            return vertices;
        }
    }
}
