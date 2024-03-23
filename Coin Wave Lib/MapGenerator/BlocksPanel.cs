using Coin_Wave_Lib.ObjCS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib.MapGenerator
{
    public class BlocksPanel : GameObject
    {
        public List<GameObject> gameObjects = new List<GameObject>(0);
        int numberOfSeats;
        public ViborObj viborObj;
        public BlocksPanel(Rctngl rectangle, int numberOfSeats) : base(rectangle)
        {
            if (numberOfSeats%2 != 0) numberOfSeats++;
            this.numberOfSeats = numberOfSeats;
        }
        public void GenerateTexturViborObj(string path)
        {
            viborObj = new ViborObj(new Rctngl(Rectangle.TopLeft, 0, 0));
        }
        public void ObjVibor(int index)
        {
            double percanteX = 0.03;
            double percanteY = 0.08;
            double xPos = gameObjects[index].Rectangle.TopLeft.X - Math.Abs(gameObjects[index].Rectangle.TopLeft.X * percanteX);
            double yPos = gameObjects[index].Rectangle.TopLeft.Y + Math.Abs(gameObjects[index].Rectangle.TopLeft.Y * percanteY);
            double width = gameObjects[index].Rectangle.GetWidth() * 1.1;
            double hidth = gameObjects[index].Rectangle.GetHeight() * 1.1;
            viborObj.NewPoints(new Rctngl(new Pnt(xPos, yPos, 0,0,0), width, hidth));
            }
        public void AddGameObject(List<GameObject> gameObjects)
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                this.gameObjects.Add(gameObjects[i]);
            }
            PlacingObjectsInPanel();
        }
        public void AddGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
        }

        public void PlacingObjectsInPanel()
        {
            int row = numberOfSeats + 1;
            double unitX = Rectangle.GetWidth() / row; //ширина одного контейнра
            double unitY = Rectangle.GetHeight() / 5;
            double xPos = Rectangle.TopLeft.X + unitX;
            double yPos = Rectangle.TopLeft.Y - unitY;
            for (int i = 0;i < gameObjects.Count;i++)
            {
                gameObjects[i].NewPoints(new Rctngl(
                        new Pnt(
                            xPos,
                            yPos,
                            gameObjects[i].Rectangle.TopLeft.Z, 
                            gameObjects[i].Rectangle.TopLeft.S, 
                            gameObjects[i].Rectangle.TopLeft.T),
                        new Pnt(
                            xPos + unitX,
                            yPos,
                            gameObjects[i].Rectangle.TopRight.Z,
                            gameObjects[i].Rectangle.TopRight.S,
                            gameObjects[i].Rectangle.TopRight.T),
                        new Pnt(
                            xPos + unitX,
                            yPos - unitY,
                            gameObjects[i].Rectangle.BottomRight.Z,
                            gameObjects[i].Rectangle.BottomRight.S,
                            gameObjects[i].Rectangle.BottomRight.T),
                        new Pnt(
                            xPos,
                            yPos - unitY,
                            gameObjects[i].Rectangle.BottomLeft.Z,
                            gameObjects[i].Rectangle.BottomLeft.S,
                            gameObjects[i].Rectangle.BottomLeft.T)
                        ));
                xPos = xPos + 2 * unitX;
                if (i+1 == numberOfSeats / 2)
                {
                    xPos = Rectangle.TopLeft.X + unitX;
                    yPos = yPos - 2 * unitY;
                }
            }
        }
    }
}