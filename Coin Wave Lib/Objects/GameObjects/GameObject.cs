using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public abstract class GameObject : Obj
    {
        public int Index { get; set; }
        protected GameObject(Rectangle rectangle, TexturePoint[] texturePoints, int index) :
            base(rectangle, texturePoints)
        {
            this.Index = index;
        }

        protected GameObject() { }
    }
}
