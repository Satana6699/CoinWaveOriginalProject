using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coin_Wave_Lib.Programs;
using Coin_Wave_Lib.Objects.GameObjects;
using Coin_Wave_Lib.Objects.GameObjects.DynamicEntity;

namespace Coin_Wave_Lib.Objects.Factories.ConcreteFactory
{
    public class DragonFactory : DynamicFactory
    {
        public DragonFactory(string name, RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(name, rectangleWithTexture, texture, index)
        {
        }

        public DragonFactory(string name, RectangleWithTexture rectangleWithTexture, (int x, int y) index) : base(name, rectangleWithTexture, index)
        {
        }

        public override GameObject GetGameObjectWithTexture() => new Dragon(_rectangleWithTexture, _texture, _index, 1)
        {
            Name = _name,
            IsSolid = true,
        };
        public override GameObject GetGameObject() => new Dragon()
        {
            RectangleWithTexture = _rectangleWithTexture,
            Index = _index,
            Name = _name,
        };
    }
}
