using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class Dragon : Monster
    {
        public Dragon()
        {
            
        }
        public Dragon(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index, int damage) : base(rectangleWithTexture, texture, index, damage)
        {
            viewDirection = MoveHelper.Right;
        }

        public override string Name { get => typeof(Dragon).Name; set { } }

    }
}
