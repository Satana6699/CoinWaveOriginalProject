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

        public SolidWall(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(rectangleWithTexture, texture, index)
        {
        }
    }
}
