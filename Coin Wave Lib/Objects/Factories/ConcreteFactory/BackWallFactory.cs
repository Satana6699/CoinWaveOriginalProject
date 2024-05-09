using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coin_Wave_Lib.Programs;
using Coin_Wave_Lib.Objects.GameObjects;
using Coin_Wave_Lib.Objects.GameObjects.NoSolidObjects;

namespace Coin_Wave_Lib.Objects.Factories.ConcreteFactory
{
    public class BackWallFactory : GameObjectFactory
    {
        public BackWallFactory(string name, RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(name, rectangleWithTexture, texture, index)
        {
        }

        public BackWallFactory(string name, RectangleWithTexture rectangleWithTexture, (int x, int y) index) : base(name, rectangleWithTexture, index)
        {
        }

        public override GameObject GetGameObjectWithTexture() => new BackWall(_rectangleWithTexture, _texture, _index)
        {
            Name = _name,
        };

        public override GameObject GetGameObject() => new BackWall()
        {
            RectangleWithTexture = _rectangleWithTexture,
            Index = _index,
            Name = _name,
        };
    }
}
