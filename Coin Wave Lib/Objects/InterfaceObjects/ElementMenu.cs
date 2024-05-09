using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coin_Wave_Lib.Programs;

namespace Coin_Wave_Lib.Objects.InterfaceObjects
{
    public class ElementMenu : InterfaceObject
    {
        public override string Name { get; set; }
        public int IndexTexture { get; set; }
        public ElementMenu(RectangleWithTexture rectangleWithTexture, Texture texture, string name, int indexTexture) :
            base(rectangleWithTexture, texture)
        {
            Name = name;
            IndexTexture = indexTexture;
        }

        public ElementMenu() { }
    }
}
