﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coin_Wave_Lib.Programs;
using Coin_Wave_Lib.Objects.GameObjects;
using Coin_Wave_Lib.Objects.GameObjects.SolidObjects;

namespace Coin_Wave_Lib.Objects.Factories.ConcreteFactory
{
    public class SolidWallFactory : GameObjectFactory
    {
        public SolidWallFactory(string name, RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(name, rectangleWithTexture, texture, index)
        {
        }

        public SolidWallFactory(string name, RectangleWithTexture rectangleWithTexture, (int x, int y) index) : base(name, rectangleWithTexture, index)
        {
        }

        public override GameObject GetGameObjectWithTexture() => new SolidWall(_rectangleWithTexture, _texture, _index)
        {
            Name = _name,
            IsSolid = true,
        };
        public override GameObject GetGameObject() => new SolidWall()
        {
            RectangleWithTexture = _rectangleWithTexture,
            Index = _index,
            Name = _name,
        };
    }
}
