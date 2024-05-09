﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coin_Wave_Lib.Programs;

namespace Coin_Wave_Lib.Objects.InterfaceObjects
{
    public class LayerInterface : InterfaceObject
    {
        public LayerInterface(RectangleWithTexture rectangleWithTexture, Texture texture) : base(rectangleWithTexture, texture)
        {
        }

        public override string Name { get => typeof(LayerInterface).Name; set { } }
    }
}
