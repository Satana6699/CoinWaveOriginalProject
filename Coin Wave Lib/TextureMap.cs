using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public record class TextureMap
    {
        private class MyTexture
        {
            public (double S, double T) TopLeft { get; private set; }
            public  (double S, double T) TopRight { get; private set; }
            public  (double S, double T) BottomRight { get; private set; }
            public  (double S, double T) BottomLeft { get; private set; }
            public MyTexture((double S, double T) topLeft,
                            (double S, double T) topRight,
                            (double S, double T) bottomRight,
                            (double S, double T) bottomLeft)
            {
                TopLeft = topLeft;
                TopRight = topRight;
                BottomRight = bottomRight;
                BottomLeft = bottomLeft;
            }
        }
        private int Width {  get; init; }
        private int Heidth {  get; init; }
        public string TexturePath {  get; init; }
        private double DistanceBetweenTextures { get; init; }
        private MyTexture[] _textures;

        public TextureMap(int width, int hidth, double distanceBetweenTextures, string texturePath)
        {
            Width = width;
            Heidth = hidth;
            _textures = new MyTexture[width * hidth];
            TexturePath = texturePath;
            DistanceBetweenTextures = distanceBetweenTextures;
        }
        public TextureMap(int width, int hidth, string texturePath)
        {
            //new TextureMap(width, hidth, 0, texturePath);
            if (width == 0) width = 1;
            if (hidth == 0) hidth = 1;
            Width = width;
            Heidth = hidth;
            _textures = new MyTexture[width * hidth];
            TexturePath = texturePath;
            DistanceBetweenTextures = 0;
            GenerateMap();
        }

        private void GenerateMap()
        {
            if (DistanceBetweenTextures == 0)
            {
                double UnitWidth = 1.0 / (double) Width;
                double UnitHeidth = 1.0 / (double) Heidth;
                // Верхний левый угол текстры соответствует координате (0,1)
                // Нижний правый угол текстуры соответствует координате (1,0)
                // В данном случе x и y это верхний левый угол
                double xPos = 0.0;
                double yPos = 1.0;
                for(int i = 0; i < _textures.Length; i++)
                {
                    _textures[i] = new MyTexture((xPos, yPos),
                                                (xPos + UnitWidth, yPos),
                                                (xPos + UnitWidth, yPos - UnitHeidth),
                                                (xPos, yPos - UnitHeidth));
                    if ((i + 1) % Width == 0 && i != 0)
                    {
                        xPos = 0.0;
                        yPos -= UnitHeidth;
                    }
                    else
                    {
                        xPos += UnitWidth;
                    }
                }
            }
            else
            {
                //НЕ РЕАЛИЗОВАННО
                //НЕ РЕАЛИЗОВАННО
                //НЕ РЕАЛИЗОВАННО
                //НЕ РЕАЛИЗОВАННО
                //НЕ РЕАЛИЗОВАННО
                //НЕ РЕАЛИЗОВАННО
                //НЕ РЕАЛИЗОВАННО
                //НЕ РЕАЛИЗОВАННО
                //НЕ РЕАЛИЗОВАННО
                //НЕ РЕАЛИЗОВАННО
                //НЕ РЕАЛИЗОВАННО
                //НЕ РЕАЛИЗОВАННО
                //НЕ РЕАЛИЗОВАННО
                //НЕ РЕАЛИЗОВАННО
            }
        }

        public Rctngl NewTextureCoords(Rctngl rctngl, int numberTexture)
        {
            rctngl.TopLeft.NewTextureCoords(_textures[numberTexture - 1].TopLeft.S, _textures[numberTexture - 1].TopLeft.T);
            rctngl.TopRight.NewTextureCoords(_textures[numberTexture - 1].TopRight.S, _textures[numberTexture - 1].TopRight.T);
            rctngl.BottomRight.NewTextureCoords(_textures[numberTexture - 1].BottomRight.S, _textures[numberTexture - 1].BottomRight.T);
            rctngl.BottomLeft.NewTextureCoords(_textures[numberTexture - 1].BottomLeft.S, _textures[numberTexture - 1].BottomLeft.T);
            return rctngl;
        }
    }
}
