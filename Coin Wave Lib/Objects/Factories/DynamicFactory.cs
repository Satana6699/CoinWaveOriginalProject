using Coin_Wave_Lib.Programs;

namespace Coin_Wave_Lib.Objects.Factories
{
    public abstract class DynamicFactory : GameObjectFactory
    {
        protected DynamicFactory(string name, RectangleWithTexture rectangleWithTexture, (int x, int y) index) : base(name, rectangleWithTexture, index)
        {
        }

        protected DynamicFactory(string name, RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(name, rectangleWithTexture, texture, index)
        {
        }
    }
}
