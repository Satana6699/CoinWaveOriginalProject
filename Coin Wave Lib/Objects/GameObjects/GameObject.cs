using Coin_Wave_Lib.ObjCS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public abstract class GameObject : Obj
    {
        public int Index { get; private set; }
        protected GameObject(Rectangle rectangle, TexturePoint[] texturePoints, IGetVertices getVertices, int index) : 
            base(rectangle, texturePoints, getVertices)
        {
            this.Index = index;
        }

    }
}
