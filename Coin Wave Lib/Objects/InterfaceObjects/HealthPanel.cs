using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Coin_Wave_Lib.Objects.InterfaceObjects
{
    public class HealthPanel : InterfaceObject
    {
        [XmlIgnore] public TexturePoint[] FoolTexturePoinys;
        public override string Name { get; set; }
        public HealthPanel(RectangleWithTexture rectangleWithTexture, Texture texture) : base(rectangleWithTexture, texture)
        {
            FoolTexturePoinys = (TexturePoint[])rectangleWithTexture.TexturePoints.Clone();
        }
        public HealthPanel()
        {

        }

        public void RealTexturePoints(int procentHP)
        {
            double width = FoolTexturePoinys[1].S - FoolTexturePoinys[0].S;
            width = width / 2;
            double unitS = width / 100;
            if (procentHP <= 100 && procentHP >= 0)
            {
                double posS = FoolTexturePoinys[0].S + (100 - procentHP) * unitS;
                RectangleWithTexture.TexturePoints[0] = new(posS, RectangleWithTexture.TexturePoints[0].T);
                RectangleWithTexture.TexturePoints[1] = new(posS + width, RectangleWithTexture.TexturePoints[1].T);
                RectangleWithTexture.TexturePoints[2] = new(posS + width, RectangleWithTexture.TexturePoints[2].T);
                RectangleWithTexture.TexturePoints[3] = new(posS, RectangleWithTexture.TexturePoints[3].T);
            }
        }
    }
}
