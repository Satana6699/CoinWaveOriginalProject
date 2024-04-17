using Coin_Wave_Lib.Objects.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public abstract class Monster : DynamicObject, IMembership, IGameMembership, IDynamic
    {
        public int Damage { get; set; }
        protected Monster(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index, int damage) : base(rectangleWithTexture, texture, index)
        {
            Damage = damage;
        }

    }
}
