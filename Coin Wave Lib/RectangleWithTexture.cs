using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Coin_Wave_Lib
{
    public class RectangleWithTexture
    {
        [XmlElement(ElementName = nameof(Coin_Wave_Lib.Rectangle))]
        public Rectangle Rectangle {  get; set; }
        [XmlElement(ElementName = nameof(TexturePoint))]
        public TexturePoint[] TexturePoints {  get; set; }
        public RectangleWithTexture(Rectangle rectangle, TexturePoint[] texturePoints)
        {
            this.Rectangle = rectangle;
            this.TexturePoints = texturePoints;
        }
        public RectangleWithTexture()
        {
            
        }
        public RectangleWithTexture(Point TopR, Point TopL, Point BotL, Point BotR, TexturePoint[] texturePoints)
        {
            Rectangle = new Rectangle(TopL, TopR, BotL, BotR);
            this.TexturePoints = texturePoints;
        }

        public double[] ToArray()
        {
            return Rectangle.TopLeft.ToArray().
                Concat(TexturePoints[0].ToArray()).ToArray().
                Concat(Rectangle.TopRight.ToArray()).ToArray().
                Concat(TexturePoints[1].ToArray()).ToArray().
                Concat(Rectangle.BottomRight.ToArray()).ToArray().
                Concat(TexturePoints[2].ToArray()).ToArray().
                Concat(Rectangle.BottomLeft.ToArray()).ToArray().
                Concat(TexturePoints[3].ToArray()).ToArray();
        }

        public object Clone()
        {
            return new RectangleWithTexture
                (
                    (Rectangle)Rectangle.Clone(),
                    new TexturePoint[]
                    {
                        (TexturePoint)TexturePoints[0].Clone(),
                        (TexturePoint)TexturePoints[1].Clone(),
                        (TexturePoint)TexturePoints[2].Clone(),
                        (TexturePoint)TexturePoints[3].Clone(),
                    }
                );
        }
    }
}
