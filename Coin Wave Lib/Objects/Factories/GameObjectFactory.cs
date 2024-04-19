using Coin_Wave_Lib.Objects.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public abstract class GameObjectFactory : ObjectFactory
    {
        protected readonly string _name;
        protected readonly RectangleWithTexture _rectangleWithTexture;
        protected readonly Texture _texture;
        protected readonly (int x, int y) _index;
        public GameObjectFactory(string name, RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index)
        {
            _name = name;
            _rectangleWithTexture = rectangleWithTexture;
            _texture = texture;
            _index = index;
        }
        public GameObjectFactory(string name, RectangleWithTexture rectangleWithTexture, (int x, int y) index)
        {
            _name = name;
            _rectangleWithTexture = rectangleWithTexture;
            _index = index;
        }
        public abstract GameObject GetGameObject();
        public abstract GameObject GetGameObjectNoTexture();
        public override Obj GetObject() => GetGameObject();
        public override Obj GetObjectNoTexture() => GetGameObjectNoTexture();
    }
}
