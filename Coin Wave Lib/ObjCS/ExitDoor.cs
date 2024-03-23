using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.ObjCS
{
    public class ExitDoor : GameObject
    {
        public ExitDoor(Rctngl rectangle) : base(rectangle)
        {
        }

        public ExitDoor(int index, Rctngl rectangle) : base(index, rectangle)
        {
        }
    }
}
