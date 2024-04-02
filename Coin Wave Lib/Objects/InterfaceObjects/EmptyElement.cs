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

        public override string Name { get => typeof(EmptyElement).Name; set { } }
    }
}
