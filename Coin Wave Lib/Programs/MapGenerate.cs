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
        private double _offsetWidth;
        private double _offsetUp;
        private double _offsetDown;
        public double _unitX;
        public double _unitY;
        private List<double> _points;
        public List<Point> mainPoints;
        public MapGenerate(int width, int heidth, double offsetWidth, double offsetUp, double offsetDown)
        {
            _width = width;
            _height = heidth;
            _unitX = (double) (2 - 2 * offsetWidth) / width;
            _unitY = (double) (2 - (offsetUp + offsetDown))/ heidth;
            _offsetWidth = offsetWidth;
            _offsetUp = offsetUp;
            _offsetDown = offsetDown;
            GeneratePoints();
        }
        public void GeneratePoints()
        {
            double x = -1.0 + _offsetWidth;
            double y =  1.0 - _offsetUp;
            double z = 0.0;
            _points = new List<double>(0);
            mainPoints = new List<Point>(0);
            for (int i = 0; i < _width * _height; i++)
            {   
                if (i % _width == 0 && i != 0)
                {
                    y -= _unitY;
                    x = -1.0 + _offsetWidth;
                }
                else if (i != 0)
                {
                    x += _unitX;
                }
                // 1
                _points.Add(x);
                _points.Add(y);
                _points.Add(z);
                _points.Add(1.0);
                _points.Add(1.0);
                mainPoints.Add(new Point(x, y));
                // 2
                _points.Add(x + _unitX);
                _points.Add(y);
                _points.Add(z);
                _points.Add(1.0);
                _points.Add(0.0);
                // 3
                _points.Add(x + _unitX);
                _points.Add(y - _unitY);
                _points.Add(z);
                _points.Add(0.0);
                _points.Add(0.0);
                // 4
                _points.Add(x);
                _points.Add(y - _unitY);
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
