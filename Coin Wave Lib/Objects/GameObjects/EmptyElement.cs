﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class EmptyElement : GameObject
    {
        public EmptyElement()
        {
        }

        public EmptyElement(Rectangle rectangle, TexturePoint[] texturePoints, int index) : base(rectangle, texturePoints, index)
        {
        }
    }
}
