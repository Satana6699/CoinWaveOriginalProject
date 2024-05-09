using Coin_Wave_Lib.Objects.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coin_Wave_Lib.Programs;

namespace Coin_Wave_Lib.Objects.GameObjects.DynamicEntity
{
    public class Stone : DynamicObject
    {
        public Stone(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(rectangleWithTexture, texture, index)
        {
        }
        public Stone()
        {
            
        }
        public override string Name { get => typeof(Stone).Name; set { } }

    }
}
