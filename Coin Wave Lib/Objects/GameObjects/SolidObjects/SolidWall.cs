using Coin_Wave_Lib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class SolidWall : GameObject
    {

        public SolidWall()
        {
            IsSolid = true;
        }

        public SolidWall(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(rectangleWithTexture, texture, index)
        {
            IsSolid = true;
        }

        public override string Name { get => typeof(SolidWall).Name; set { } }
    }
}
