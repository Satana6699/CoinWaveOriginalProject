using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public interface IMovement
    {
        enum MovementHelp
        {
            Up,
            Down,
            Right,
            Left,
        }
        void Move(MovementHelp muvementHelp);
    }
}
