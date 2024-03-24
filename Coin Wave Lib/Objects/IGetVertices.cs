using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public interface IGetVertices
    {
        double[] GetVertices(Point[] points, TexturePoint[] texturePoints, int offset);
        double[] GetVertices(Obj[] objects, int offset);
    }
}
