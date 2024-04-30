using Coin_Wave_Lib.Objects.GameObjects.Boneses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.Objects.Factories.ConcreteFactory
{
    public class SpeedDownFactory : GameObjectFactory
    {
        public SpeedDownFactory(string name, RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(name, rectangleWithTexture, texture, index)
        {
        }

        public override GameObject GetGameObject() => new SpeedDownBonus(_rectangleWithTexture, _texture, _index)
        {
            Name = _name
        };
    }
}
