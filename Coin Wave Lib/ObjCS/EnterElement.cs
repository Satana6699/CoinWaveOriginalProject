using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.ObjCS
{
    public class EnterElement : GameObject
    {
        public EnterElement(Rctngl rectangle) : base(rectangle)
        {
        }

        public EnterElement(int index, Rctngl rectangle) : base(index, rectangle)
        {
        }
    }
}
