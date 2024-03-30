using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.Objects.InterfaceObjects
{
    public class ElementMenu : InterfaceObject
    {
        public string Name { get; private set; }
        public int IndexTexture { get; set; }
        public ElementMenu(Rectangle rectangle, TexturePoint[] texturePoints, string name, int indexTexture) :
            base(rectangle, texturePoints)
        {
            Name = name;
            IndexTexture = indexTexture;
        }

        public ElementMenu() {  }
    }
}
