using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public abstract class SolidObject : GameObject
    {
        public SolidObject ()
        {
            isSolid = true;
        }
        public SolidObject(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(rectangleWithTexture, texture, index)
        {
            isSolid = true;
        }
    }

}
