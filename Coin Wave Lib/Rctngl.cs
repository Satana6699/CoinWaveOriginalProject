using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class Rctngl
    {
        public Pnt TopLeft { get; private set; }
        public Pnt TopRight { get; private set; }
        public Pnt BottomRight { get; private set; }
        public Pnt BottomLeft { get; private set; }
        private Pnt[] Pnts { get; set; }
        private double[] _vertices = new double[0];
        public Rctngl(Pnt topLeft, Pnt topRight, Pnt bottomRight, Pnt bottomLeft)
        {
            SetPnts(topLeft, topRight, bottomRight, bottomLeft);

            DefaultTextureCootds(); //ВРЕМЕННО, ПОТОМ НЕОБХОДИМО УБРАТЬ
        }
        public Rctngl(Pnt topLeft, double width, double hidth)
        {
            SetPnts(topLeft, width, hidth);

            DefaultTextureCootds(); //ВРЕМЕННО, ПОТОМ НЕОБХОДИМО УБРАТЬ
        }
        public void SetPnts(Pnt topLeft, Pnt topRight, Pnt bottomRight, Pnt bottomLeft)
        {
            Pnts = new Pnt[4];
            Pnts[0] = TopLeft = topLeft;
            Pnts[1] = TopRight = topRight;
            Pnts[2] = BottomRight = bottomRight;
            Pnts[3] = BottomLeft = bottomLeft;
        }

        public void SetPnts(Pnt topLeft, double width, double hidth)
        {
            Pnts = new Pnt[4];
            Pnts[0] = TopLeft = topLeft;
            Pnts[1] = TopRight = new Pnt(topLeft.X + width,
                             topLeft.Y,
                             topLeft.Z,
                             0, 0);
            Pnts[2] = BottomRight = new Pnt(topLeft.X + width,
                             topLeft.Y - hidth,
                             topLeft.Z,
                             0, 0);
            Pnts[3] = BottomLeft = new Pnt(topLeft.X,
                             topLeft.Y - hidth,
                             topLeft.Z,
                             0, 0);
            DefaultTextureCootds();
        }
        /// <summary>
        /// Получить ширину объекта
        /// </summary>
        /// <returns></returns>
        public double GetWidth()
        {
            return Math.Abs(TopRight.X - TopLeft.X);
        }
        /// <summary>
        /// Получить высоту объекта
        /// </summary>
        /// <returns></returns>
        public double GetHeight()
        {
            return Math.Abs(TopLeft.Y - BottomLeft.Y);
        }

        public void DefaultTextureCootds()
        {
            TopLeft.NewTexturCoords(0, 1);
            TopRight.NewTexturCoords(1, 1);
            BottomRight.NewTexturCoords(1, 0);
            BottomLeft.NewTexturCoords(0, 0);
            _vertices = new double[0];
        }
        public double[] GetVertieces()
        {
            if (_vertices.Length == 0)
            {
                _vertices = new double[20];
                int offset = 5; // каждые 5 позиций координата повторяется
                for (int i = 0, j = 0; j < 4 && i < _vertices.Length; i+=offset, j++)
                {
                    _vertices[i] = Pnts[j].X;
                    _vertices[i + 1] = Pnts[j].Y;
                    _vertices[i + 2] = Pnts[j].Z;
                    _vertices[i + 3] = Pnts[j].S;
                    _vertices[i + 4] = Pnts[j].T;
                }
            }
            return _vertices.ToArray();
        }

    }
}
