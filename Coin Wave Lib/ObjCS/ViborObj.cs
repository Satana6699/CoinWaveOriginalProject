using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.ObjCS
{
    public class ViborObj : GameObject
    {
        public ViborObj(Rctngl rectangle) : base(rectangle)
        {
        }

        public ViborObj(int index, Rctngl rectangle) : base(index, rectangle)
        {
        }
    }
}
