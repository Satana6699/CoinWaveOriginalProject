using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public abstract class GameObject
    {
        public string path;
        public Rctngl Rectangle {  get; set; }
        public GameObject(Rctngl rectangle, string path)
        {
            Rectangle = rectangle;
            this.path = path;
        }
        public void NewPoints(Rctngl sqrt)
        {
            Rectangle = sqrt;
        }
        public void NewPoints(Pnt leftTop, double width, double heidth)
        {
            Rectangle = new Rctngl(leftTop, width, heidth);
        }
        public double[] GetVertices()
        {
            return Rectangle.GetVertieces();
        }

        public static double[] GetVertices(Rctngl[] rctngls)
        {
            List<double> vertices = new List<double>();
            return vertices.ToArray();
        }
        public static double[] GetVertices(List<Rctngl> rctngls)
        {
            return GetVertices(rctngls.ToArray());
        }
    }
}
