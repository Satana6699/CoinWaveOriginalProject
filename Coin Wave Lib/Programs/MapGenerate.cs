using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class MapGenerate
    {
        private int _width;
        private int _height;
        public double _sizeX;
        public double _sizeY;
        private List<double> _points;
        public List<Point> mainPoints;
        public MapGenerate(int width, int heidth)
        {
            _width = width;
            _height = heidth;
            _sizeX = (double) 2 / width;
            _sizeY = (double) 2/ heidth;
        }
        public void GeneratePoints()
        {
            double x = -1.0;
            double y =  1.0;
            double z = 0.0;
            _points = new List<double>(0);
            mainPoints = new List<Point>(0);
            for (int i = 0; i < _width * _height; i++)
            {   
                if (i%_width == 0 && i != 0)
                {
                    y -= _sizeY;
                    x = -1.0;
                }
                else if (i != 0)
                {
                    x += _sizeX;
                }
                // 1
                _points.Add(x);
                _points.Add(y);
                _points.Add(z);
                _points.Add(1.0);
                _points.Add(1.0);
                mainPoints.Add(new Point(x, y));
                // 2
                _points.Add(x + _sizeX);
                _points.Add(y);
                _points.Add(z);
                _points.Add(1.0);
                _points.Add(0.0);
                // 3
                _points.Add(x + _sizeX);
                _points.Add(y - _sizeY);
                _points.Add(z);
                _points.Add(0.0);
                _points.Add(0.0);
                // 4
                _points.Add(x);
                _points.Add(y - _sizeY);
                _points.Add(z);
                _points.Add(0.0);
                _points.Add(1.0);
            }
        }

        public double[] GetPoints()
        {
            return _points.ToArray();
        }
    }
}
