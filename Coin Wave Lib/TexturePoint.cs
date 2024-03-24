using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class TexturePoint
    {
        public double S { get; private set; } // координата текстуры X
        public double T { get; private set; } // координата текстуры Y
        public TexturePoint(double s, double t)
        {
            S = s;
            T = t;
        }

        public void NewPoint(double s, double t)
        {
            S = s;
            T = t;
        }
    }
}
