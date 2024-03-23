using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.ObjCS
{
    public class Chest : GameObject
    {
        public Chest(Rctngl rectangle) : base(rectangle)
        {
        }

        public Chest(int index, Rctngl rectangle) : base(index, rectangle)
        {
        }
    }
}
