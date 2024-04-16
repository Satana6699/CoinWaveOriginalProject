using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.Objects.GameObjects
{
    public interface IMoveable
    {
        // я умею двигаться к обьекту
        // Стремление к точке
        // Но в данном случае стремление к сектору в сетке
        void Move(GameObject gameObject);
    }
}
