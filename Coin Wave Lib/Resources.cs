using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public static class Resources
    {
        public static readonly (int Width, int Height) textureMap = (8, 8);
        public const int SolidWall = 0;
        public const int BackWall = 1;
        public const int Stone = 2;
        public const int PlayerDefault = 3;
        public const int PlayerAndStone = 4;
        public const int DragonRight = 5;
        public const int DragonLeft = 6;
        public const int FireWhell = 7;
        public const int MonkeyDown = 8;
        public const int MonkeyUp = 9;
        public const int TrapFire = 10;
        public const int Fire = 11;
        public const int Coin = 12;
        public const int StartDoor = 13;
        public const int ExitDoor = 14;
        public const int HealthPanel = 15;
        public const int ActiveThorn = 16;
        public const int DeActiveThorn = 17;
        public const int Air = 60;
    }
}
