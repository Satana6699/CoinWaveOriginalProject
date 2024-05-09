using Coin_Wave_Lib;
using Coin_Wave_Lib.Objects;
using Coin_Wave_Lib.Objects.GameObjects.DynamicEntity;
using Coin_Wave_Lib.Programs;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Reflection.Emit;
using Coin_Wave_Lib.Objects.GameObjects;
using Coin_Wave_Lib.Objects.GameObjects.Player;


[TestClass]
public class MonsterTests
{
    (int widht, int hidth) sides = (32, 18);
    string _filePathForLevels = @"..\..\..\..\Coin Wave Lib\data\maps\lvl";

    private (GameObject[,] first, GameObject[,] second) layers;
    Player player;
    List<DynamicObject> dynamicObjects = new List<DynamicObject>();

    [TestMethod]
    public void TestMonkeyDamage()
    {
        Monster monkey = new Monkey()
        {
            RectangleWithTexture = new RectangleWithTexture()
            {
                Rectangle = new Rectangle(),
                TexturePoints = TexturePoint.Default()
            }
        };
        string fileFirst = _filePathForLevels + 3 + @"\first.xml";
        string fileSecond = _filePathForLevels + 3 + @"\second.xml";
        int speedObj = 3;
        GenerateLevel generateLevel = new GenerateLevel(fileFirst, fileSecond, speedObj, sides);
        layers.first = generateLevel.layers.first;
        layers.second = generateLevel.layers.second;
        player = generateLevel.player;
        dynamicObjects = generateLevel.dynamicObjects;

        int damage = 0;
        foreach (var obj in dynamicObjects)
        {
            if (obj is Monkey monster)
            {
                player.Damage(monster.Damage);
                damage = monster.Damage;
                break;
            }
        }

        // Утверждение для проверки урона, нанесенного игроком монстру
        Assert.AreEqual(player.HealthPoint, player.MaxHealthPoint - damage);
    }


    [TestMethod]
    public void TestFireWheelDamage()
    {
        Monster monkey = new Monkey()
        {
            RectangleWithTexture = new RectangleWithTexture()
            {
                Rectangle = new Rectangle(),
                TexturePoints = TexturePoint.Default()
            }
        };
        string fileFirst = _filePathForLevels + 3 + @"\first.xml";
        string fileSecond = _filePathForLevels + 3 + @"\second.xml";
        int speedObj = 3;
        GenerateLevel generateLevel = new GenerateLevel(fileFirst, fileSecond, speedObj, sides);
        layers.first = generateLevel.layers.first;
        layers.second = generateLevel.layers.second;
        player = generateLevel.player;
        dynamicObjects = generateLevel.dynamicObjects;

        int damage = 0;
        foreach (var obj in dynamicObjects)
        {
            if (obj is FireWheel monster)
            {
                player.Damage(monster.Damage);
                damage = monster.Damage;
                break;
            }
        }

        // Утверждение для проверки урона, нанесенного игроком монстру
        Assert.AreEqual(player.HealthPoint, player.MaxHealthPoint - damage);
    }


    [TestMethod]
    public void TestDragonDamage()
    {
        Monster monkey = new Monkey()
        {
            RectangleWithTexture = new RectangleWithTexture()
            {
                Rectangle = new Rectangle(),
                TexturePoints = TexturePoint.Default()
            }
        };
        string fileFirst = _filePathForLevels + 3 + @"\first.xml";
        string fileSecond = _filePathForLevels + 3 + @"\second.xml";
        int speedObj = 3;
        GenerateLevel generateLevel = new GenerateLevel(fileFirst, fileSecond, speedObj, sides);
        layers.first = generateLevel.layers.first;
        layers.second = generateLevel.layers.second;
        player = generateLevel.player;
        dynamicObjects = generateLevel.dynamicObjects;

        int damage = 0;
        foreach (var obj in dynamicObjects)
        {
            if (obj is Dragon monster)
            {
                player.Damage(monster.Damage);
                damage = monster.Damage;
                break;
            }
        }

        // Утверждение для проверки урона, нанесенного игроком монстру
        Assert.AreEqual(player.HealthPoint, player.MaxHealthPoint - damage);
    }
}