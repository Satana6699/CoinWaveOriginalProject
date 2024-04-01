using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.Objects.InterfaceObjects
{
    public class EmptyElement : InterfaceObject
    {
        public EmptyElement(Rectangle rectangle, TexturePoint[] texturePoints) : base(rectangle, texturePoints)
        {
        }
    }
}
