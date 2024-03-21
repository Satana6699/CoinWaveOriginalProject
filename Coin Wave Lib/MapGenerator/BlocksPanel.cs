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
        public BlocksPanel(Pnt pnt, double width, double hidth, string path, int numberOfSeats) : base(pnt, width, hidth, path)
        {
            if (numberOfSeats%2 != 0) numberOfSeats++;
            this.numberOfSeats = numberOfSeats;
        }
        public void GenerateTexturViborObj(string path)
        {
            viborObj = new ViborObj(pnt, 0, 0, path);
        }
        public void ObjVibor(int index)
        {
            double percanteX = 0.03;
            double percanteY = 0.08;
            double xPos = gameObjects[index].pnts[0].X - Math.Abs(gameObjects[index].pnts[0].X * percanteX);
            double yPos = gameObjects[index].pnts[0].Y + Math.Abs(gameObjects[index].pnts[0].Y * percanteY);
            double width = Math.Abs(gameObjects[index].pnts[1].X + Math.Abs(gameObjects[index].pnts[1].X * percanteX) - xPos);
            double hidth = Math.Abs(gameObjects[index].pnts[2].Y - Math.Abs(gameObjects[index].pnts[2].Y * 0.1) - yPos);
            viborObj.NewPoints(new Pnt(xPos,
                yPos,
                gameObjects[index].pnts[0].Z,
                gameObjects[index].pnts[0].S,
                gameObjects[index].pnts[0].T),
                width,
                hidth);
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
            double unitX = width / row; //ширина одного контейнра
            double unitY = hidth / 5;
            double xPos = pnt.X + unitX;
            double yPos = pnt.Y - unitY;
            for (int i = 0;i < gameObjects.Count;i++)
            {
                gameObjects[i].NewPoints(new Pnt(xPos, yPos, 0.0, 0.0, 0.0), unitX, unitY);
                xPos = xPos + 2 * unitX;
                if (i+1 == numberOfSeats / 2)
                {
                    xPos = pnt.X + unitX;
                    yPos = yPos - 2 * unitY;
                }
            }
        }
    }
}
