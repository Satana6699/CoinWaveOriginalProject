using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.Objects.Factories
{
    public class PlayerFactory : ObjectFactoy
    {
        private readonly string _name;
        private readonly RectangleWithTexture _rectangleWithTexture;
        private readonly Texture _texture;
        private readonly (int x, int y) _index;
        public PlayerFactory(string name, RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index)
        {
            _name = name;
            _rectangleWithTexture = rectangleWithTexture;
            _texture = texture;
            _index = index;
        }

        public override IMembership GetMembership()
        {
            Player player = new(_rectangleWithTexture, _texture, _index)
            {
                Name = _name
            };
            return player;
        }
    }
}
