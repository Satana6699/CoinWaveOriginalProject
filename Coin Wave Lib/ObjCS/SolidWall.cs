using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.ObjCS
{
    public class SolidWall : GameObject
    {
        public SolidWall(Pnt pnt, double width, double hidth, string path) : base(pnt, width, hidth, path)
        {
        }
    }
}
