﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.ObjCS
{
    public class EnterDoor : GameObject
    {
        public EnterDoor(Rctngl rectangle) : base(rectangle)
        {
        }

        public EnterDoor(int index, Rctngl rectangle) : base(index, rectangle)
        {
        }
    }
}
