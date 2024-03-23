using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.ObjCS
{
    public class Coin : GameObject
    {
        public Coin(Rctngl rectangle) : base(rectangle)
        {
        }

        public Coin(int index, Rctngl rectangle) : base(index, rectangle)
        {
        }
    }
}
