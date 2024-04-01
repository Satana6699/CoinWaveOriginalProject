using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public abstract class InterfaceObject : Obj
    {
        protected InterfaceObject()
        {
        }

        protected InterfaceObject(RectangleWithTexture rectangleWithTexture, Texture texture) : base(rectangleWithTexture, texture)
        {
        }
    }
}
