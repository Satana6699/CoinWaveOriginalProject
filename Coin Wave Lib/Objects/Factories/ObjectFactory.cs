using Coin_Wave_Lib.Objects.Factories.ConcreteFactory;
using Coin_Wave_Lib.Objects.GameObjects.Boneses;
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
                // Игрок
                "Player" => new PlayerFactory(n, rwc, t, i),

                // Двери
                "ExitDoor" => new ExitDoorFactory(n, rwc, t, i),
                "StartDoor" => new StartDoorFactory(n, rwc, t, i),

                // Стены
                "SolidWall" => new SolidWallFactory(n, rwc, t, i),
                "BackWall" => new BackWallFactory(n, rwc, t, i),
                

                // Монстры
                "Dragon" => new DragonFactory(n, rwc, t, i),
                "Monkey" => new MonkeyFactory(n, rwc, t, i),
                "FireWheel" => new FireWheelFactory(n, rwc, t, i),

                // Ловушки
                "Thorn" => new ThornFactory(n, rwc, t, i),
                "TrapFire" => new TrapFireFactory(n, rwc, t, i),

                // Бонусы
                "SpeedUp" => new SpeedUpFactory(n, rwc, t, i),
                "HealthUp" => new HealthUpFactory(n, rwc, t, i),

                // Воздух
                "Air" => new AirFactory(n, rwc, t, i),

                // Остальное
                "Coin" => new CoinFactory(n, rwc, t, i),
                "Chest" => new ChestFactory(n, rwc, t, i),
                "Stone" => new StoneFactory(n, rwc, t, i),

                // Если объект не определён, тогда создать воздух
                _ => new AirFactory(n, rwc, t, i),
            };
    }
}
