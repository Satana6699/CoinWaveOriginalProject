using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coin_Wave_Lib.Programs;
using Coin_Wave_Lib.Objects.GameObjects;
using Coin_Wave_Lib.Objects.GameObjects.Player;

namespace Coin_Wave_Lib.Objects.Factories.ConcreteFactory
{
    public class PlayerFactory : GameObjectFactory
    {
        public PlayerFactory(string name, RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(name, rectangleWithTexture, texture, index)
        {
        }

        public PlayerFactory(string name, RectangleWithTexture rectangleWithTexture, (int x, int y) index) : base(name, rectangleWithTexture, index)
        {
        }

        public override GameObject GetGameObjectWithTexture() => new Player(_rectangleWithTexture, _texture, _index)
        {
            Name = _name
        };
        public override GameObject GetGameObject() => new Player()
        {
            RectangleWithTexture = _rectangleWithTexture,
            Index = _index,
            Name = _name
        };
    }
}
