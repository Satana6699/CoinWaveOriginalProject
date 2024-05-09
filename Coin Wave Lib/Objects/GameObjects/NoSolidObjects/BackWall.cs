using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coin_Wave_Lib.Programs;
using Coin_Wave_Lib.Objects.GameObjects;

namespace Coin_Wave_Lib.Objects.GameObjects.NoSolidObjects
{
    public class BackWall : GameObject
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
