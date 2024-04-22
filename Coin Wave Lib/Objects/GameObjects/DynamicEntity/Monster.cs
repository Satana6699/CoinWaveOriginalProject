using Coin_Wave_Lib.Objects.GameObjects;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public enum MoveHelper
    {
        Up,
        Down,
        Right,
        Left,
    }
    public abstract class Monster : DynamicObject, IMembership, IGameMembership, IDynamic
    {
        public MoveHelper viewDirection = MoveHelper.Right;
        public int Damage { get; set; }
        protected Monster(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index, int damage) : base(rectangleWithTexture, texture, index)
        {
            Damage = damage;
        }


        public void Move((GameObject[,] first, GameObject[,] second) layers, List<DynamicObject> dynamicObjects)
        {
            (int x, int y) index = (0,0);
            switch (viewDirection)
            {
                case MoveHelper.Down:
                    index = (Index.x, Index.y + 1);
                    NextIndexForMove(layers, dynamicObjects, index);
                    break;
                case MoveHelper.Up:
                    index = (Index.x, Index.y - 1);
                    NextIndexForMove(layers, dynamicObjects, index);
                    break;
                case MoveHelper.Left:
                    index = (Index.x - 1, Index.y);
                    NextIndexForMove(layers, dynamicObjects, index);
                    break;
                case MoveHelper.Right:
                    index = (Index.x + 1, Index.y);
                    NextIndexForMove(layers, dynamicObjects, index);
                    break;
            }
        }

        private void NextIndexForMove((GameObject[,] first, GameObject[,] second) layers, List<DynamicObject> dynamicObjects, (int x, int y) index)
        {
            if (ContinueMove())
                if (!layers.second[index.y, index.x].IsSolid &&
                            !layers.first[index.y, index.x].IsSolid)
                {
                    bool nextOdjSolid = false;
                    foreach (var nextObj in dynamicObjects)
                    {
                        if ((index.x, index.y) == nextObj.Index)
                        {
                            nextOdjSolid = true;
                            ReverseMove();
                            break;
                        }
                    }
                    if (!nextOdjSolid)
                        Index = (index.x, index.y);
                }
                else
                {
                    ReverseMove();
                }
        }

        protected virtual void ReverseMove()
        {
            switch (viewDirection)
            {
                case MoveHelper.Down:
                    viewDirection = MoveHelper.Up;
                    break;
                case MoveHelper.Up:
                    viewDirection = MoveHelper.Down;
                    break;
                case MoveHelper.Left:
                    viewDirection = MoveHelper.Right;
                    break;
                case MoveHelper.Right:
                    viewDirection = MoveHelper.Left;
                    break;
            }
        }
    }
}
