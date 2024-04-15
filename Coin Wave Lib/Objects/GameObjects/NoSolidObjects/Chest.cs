﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class Chest : GameObject, IMembership, IGameMembership
    {
        public Chest()
        {
        }

        public Chest(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(rectangleWithTexture, texture, index)
        {
        }

        public override string Name { get => typeof(Chest).Name; set { } }
    }
}
