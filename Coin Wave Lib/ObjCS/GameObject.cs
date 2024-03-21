using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public abstract class GameObject
    {
        public List<Pnt> pnts = new List<Pnt>(0);
        public string path;
        public double[] vertices = new double[20];
        public double width;
        public double hidth;
        public Pnt pnt;
        public GameObject(Pnt pnt, double width, double hidth, string path)
        {
            this.pnt = pnt;
            this.width = width;
            this.hidth = hidth;

            pnts.Add(new Pnt(pnt.X,
                             pnt.Y,
                             pnt.Z,
                             1.0, 1.0));
            pnts.Add(new Pnt(pnt.X + width,
                             pnt.Y,
                             pnt.Z,
                             1.0, 0.0));
            pnts.Add(new Pnt(pnt.X + width,
                             pnt.Y - hidth,
                             pnt.Z,
                             0.0, 0.0));
            pnts.Add(new Pnt(pnt.X,
                             pnt.Y - hidth,
                             pnt.Z,
                             0.0, 1.0));
            this.path = path;
        }
        public void NewPoints(Pnt pnt, double width, double hidth)
        {
            this.pnt = pnt;
            this.width = width;
            this.hidth = hidth;
            pnts = new List<Pnt>(0);
            pnts.Add(new Pnt(pnt.X,
                             pnt.Y,
                             pnt.Z,
                             1.0, 1.0));
            pnts.Add(new Pnt(pnt.X + width,
                             pnt.Y,
                             pnt.Z,
                             1.0, 0.0));
            pnts.Add(new Pnt(pnt.X + width,
                             pnt.Y - hidth,
                             pnt.Z,
                             0.0, 0.0));
            pnts.Add(new Pnt(pnt.X,
                             pnt.Y - hidth,
                             pnt.Z,
                             0.0, 1.0));
        }
        public double[] GetVertices()
        {
            int offset = 5;
            for(int i = 0, j = 0; j < 4; i+=offset, j++)
            {
                vertices[i] = pnts[j].X;
                vertices[i+1] = pnts[j].Y;
                vertices[i+2] = pnts[j].Z;
                vertices[i+3] = pnts[j].S;
                vertices[i+4] = pnts[j].T;
            }
            return vertices;
        }
    }
}
