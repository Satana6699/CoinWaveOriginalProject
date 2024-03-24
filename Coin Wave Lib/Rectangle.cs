using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class Rectangle
    {
        public Point TopLeft { get; private set; }
        public Point TopRight { get; private set; }
        public Point BottomRight { get; private set; }
        public Point BottomLeft { get; private set; }
        public Point[] Points { get; private set; }
        public Rectangle(Point topLeft, Point topRight, Point bottomRight, Point bottomLeft)
        {
            SetPnts(topLeft, topRight, bottomRight, bottomLeft);
        }
        public Rectangle(Point topLeft, double width, double hidth)
        {
            SetPnts(topLeft, width, hidth);
        }
        public void SetPnts(Point topLeft, Point topRight, Point bottomRight, Point bottomLeft)
        {
            Points = new Point[4];
            Points[0] = TopLeft = topLeft;
            Points[1] = TopRight = topRight;
            Points[2] = BottomRight = bottomRight;
            Points[3] = BottomLeft = bottomLeft;
        }

        public void SetPnts(Point topLeft, double width, double hidth)
        {
            Points = new Point[4];

            Points[0] = TopLeft = topLeft;

            Points[1] = TopRight = new Point
                (
                    topLeft.X + width,
                    topLeft.Y
                );

            Points[2] = BottomRight = new Point
                (
                    topLeft.X + width,
                    topLeft.Y - hidth
                );
            Points[3] = BottomLeft = new Point
                (
                    topLeft.X,
                    topLeft.Y - hidth
                );
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
    }
}
