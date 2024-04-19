using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.Objects.InterfaceObjects
{
    public class CurrentPositionElement : InterfaceObject
    {
        public CurrentPositionElement(RectangleWithTexture rectangleWithTexture, Texture texture) : base(rectangleWithTexture, texture)
        {
        }
        public CurrentPositionElement()
        {
            
        }
        public override string Name { get => typeof(CurrentPositionElement).Name; set { } }

        public override object Clone()
        {
            return new CurrentPositionElement()
            {
                RectangleWithTexture = (RectangleWithTexture)RectangleWithTexture.Clone(),
                Texture = Texture,
                Buffer = new Buffer(GetVertices())
            };
        }
    }
}
