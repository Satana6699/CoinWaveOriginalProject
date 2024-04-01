using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.Objects.GameObjects.Player
{
    public class Player : GameObject, IMovement
    {
        
        public Player(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(rectangleWithTexture, texture, index)
        {
        }

        public void Move(IMovement.MovementHelp movementHelp)
        {
            switch (movementHelp)
            {
                case IMovement.MovementHelp.Up:
                    break;
                case IMovement.MovementHelp.Down:
                    break;
                case IMovement.MovementHelp.Right:
                    break;
                case IMovement.MovementHelp.Left:
                    break;
                default:
                    break;
            }
        }
    }
}
