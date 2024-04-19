using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class Monkey : Monster, IDynamic
    {
        public Monkey(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(rectangleWithTexture, texture, index)
        {
        }
        public Monkey() : base() { }
        public override string Name { get => typeof(Monkey).Name; set { } }
        public override object Clone()
        {
            return new Monkey()
            {
                RectangleWithTexture = (RectangleWithTexture)RectangleWithTexture.Clone(),
                Texture = Texture,
                Buffer = new Buffer(GetVertices()),
                Index = (Index.x, Index.y),
            };
        }
    }
}
