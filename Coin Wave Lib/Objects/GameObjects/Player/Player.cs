using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class Player : DynamicObject
    {
        public int CountCoins { get; private set; } = 0;
        public override string Name { get => typeof(Player).Name; set { } }
        public int HealthPoint { get; private set; }
        public int MaxHealthPoint { get; private set; } = 200;
        public Player()
        {
            HealthPoint = MaxHealthPoint;
        }
        public Player(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(rectangleWithTexture, texture, index)
        {
            HealthPoint = MaxHealthPoint;
        }

        public void ColletCoins(int coins)
        {
            CountCoins += coins;
        }

        public void Damage(int damage)
        {
            HealthPoint -= damage;
        }

        public void Kill()
        {
            HealthPoint -= MaxHealthPoint + 1;
        }
        public void Heal(int heal)
        {
            HealthPoint += heal;
            if (HealthPoint >= MaxHealthPoint) HealthPoint = MaxHealthPoint;
        }

    }
}
