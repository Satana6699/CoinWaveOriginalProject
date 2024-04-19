using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.Objects.Factories.ConcreteFactory
{
    public class StartDoorFactory : SolidObjectFactory
    {
        public StartDoorFactory(string name, RectangleWithTexture rectangleWithTexture, (int x, int y) index) : base(name, rectangleWithTexture, index)
        {
        }

        public StartDoorFactory(string name, RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(name, rectangleWithTexture, texture, index)
        {
        }

        public override GameObject GetGameObject() => new StartDoor(_rectangleWithTexture, _texture, _index)
        {
            Name = _name,
            IsSolid = true,
        };
        public override GameObject GetGameObjectNoTexture() => new StartDoor(_rectangleWithTexture, _index)
        {
            Name = _name,
            IsSolid = true,
        };
    }
}
