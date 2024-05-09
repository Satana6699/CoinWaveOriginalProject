using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coin_Wave_Lib.Programs;

namespace Coin_Wave_Lib.Objects.GameObjects.Boneses
{
    public abstract class Bonus : GameObject
    {
        protected Bonus()
        {
            
        }
        protected Bonus(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(rectangleWithTexture, texture, index)
        {
        }
    }
}
