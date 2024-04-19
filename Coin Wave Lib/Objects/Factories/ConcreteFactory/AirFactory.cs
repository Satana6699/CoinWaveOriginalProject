using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class AirFactory : GameObjectFactory
    {
        public AirFactory(string name, RectangleWithTexture rectangleWithTexture, (int x, int y) index) : base(name, rectangleWithTexture, index)
        {
        }

        public AirFactory(string name, RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(name, rectangleWithTexture, texture, index)
        {
        }
        public override GameObject GetGameObject() => new Air(_rectangleWithTexture, _texture, _index)
        {
            Name = _name,

        };
        public override GameObject GetGameObjectNoTexture() => new Air(_rectangleWithTexture, _index)
        {
            Name = _name,
        };
    }
}
