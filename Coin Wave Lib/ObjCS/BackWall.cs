using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.ObjCS
{
    public class BackWall : GameObject
    {
        public BackWall(Rctngl rectangle) : base(rectangle)
        {
        }

        public BackWall(int index, Rctngl rectangle) : base(index, rectangle)
        {
        }
    }
}
