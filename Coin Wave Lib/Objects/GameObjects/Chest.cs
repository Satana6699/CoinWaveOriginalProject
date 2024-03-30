using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class Chest : GameObject
    {
        public Chest()
        {
        }

        public Chest(Rectangle rectangle, TexturePoint[] texturePoints, IGetVertices getVertices, int index) : base(rectangle, texturePoints, getVertices, index)
        {
        }
    }
}
