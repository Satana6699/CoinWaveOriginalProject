using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.ObjCS
{
    public class ViborObj : InterfaceObject
    {
        public ViborObj()
        {
        }

        public ViborObj(Rectangle rectangle, TexturePoint[] texturePoints, IGetVertices getVertices) : base(rectangle, texturePoints, getVertices)
        {
        }
    }
}
