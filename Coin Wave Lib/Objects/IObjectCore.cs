using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coin_Wave_Lib.Programs;
using Buffer = Coin_Wave_Lib.Programs.Buffer;

namespace Coin_Wave_Lib
{
    public interface IObjectCore
    {
        string Name { get; set; }
        double[] Vertices { get; set; }
        RectangleWithTexture RectangleWithTexture { get; set; }
        Texture Texture { get; set; }
        Buffer Buffer { get; set; }
    }
}
