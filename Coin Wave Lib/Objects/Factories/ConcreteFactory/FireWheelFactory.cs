﻿using Coin_Wave_Lib.Objects.GameObjects.DynamicEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.Objects.Factories.ConcreteFactory
{
    public class FireWheelFactory : GameObjectFactory
    {
        public FireWheelFactory(string name, RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(name, rectangleWithTexture, texture, index)
        {
        }

        public override GameObject GetGameObject() => new FireWheel(_rectangleWithTexture, _texture, _index, 1)
        {
            Name = _name,
            IsSolid = true,
        };
    }
}