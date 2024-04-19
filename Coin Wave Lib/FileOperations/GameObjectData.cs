using Coin_Wave_Lib.Objects.InterfaceObjects;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;


namespace Coin_Wave_Lib
{
    public class GameObjectData : GameObject
    {
        public override string Name { get; set; }
        public GameObjectData()
        {
            
        }
        public override object Clone()
        {
            return new GameObjectData()
            {
                RectangleWithTexture = (RectangleWithTexture)RectangleWithTexture.Clone(),
                Texture = Texture,
                Buffer = new Buffer(GetVertices())
            };
        }
    }
}