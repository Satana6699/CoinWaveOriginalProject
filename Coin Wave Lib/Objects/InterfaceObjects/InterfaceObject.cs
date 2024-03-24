using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public abstract class InterfaceObject : Obj
    {
        public InterfaceObject(Rectangle rectangle, TexturePoint[] texturePoints, IGetVertices getVertices) : base(rectangle, texturePoints, getVertices)
        {
        }
    }
}
