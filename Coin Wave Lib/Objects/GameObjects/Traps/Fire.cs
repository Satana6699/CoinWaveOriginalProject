using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class Fire : Trap
    {
        public override string Name { get; set; }
        private bool _active = false;
        public Fire(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index, int damage) : base(rectangleWithTexture, texture, index, damage)
        {
        }
        public int Damage()
        {
            if (_active)
                return base.Damage;
            return 0;
        }
        public void Activate()
        {
            _active = true;
        }
        public void Deactivate()
        {
            _active = false;
        }
    }
}
