using Coin_Wave_Lib.Objects.Factories.ConcreteFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public abstract class ObjectFactory
    {
        public abstract Obj GetObject();
        public static GameObjectFactory GetFactory(string n, RectangleWithTexture rwc, Texture t, (int x, int y) i) =>
            n switch
            {
                "Player" => new PlayerFactory(n, rwc, t, i),
                "ExitDoor" => new ExitDoorFactory(n, rwc, t, i),
                "StartDoor" => new StartDoorFactory(n, rwc, t, i),
                "SolidWall" => new SolidWallFactory(n, rwc, t, i),

                "BackWall" => new BackWallFactory(n, rwc, t, i),
                "Coin" => new CoinFactory(n, rwc, t, i),
                "Chest" => new ChestFactory(n, rwc, t, i),
                "Stone" => new StoneFactory(n, rwc, t, i),
                "Dragon" => new DragonFactory(n, rwc, t, i),
                "Monkey" => new MonkeyFactory(n, rwc, t, i),
                "FireWheel" => new FireWheelFactory(n, rwc, t, i),
                "Thorn" => new ThornFactory(n, rwc, t, i),
                "Air" => new AirFactory(n, rwc, t, i),
                _ => new AirFactory(n, rwc, t, i),
            };
    }
}
