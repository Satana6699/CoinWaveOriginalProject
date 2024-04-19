using Coin_Wave_Lib.Objects.InterfaceObjects;
namespace Coin_Wave_Lib
{
    public class BlocksPanel : InterfaceObject
    {
        public List<ElementMenu> MenuElements { get; private set; } = new List<ElementMenu>(0);
        public override string Name { get => typeof(BlocksPanel).Name; set { } }

        int numberOCell;
        public ChoiceObj choiceObj;
        private TextureMap _textureMap;
        double unitX; //ширина одного контейнра
        double unitY;
        double xPos;
        double yPos;
        double zPos;
        public BlocksPanel
            (
                RectangleWithTexture rectangleWithTexture,
                int numberOfCell,
                Texture texture,
                TextureMap textureMap
            ) : base(rectangleWithTexture, texture)
        {
            // Если войдёт нечетное число, то сделать его чётным
            if (numberOfCell % 2 != 0) numberOfCell++;
            this.numberOCell = numberOfCell;
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
            if (MenuElements.Count == numberOCell / 2)
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
                        [new TexturePoint(0,1), new TexturePoint(1, 1), new TexturePoint(1, 0), new TexturePoint(0, 0)]
                    ),
                    texture
                );
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
            choiceObj.SetPoints(new Rectangle(new Point(xPos, yPos, zPos), width, hidth));
        }
        public void CountDimensions()
        {
            int row = numberOCell + 1;
            unitX = RectangleWithTexture.Rectangle.GetWidth() / row; //ширина одного контейнра
            unitY = RectangleWithTexture.Rectangle.GetHeight() / 5;
            xPos = RectangleWithTexture.Rectangle.TopLeft.X + unitX;
            yPos = RectangleWithTexture.Rectangle.TopLeft.Y - unitY;
            zPos = 0.0;
        }

        public override object Clone()
        {
            return new BlocksPanel()
            {
                RectangleWithTexture = (RectangleWithTexture)RectangleWithTexture.Clone(),
                Texture = Texture,
                Buffer = new Buffer(GetVertices())
            };
        }
    }
}
