using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.Objects.GameObjects.Boneses
{
    public class SpeedUpBonus : Bonus, ICollectable
    {
        public SpeedUpBonus(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(rectangleWithTexture, texture, index)
        {
        }

        public override string Name { get => typeof(SpeedUpBonus).Name; set { } }
    }
}
