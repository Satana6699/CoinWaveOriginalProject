using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.ObjCS
{
    public class EmptyElement : GameObject
    {
        public EmptyElement(Rctngl rectangle) : base(rectangle)
        {
        }

        public EmptyElement(int index, Rctngl rectangle) : base(index, rectangle)
        {
        }
    }
}
