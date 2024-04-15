using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class Coin : GameObject, IMembership, IGameMembership
    {
        public Coin()
        {
        }

        public Coin(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(rectangleWithTexture, texture, index)
        {
        }

        public override string Name { get => typeof(Coin).Name; set { } }
    }
}
