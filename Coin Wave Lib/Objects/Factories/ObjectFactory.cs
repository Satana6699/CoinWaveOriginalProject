using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.Objects.Factories
{
    public abstract class ObjectFactory
    {
        public abstract Obj GetObject();
        public abstract Obj GetObjectNoTexture();
    }
}
