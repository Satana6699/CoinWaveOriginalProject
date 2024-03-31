using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class SolidWall : GameObject
    {
        static SolidWall()
        {
            isSolid = true;
        }
        public SolidWall()
        {
        }

        public SolidWall(Rectangle rectangle, TexturePoint[] texturePoints, int index) : base(rectangle, texturePoints, index)
        {
        }
    }
}
