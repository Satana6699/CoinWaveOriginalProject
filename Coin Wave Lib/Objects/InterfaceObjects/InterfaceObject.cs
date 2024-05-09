using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coin_Wave_Lib.Programs;

namespace Coin_Wave_Lib.Objects.InterfaceObjects
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
