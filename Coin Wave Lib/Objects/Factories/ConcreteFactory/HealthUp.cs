using Coin_Wave_Lib.Objects.GameObjects.Boneses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Coin_Wave_Lib.Objects.Factories.ConcreteFactory
{
    public class HealthUpFactory : GameObjectFactory
    {
        public HealthUpFactory(string name, RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(name, rectangleWithTexture, texture, index)
        {
        }

        public HealthUpFactory(string name, RectangleWithTexture rectangleWithTexture, (int x, int y) index) : base(name, rectangleWithTexture, index)
        {
        }

        public override GameObject GetGameObjectWithTexture() => new HealthUpBonus(_rectangleWithTexture, _texture, _index)
        {
            Name = _name
        };
        public override GameObject GetGameObject() => new HealthUpBonus()
        {
            RectangleWithTexture = _rectangleWithTexture,
            Index = _index,
            Name = _name,
        };
    }
}
