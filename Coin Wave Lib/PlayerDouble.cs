using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Coin_Wave_Lib
{
    internal class PlayerDouble
    {
        public enum direction
        {
            Up, 
            Down, 
            Left, 
            Right,
        }

        private double[] _position;
        /// <summary>
        /// Смещение. Через какой шаг в массиве будет та же характеристика в массиве _position
        /// </summary>
        private int _offset;
        private double[] _unitOfMovementX;
        private double[] _unitOfMovementY;
        private double _velocityX;
        private double _velocityY;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position">Массив позиций вершин игрока и дополнительных характеристик вершин</param>
        /// <param name="numberOfPoints">Колличество вершин у персонажа</param>
        public PlayerDouble(double[] position, int numberOfPoints, double velocityX, double velocityY)
        {
            _position = position;
            _velocityY = velocityY;
            _velocityX = velocityX;
            _offset = _position.Length / numberOfPoints;
            _unitOfMovementX = new double[_position.Length];
            Array.Clear(_unitOfMovementX, 0, _unitOfMovementX.Length);
            _unitOfMovementY = new double[_position.Length];
            Array.Clear(_unitOfMovementY, 0, _unitOfMovementY.Length);
            for (int i = 0; i < _position.Length; i += _offset)
            {
                _unitOfMovementX[i] = _velocityX;
                _unitOfMovementY[i+1] = _velocityY;
            }

        }

        public double[] GetPosition()
        {
            return _position;
        }

        public double[] Movement(direction direction)
        {
            switch (direction)
            {
                case direction.Up:
                    _position = _position.Zip(_unitOfMovementY, (a,b) => a+b).ToArray();
                    break;
                case direction.Down:
                    _position = _position.Zip(_unitOfMovementY, (a, b) => a - b).ToArray();
                    break;
                case direction.Left:
                    _position = _position.Zip(_unitOfMovementX, (a, b) => a - b).ToArray();
                    break;
                case direction.Right:
                    _position = _position.Zip(_unitOfMovementX, (a, b) => a + b).ToArray();
                    break;
                default:
                    break;
            }

            return _position;
        }
    }
}
