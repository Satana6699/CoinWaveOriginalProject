﻿using Coin_Wave_Lib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coin_Wave_Lib.Programs;

namespace Coin_Wave_Lib.Objects.GameObjects
{
    public abstract class GameObject : Obj, IObjectCore, IGameCore
    {
        public (int x, int y) Index { get; set; }
        public bool IsSolid { get; set; }
        protected GameObject(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) :
            base(rectangleWithTexture, texture)
        {
            this.Index = index;
            IsSolid = false;
        }
        protected GameObject()
        {
            IsSolid = false;
        }
    }
}
