using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.Objects.InterfaceObjects
{
    public class EmptyElement : InterfaceObject
    {
        public EmptyElement(RectangleWithTexture rectangleWithTexture, Texture texture) : base(rectangleWithTexture, texture)
        {
        }
        public EmptyElement()
        {
            
        }
        public override string Name { get => typeof(EmptyElement).Name; set { } }
        public override object Clone()
        {
            return new EmptyElement()
            {
                RectangleWithTexture = (RectangleWithTexture)RectangleWithTexture.Clone(),
                Texture = Texture,
                Buffer = new Buffer(GetVertices())
            };
        }
    }
}
