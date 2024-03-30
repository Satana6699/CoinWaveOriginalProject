using Coin_Wave_Lib.Objects.InterfaceObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class BlocksPanel : InterfaceObject
    {
        public List<ElementMenu> MenuElements { get; private set; } = new List<ElementMenu>(0);
        int numberOfSeats;
        public ViborObj viborObj;
        private TextureMap _textureMap;
        double unitX; //ширина одного контейнра
        double unitY;
        double xPos;
        double yPos;
        public BlocksPanel
            (
                Rectangle rectangle,
                TexturePoint[] texturePoints,
                int numberOfSeats,
                TextureMap textureMap
            ) : base(rectangle, texturePoints)
        {
            if (numberOfSeats % 2 != 0) numberOfSeats++;
            this.numberOfSeats = numberOfSeats;
            _textureMap = textureMap;
            CountDimensions();
        }

        public BlocksPanel() { }

        public void GenerateMenuElement(string name, int texture)
        {
            MenuElements.Add(new ElementMenu
                (
                    new Rectangle
                    (
                        new Point
                        (
                            xPos,
                            yPos
                        ),
                        new Point
                        (
                            xPos + unitX,
                            yPos
                        ),
                        new Point
                        (
                            xPos + unitX,
                            yPos - unitY
                        ),
                        new Point
                        (
                            xPos,
                            yPos - unitY
                        )
                    ),
                    _textureMap.GetTexturePoints(texture),
                    name,
                    texture
                ));

            xPos = xPos + 2 * unitX;
            if (MenuElements.Count == numberOfSeats / 2)
            {
                xPos = Rectangle.TopLeft.X + unitX;
                yPos = yPos - 2 * unitY;
            }
        }
        public void GenerateTexturViborObj(string path)
        {
            viborObj = new ViborObj(new Rectangle(Rectangle.TopLeft, 0, 0), 
                new TexturePoint[] {new TexturePoint(0,1), new TexturePoint(1, 1), new TexturePoint(1, 0), new TexturePoint(0, 0)});
        }
        public void ObjVibor(int index)
        {
            if (MenuElements[index] == null)
                return;
            double percanteX = 0.03;
            double percanteY = 0.08;
            double xPos = MenuElements[index].GetRectangle().TopLeft.X - Math.Abs(MenuElements[index].GetRectangle().TopLeft.X * percanteX);
            double yPos = MenuElements[index].GetRectangle().TopLeft.Y + Math.Abs(MenuElements[index].GetRectangle().TopLeft.Y * percanteY);
            double width = MenuElements[index].GetRectangle().GetWidth() * 1.1;
            double hidth = MenuElements[index].GetRectangle().GetHeight() * 1.1;
            viborObj.NewPoints(new Rectangle(new Point(xPos, yPos), width, hidth));
        }

        public void CountDimensions()
        {
            int row = numberOfSeats + 1;
            unitX = Rectangle.GetWidth() / row; //ширина одного контейнра
            unitY = Rectangle.GetHeight() / 5;
            xPos = Rectangle.TopLeft.X + unitX;
            yPos = Rectangle.TopLeft.Y - unitY;
        }
    }
}
