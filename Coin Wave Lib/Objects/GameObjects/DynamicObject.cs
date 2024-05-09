using Coin_Wave_Lib.Objects.GameObjects;
using Coin_Wave_Lib.Objects.GameObjects.DynamicEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coin_Wave_Lib.Programs;
using Coin_Wave_Lib.Objects.GameObjects.NoSolidObjects;
using Coin_Wave_Lib.Objects.Factories;

namespace Coin_Wave_Lib.Objects.GameObjects
{
    public abstract class DynamicObject : GameObject, IDynamic
    {
        public int Time { get; private set; }
        public int FrameTime { get; private set; }
        private (double x, double y) _unit = (0, 0);

        public DynamicObject(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index) : base(rectangleWithTexture, texture, index)
        {
        }
        public DynamicObject()
        {
            
        }
        public void SetSpeed(int frameTime)
        {
            double x = RectangleWithTexture.Rectangle.GetWidth();
            double y = RectangleWithTexture.Rectangle.GetHeight();
            this.FrameTime = Time = frameTime;
            this._unit = (x / (double)frameTime, y / (double)frameTime);
            IsSolid = true;
        }
        public void MoveInOneFrame(GameObject gameObject)
        {
            Time--;
            double errrorRate = 0.0001;
            if (Math.Abs(gameObject.RectangleWithTexture.Rectangle.TopLeft.X - RectangleWithTexture.Rectangle.TopLeft.X) < errrorRate)
            {
                // Для более точного фиксированного расположения игрока на позиции, иначе позиция обновляется в каждом кадре
            }
            else if (gameObject.RectangleWithTexture.Rectangle.TopLeft.X < RectangleWithTexture.Rectangle.TopLeft.X)
                RectangleWithTexture.Rectangle = new
                    (
                        new Point
                        (
                            RectangleWithTexture.Rectangle.TopLeft.X - _unit.x,
                            RectangleWithTexture.Rectangle.TopLeft.Y,
                            RectangleWithTexture.Rectangle.TopLeft.Z
                        ),
                        RectangleWithTexture.Rectangle.GetWidth(),
                        RectangleWithTexture.Rectangle.GetHeight()
                    );
            else if (gameObject.RectangleWithTexture.Rectangle.TopLeft.X > RectangleWithTexture.Rectangle.TopLeft.X)
                RectangleWithTexture.Rectangle = new
                    (
                        new Point
                        (
                            RectangleWithTexture.Rectangle.TopLeft.X + _unit.x,
                            RectangleWithTexture.Rectangle.TopLeft.Y,
                            RectangleWithTexture.Rectangle.TopLeft.Z
                        ),
                        RectangleWithTexture.Rectangle.GetWidth(),
                        RectangleWithTexture.Rectangle.GetHeight()
                    );

            if (Math.Abs(gameObject.RectangleWithTexture.Rectangle.TopLeft.Y - RectangleWithTexture.Rectangle.TopLeft.Y) < errrorRate)
            {
                // Для более точного фиксированного расположения игрока на позиции, иначе позиция обновляется в каждом кадре
            }
            else if (gameObject.RectangleWithTexture.Rectangle.TopLeft.Y < RectangleWithTexture.Rectangle.TopLeft.Y)
                RectangleWithTexture.Rectangle = new
                    (
                        new Point
                        (
                            RectangleWithTexture.Rectangle.TopLeft.X,
                            RectangleWithTexture.Rectangle.TopLeft.Y - _unit.y,
                            RectangleWithTexture.Rectangle.TopLeft.Z
                        ),
                        RectangleWithTexture.Rectangle.GetWidth(),
                        RectangleWithTexture.Rectangle.GetHeight()
                    );
            else if (gameObject.RectangleWithTexture.Rectangle.TopLeft.Y > RectangleWithTexture.Rectangle.TopLeft.Y)
                RectangleWithTexture.Rectangle = new
                    (
                        new Point
                        (
                            RectangleWithTexture.Rectangle.TopLeft.X,
                            RectangleWithTexture.Rectangle.TopLeft.Y + _unit.y,
                            RectangleWithTexture.Rectangle.TopLeft.Z
                        ),
                        RectangleWithTexture.Rectangle.GetWidth(),
                        RectangleWithTexture.Rectangle.GetHeight()
                    );

            ReturnTime(gameObject, errrorRate);
            this.SetPoints(RectangleWithTexture.Rectangle);
        }

        public bool ContinueMove()
        {
            return Time == FrameTime;
        }

        private void ReturnTime(GameObject gameObject, double errrorRate)
        {
            if (Math.Abs(gameObject.RectangleWithTexture.Rectangle.TopLeft.X - RectangleWithTexture.Rectangle.TopLeft.X) < errrorRate &&
                Math.Abs(gameObject.RectangleWithTexture.Rectangle.TopLeft.Y - RectangleWithTexture.Rectangle.TopLeft.Y) < errrorRate)
            {
                Time = FrameTime;
            }
        }

        public static List<DynamicObject> SearchDynamicObjects(GameObject[,] gameObjects, TextureMap textureMap, int speedDynamicObject)
        {
            List<DynamicObject> dynamicObjects = new List<DynamicObject>();

            foreach (GameObject obj in gameObjects)
            {
                if (obj is IDynamic)
                {
                    GameObjectFactory objFactory = ObjectFactory.GetFactory
                        (
                            obj.Name,
                                new RectangleWithTexture
                                    (
                                        obj.RectangleWithTexture.Rectangle,
                                        obj.RectangleWithTexture.TexturePoints
                                    ),
                            textureMap.Texture,
                            obj.Index
                        );

                    DynamicObject gameObject = (DynamicObject)objFactory.GetGameObjectWithTexture();
                    if (gameObject is Stone)
                        gameObject.SetSpeed(speedDynamicObject);
                    else if (gameObject is FireWheel)
                        gameObject.SetSpeed(speedDynamicObject);
                    else if (gameObject is Monster)
                        gameObject.SetSpeed(speedDynamicObject * 3);
                    dynamicObjects.Add(gameObject);

                    gameObjects[obj.Index.y, obj.Index.x] = new Air
                        (
                            new RectangleWithTexture
                            (
                                obj.RectangleWithTexture.Rectangle,
                                textureMap.GetTexturePoints(Resources.Air)
                            ),
                            textureMap.Texture,
                            obj.Index
                        )
                    {
                        Name = typeof(Air).Name
                    };
                }
            }

            return dynamicObjects;
        }
    }
}
