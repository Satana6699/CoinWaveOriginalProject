using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.Decorators
{
    public abstract class GameObjectDecorator : GameObject
    {
        private GameObject _gameObject;
        protected GameObjectDecorator(Rctngl rectangle, GameObject gameObject) : base(rectangle)
        {
            _gameObject = gameObject;
        }
    }
}
