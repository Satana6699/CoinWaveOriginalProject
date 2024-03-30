using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.Objects.InterfaceObjects
{
    public class InterfaceConcreteObj : InterfaceObject
    {
        public float frameTimeLive {get; private set;} = 2f;
        public InterfaceConcreteObj(Rectangle rectangle, TexturePoint[] texturePoints, IGetVertices getVertices) : base(rectangle, texturePoints, getVertices)
        {
        }

        public InterfaceConcreteObj() { }

        public bool IsLive(float time)
        {
            frameTimeLive -= time;
            if (frameTimeLive <= 0)
            {
                frameTimeLive = 2f;
                return false;
            }
            else return true;
        }
    }
}
