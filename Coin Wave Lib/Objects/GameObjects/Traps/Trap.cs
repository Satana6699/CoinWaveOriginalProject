using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coin_Wave_Lib.Programs;
using Coin_Wave_Lib.Objects.GameObjects;

namespace Coin_Wave_Lib.Objects.GameObjects.Traps
{
    public abstract class Trap : GameObject
    {
        public int Damage { get; set; }
        protected Trap(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index, int damage) : base(rectangleWithTexture, texture, index)
        {
            Damage = damage;
        }
        protected Trap()
        {
            
        }
    }
}
