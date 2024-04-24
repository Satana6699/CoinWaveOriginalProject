using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class BackWall : GameObject, IObjectCore, IGameCore
    {
        public BackWall()
        {
        }

        public BackWall(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(rectangleWithTexture, texture, index)
        {
        }

        public override string Name { get => typeof(BackWall).Name; set { } }
    }
}
