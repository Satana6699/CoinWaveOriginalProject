using Coin_Wave_Lib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class ExitDoor : SolidObject, IMembership, IGameMembership
    {
        public ExitDoor()
        {
        }

        public ExitDoor(RectangleWithTexture rectangleWithTexture, (int x, int y) index) : base(rectangleWithTexture, index)
        {
        }

        public ExitDoor(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(rectangleWithTexture, texture, index)
        {
        }

        public override string Name { get => typeof(ExitDoor).Name; set { } }
        public override object Clone()
        {
            return new ExitDoor()
            {
                RectangleWithTexture = (RectangleWithTexture)RectangleWithTexture.Clone(),
                Texture = Texture,
                Buffer = new Buffer(GetVertices()),
                Index = (Index.x, Index.y),
            };
        }
    }
}
