using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class ChoiceObj : InterfaceObject
    {
        public ChoiceObj()
        {
        }

        public ChoiceObj(RectangleWithTexture rectangleWithTexture, Texture texture) : base(rectangleWithTexture, texture)
        {
        }
    }
}
