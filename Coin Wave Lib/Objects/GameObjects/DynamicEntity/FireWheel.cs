using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coin_Wave_Lib.Programs;

namespace Coin_Wave_Lib.Objects.GameObjects.DynamicEntity
{
    public class FireWheel : Monster
    {
        public override string Name { get; set; }
        public FireWheel(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index, int damage) : base(rectangleWithTexture, texture, index, damage)
        {
        }
        public FireWheel()
        {
            
        }
        protected override void ReverseMove()
        {
            Random random = new Random();
            int randomIndex = random.Next(0, 2);
            switch (viewDirection)
            {
                case MoveHelper.Down:

                    if (randomIndex == 0)
                        viewDirection = MoveHelper.Right;
                    else
                        viewDirection = MoveHelper.Left;

                    break;
                case MoveHelper.Up:

                    if (randomIndex == 0)
                        viewDirection = MoveHelper.Right;
                    else
                        viewDirection = MoveHelper.Left;

                    break;
                case MoveHelper.Left:

                    if (randomIndex == 0)
                        viewDirection = MoveHelper.Down;
                    else
                        viewDirection = MoveHelper.Up;

                    break;
                case MoveHelper.Right:

                    if (randomIndex == 0)
                        viewDirection = MoveHelper.Down;
                    else
                        viewDirection = MoveHelper.Up;

                    break;
            }
        }
    }
}
