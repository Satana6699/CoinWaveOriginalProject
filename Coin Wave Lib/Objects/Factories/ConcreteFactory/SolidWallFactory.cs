using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.Objects.Factories.ConcreteFactory
{
    public class SolidWallFactory : GameObjectFactory
    {
        public SolidWallFactory(string name, RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(name, rectangleWithTexture, texture, index)
        {
        }

        public override GameObject GetGameObject() => new SolidWall(_rectangleWithTexture, _texture, _index)
        {
            Name = _name,
            IsSolid = true,
        };
    }
}
