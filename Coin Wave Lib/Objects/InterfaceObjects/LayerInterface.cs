using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.Objects.InterfaceObjects
{
    public class LayerInterface : InterfaceObject
    {
        public LayerInterface(RectangleWithTexture rectangleWithTexture, Texture texture) : base(rectangleWithTexture, texture)
        {
        }
        public LayerInterface()
        {
            
        }
        public override string Name { get => typeof(LayerInterface).Name; set { } }
        public override object Clone()
        {
            return new LayerInterface()
            {
                RectangleWithTexture = (RectangleWithTexture)RectangleWithTexture.Clone(),
                Texture = Texture,
                Buffer = new Buffer(GetVertices())
            };
        }
    }
}
