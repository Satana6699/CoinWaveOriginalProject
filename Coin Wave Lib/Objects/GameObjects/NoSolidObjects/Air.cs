using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class Air : GameObject
    {
        public Air(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(rectangleWithTexture, texture, index)
        {
        }
        public Air() { }

        public Air(RectangleWithTexture rectangleWithTexture, (int x, int y) index) : base(rectangleWithTexture, index)
        {
        }

        public override string Name { get => typeof(BackWall).Name; set { } }
        public override object Clone()
        {
            return new Air()
            {
                RectangleWithTexture = (RectangleWithTexture)RectangleWithTexture.Clone(),
                Texture = Texture,
                Buffer = new Buffer(GetVertices()),
                Index = Index,
            };
        }

    }
}
