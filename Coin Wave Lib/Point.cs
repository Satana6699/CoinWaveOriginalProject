using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class Point
    {
        public double X {  get; set; }
        public double Y {  get; set; }
        public Point()
        {
            
        }
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
        public void NewPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
