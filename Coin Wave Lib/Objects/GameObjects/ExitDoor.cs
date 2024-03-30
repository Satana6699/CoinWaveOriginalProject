using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class ExitDoor : GameObject
    {
        public ExitDoor()
        {
        }

        public ExitDoor(Rectangle rectangle, TexturePoint[] texturePoints, IGetVertices getVertices, int index) : base(rectangle, texturePoints, getVertices, index)
        {
        }
    }
}
