using Coin_Wave_Lib.Objects.InterfaceObjects;
namespace Coin_Wave_Lib
{
    public class BlocksPanel : InterfaceObject
    {
        public List<ElementMenu> MenuElements { get; private set; } = new List<ElementMenu>(0);
        public override string Name { get => typeof(BlocksPanel).Name; set { } }

        int numberOfCell;
        public ChoiceObj choiceObj;
        private TextureMap _textureMap;
        double unitX; //ширина одного контейнра
        double unitY;
        double xPos;
        double yPos;
        double zPos;
        int lines;
        int objectInLine;
        public BlocksPanel
            (
                RectangleWithTexture rectangleWithTexture,
                int numberOfCell,
                int lines,
                Texture texture,
                TextureMap textureMap
            ) : base(rectangleWithTexture, texture)
        {
            this.lines = lines;

            while (true)
            {
                // Если войдёт некратное число, то сделать его кратным
                if (numberOfCell % lines != 0) numberOfCell++;
                else break;
            }

            this.numberOfCell = numberOfCell;
            objectInLine = numberOfCell / lines;
            _textureMap = textureMap;
            CountDimensions();
        }
        public BlocksPanel() { }

        public void GenerateMenuElement(string name, int indexTexture)
        {
            MenuElements.Add(new ElementMenu
                (
                    new RectangleWithTexture
                    (
                        new Rectangle
                        (

                            new Point
                            (
                                xPos,
                                yPos,
                                zPos
                            ),
                            new Point
                            (
                                xPos + unitX,
                                yPos,
                                zPos
                            ),
                            new Point
                            (
                                xPos + unitX,
                                yPos - unitY,
                                zPos
                            ),
                            new Point
                            (
                                xPos,
                                yPos - unitY,
                                zPos
                            )
                        ),
                        _textureMap.GetTexturePoints(indexTexture)
                    ),
                    _textureMap.Texture,
                    name,
                    indexTexture
                ));

            xPos = xPos + 2 * unitX;
            if (MenuElements.Count % objectInLine == 0)
            {
                xPos = RectangleWithTexture.Rectangle.TopLeft.X + unitX;
                yPos = yPos - 2 * unitY;
            }
        }
        public void GenerateTexturViborObj(Texture texture)
        {
            choiceObj = new ChoiceObj
                (
                    new RectangleWithTexture
                    (
                        new Rectangle(RectangleWithTexture.Rectangle.TopLeft, 0, 0),
                        [new TexturePoint(0, 1), new TexturePoint(1, 1), new TexturePoint(1, 0), new TexturePoint(0, 0)]
                    ),
                    texture
                );
        }
        public void ObjVibor(int index)
        {
            if (MenuElements[index] == null)
                return;
            double percante = 0.08;
            //double xPos = MenuElements[index].GetRectangle().TopLeft.X - Math.Abs(MenuElements[index].GetRectangle().TopLeft.X * percanteX);
            //double yPos = MenuElements[index].GetRectangle().TopLeft.Y + Math.Abs(MenuElements[index].GetRectangle().TopLeft.Y * percanteY);
            //double width = MenuElements[index].GetRectangle().GetWidth() * 1.1;
            //double hidth = MenuElements[index].GetRectangle().GetHeight() * 1.1;

            // --- Top Left ---
            choiceObj.RectangleWithTexture.Rectangle.TopLeft.X = MenuElements[index].GetRectangle().TopLeft.X - MenuElements[index].GetRectangle().GetWidth() * percante;
            choiceObj.RectangleWithTexture.Rectangle.TopLeft.Y = MenuElements[index].GetRectangle().TopLeft.Y + MenuElements[index].GetRectangle().GetHeight() * percante;

            // --- Top Right ---
            choiceObj.RectangleWithTexture.Rectangle.TopRight.X = MenuElements[index].GetRectangle().TopRight.X + MenuElements[index].GetRectangle().GetWidth() * percante;
            choiceObj.RectangleWithTexture.Rectangle.TopRight.Y = MenuElements[index].GetRectangle().TopRight.Y + MenuElements[index].GetRectangle().GetHeight() * percante;

            // --- Bottom Right ---
            choiceObj.RectangleWithTexture.Rectangle.BottomRight.X = MenuElements[index].GetRectangle().BottomRight.X + MenuElements[index].GetRectangle().GetWidth() * percante;
            choiceObj.RectangleWithTexture.Rectangle.BottomRight.Y = MenuElements[index].GetRectangle().BottomRight.Y - MenuElements[index].GetRectangle().GetHeight() * percante;

            // --- Bottom Left ---
            choiceObj.RectangleWithTexture.Rectangle.BottomLeft.X = MenuElements[index].GetRectangle().BottomLeft.X - MenuElements[index].GetRectangle().GetWidth() * percante;
            choiceObj.RectangleWithTexture.Rectangle.BottomLeft.Y = MenuElements[index].GetRectangle().BottomLeft.Y - MenuElements[index].GetRectangle().GetHeight() * percante;
            //choiceObj.SetPoints(new Rectangle(new Point(xPos, yPos, zPos), width, hidth));
        }
        public void CountDimensions()
        {
            unitX = RectangleWithTexture.Rectangle.GetWidth() / (objectInLine * 2 + 1); //ширина одного контейнра
            unitY = RectangleWithTexture.Rectangle.GetHeight() / (lines * 2 + 1);
            xPos = RectangleWithTexture.Rectangle.TopLeft.X + unitX;
            yPos = RectangleWithTexture.Rectangle.TopLeft.Y - unitY;
            zPos = 0.0;
        }
    }
}
