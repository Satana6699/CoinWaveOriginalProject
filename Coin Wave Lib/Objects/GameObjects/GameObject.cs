using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public abstract class GameObject : Obj
    {
        public (int x, int y)Index { get; set; }
        public static bool isSolid { get; set; } = false;   
        protected GameObject(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) :
            base(rectangleWithTexture, texture)
        {
            this.Index = index;
        }

        protected GameObject() { }
    }
}
