using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class Player : DynamicObject, IMembership, IGameMembership
    {
        public int CountCoins { get; private set; } = 0;
        public override string Name { get => typeof(Player).Name; set { } }

        public Player(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(rectangleWithTexture, texture, index)
        {
        }
        
        public void ColletCoins(int coins)
        {
            CountCoins += coins;
        }
    }
}
