using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class TrapFire : Trap
    {
        public override string Name { get; set; }
        public TrapFire(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index, int damage) : base(rectangleWithTexture, texture, index, damage)
        {
        }
    }
}
