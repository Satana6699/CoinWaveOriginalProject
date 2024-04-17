using Coin_Wave_Lib.Objects.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class Stone : DynamicObject
    {
        public Stone(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(rectangleWithTexture, texture, index)
        {
        }
        public override string Name { get => typeof(ExitDoor).Name; set { } }

    }
}
