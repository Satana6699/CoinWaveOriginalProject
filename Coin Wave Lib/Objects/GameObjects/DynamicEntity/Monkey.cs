using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class Monkey : Monster
    {
        public Monkey(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index, int damage) : base(rectangleWithTexture, texture, index, damage)
        {
            viewDirection = MoveHelper.Down;
        }
        public Monkey()
        {
            
        }
        public override string Name { get => typeof(Monkey).Name; set { } }
    }
}
