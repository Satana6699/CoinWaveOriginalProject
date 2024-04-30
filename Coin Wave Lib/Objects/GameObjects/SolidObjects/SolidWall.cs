using Coin_Wave_Lib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class SolidWall : SolidObject
    {

        public SolidWall()
        {
        }

        public SolidWall(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(rectangleWithTexture, texture, index)
        {
        }

        public override string Name { get => typeof(SolidWall).Name; set { } }
    }
}
