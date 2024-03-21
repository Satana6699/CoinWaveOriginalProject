using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class Pnt
    {
        public double X {  get; private set; }
        public double Y {  get; private set; }
        public double Z {  get; private set; }
        public double S {  get; private set; } // координата текстуры X
        public double T {  get; private set; } // координата текстуры Y
        public Pnt(double x, double y, double z, double s, double t)
        {
            X = x; Y = y; Z = z;
            S = s; T = t;
        }
        public void NewCoords(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void NewTexturCoords(double s, double t)
        {
            S = s;
            T = t;
        }
    }
}
