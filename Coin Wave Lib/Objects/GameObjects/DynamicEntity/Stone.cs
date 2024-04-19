using Coin_Wave_Lib.Objects.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class Stone : DynamicObject, IDynamic
    {
        public Stone(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(rectangleWithTexture, texture, index)
        {
        }
        public Stone() { }

        public Stone(RectangleWithTexture rectangleWithTexture, (int x, int y) index) : base(rectangleWithTexture, index)
        {
        }

        public override string Name { get => typeof(Stone).Name; set { } }
        public override object Clone()
        {
            return new Stone()
            {
                RectangleWithTexture = (RectangleWithTexture)RectangleWithTexture.Clone(),
                Texture = Texture,
                Buffer = new Buffer(GetVertices()),
                Index = Index,
            };
        }

    }
}
