using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Coin_Wave_Lib
{
    internal class PlayerFloat
    {
        public enum directionFloat
        {
            Up, 
            Down, 
            Left, 
            Right,
        }

        private float[] _position;
        /// <summary>
        /// Смещение. Через какой шаг в массиве будет та же характеристика в массиве _position
        /// </summary>
        private int _offset;
        private float[] _unitOfMovementX;
        private float[] _unitOfMovementY;
        private float _velocityX;
        private float _velocityY;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position">Массив позиций вершин игрока и дополнительных характеристик вершин</param>
        /// <param name="numberOfPoints">Колличество вершин у персонажа</param>
        public PlayerFloat(float[] position, int numberOfPoints, float velocityX, float velocityY)
        {
            _position = position;
            _velocityY = velocityY;
            _velocityX = velocityX;
            _offset = _position.Length / numberOfPoints;
            _unitOfMovementX = new float[_position.Length];
            Array.Clear(_unitOfMovementX, 0, _unitOfMovementX.Length);
            _unitOfMovementY = new float[_position.Length];
            Array.Clear(_unitOfMovementY, 0, _unitOfMovementY.Length);
            for (int i = 0; i < _position.Length; i += _offset)
            {
                _unitOfMovementX[i] = _velocityX;
                _unitOfMovementY[i+1] = _velocityY;
            }

        }

        public float[] GetPosition()
        {
            return _position;
        }

        public float[] Movement(directionFloat direction)
        {
            switch (direction)
            {
                case directionFloat.Up:
                    _position = _position.Zip(_unitOfMovementY, (a,b) => a+b).ToArray();
                    break;
                case directionFloat.Down:
                    _position = _position.Zip(_unitOfMovementY, (a, b) => a - b).ToArray();
                    break;
                case directionFloat.Left:
                    _position = _position.Zip(_unitOfMovementX, (a, b) => a - b).ToArray();
                    break;
                case directionFloat.Right:
                    _position = _position.Zip(_unitOfMovementX, (a, b) => a + b).ToArray();
                    break;
                default:
                    break;
            }

            return _position;
        }
    }
}
