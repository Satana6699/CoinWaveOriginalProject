using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    
    public class Thorn : Trap
    {
        public override string Name { get; set; }
        private bool _active = true;
        public Thorn(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index, int damage) : base(rectangleWithTexture, texture, index, damage)
        {
        }
        public Thorn()
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
            if (!_active)
                _active = true;
            else 
                Deactivate();
        }
        public void Deactivate()
        {
            _active = false;
        }

        public bool IsActine()
        {
            return _active;
        }
    }
}
