using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.Objects.GameObjects.Player
{
    public class Player : GameObject
    {
        public Player(Rectangle rectangle, TexturePoint[] texturePoints, int index) : base(rectangle, texturePoints, index)
        {
        }
    }
}
