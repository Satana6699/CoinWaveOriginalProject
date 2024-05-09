using Coin_Wave_Lib.Objects.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coin_Wave_Lib.Programs;

namespace Coin_Wave_Lib.Objects.GameObjects.SolidObjects
{
    public class Coin : GameObject, ICollectable
    {
        public Coin()
        {
            IsSolid = true;
        }

        public Coin(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(rectangleWithTexture, texture, index)
        {
            IsSolid = true;
        }

        public override string Name { get => typeof(Coin).Name; set { } }
    }
}
