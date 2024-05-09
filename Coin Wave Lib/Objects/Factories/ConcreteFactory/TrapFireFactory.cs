using Coin_Wave_Lib.Programs;
using Coin_Wave_Lib.Objects.GameObjects;
using Coin_Wave_Lib.Objects.GameObjects.Traps;

namespace Coin_Wave_Lib.Objects.Factories.ConcreteFactory
{
    internal class TrapFireFactory : GameObjectFactory
    {
        public TrapFireFactory(string name, RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(name, rectangleWithTexture, texture, index)
        {
        }

        public TrapFireFactory(string name, RectangleWithTexture rectangleWithTexture, (int x, int y) index) : base(name, rectangleWithTexture, index)
        {
        }

        public override GameObject GetGameObjectWithTexture() => new TrapFire(_rectangleWithTexture, _texture, _index, 1)
        {
            Name = _name
        };
        public override GameObject GetGameObject() => new TrapFire()
        {
            RectangleWithTexture = _rectangleWithTexture,
            Index = _index,
            Name = _name,
        };
    }
}
