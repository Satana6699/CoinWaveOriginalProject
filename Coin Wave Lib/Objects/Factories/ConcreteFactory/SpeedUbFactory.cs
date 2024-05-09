using Coin_Wave_Lib.Objects.GameObjects.Boneses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coin_Wave_Lib.Programs;
using Coin_Wave_Lib.Objects.GameObjects;

namespace Coin_Wave_Lib.Objects.Factories.ConcreteFactory
{
    public class SpeedUpFactory : GameObjectFactory
    {
        public SpeedUpFactory(string name, RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(name, rectangleWithTexture, texture, index)
        {
        }

        public SpeedUpFactory(string name, RectangleWithTexture rectangleWithTexture, (int x, int y) index) : base(name, rectangleWithTexture, index)
        {
        }

        public override GameObject GetGameObjectWithTexture() => new SpeedUpBonus(_rectangleWithTexture, _texture, _index)
        {
            Name = _name
        };
        public override GameObject GetGameObject() => new SpeedUpBonus()
        {
            RectangleWithTexture = _rectangleWithTexture,
            Index = _index,
            Name = _name,
        };
    }
}
