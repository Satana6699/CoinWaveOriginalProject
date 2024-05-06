﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Wave_Lib
{
    public class TrapFire : Trap
    {
        public override string Name { get; set; }
        private int _fireCount;
        private int _activeFireCount = 0;
        public List<Fire> Fires { get; private set; } = new List<Fire>(0);
        public MoveHelper MoveHelper { get; set; } = MoveHelper.Right;

        public TrapFire(RectangleWithTexture rectangleWithTexture, Texture texture, (int x, int y) index, int damage) : base(rectangleWithTexture, texture, index, damage)
        {
            _fireCount = 3;
            Fires = new List<Fire>(_fireCount);
        }
        public TrapFire()
        {
            
        }

        public void ActiveOneFire()
        {
            if (_activeFireCount < _fireCount)
            {
                Fires[_activeFireCount].Visible = true;
                Fires[_activeFireCount].Activate();
                _activeFireCount++;
            }
            else
            {
                _activeFireCount = 0;
                DeactivateAllFire();
            }
        }

        private void DeactivateAllFire()
        {
            foreach (var fire in Fires)
            {
                fire.Visible = false;
                fire.Deactivate();
            }
        }

        public void GenerateFires(TextureMap textureMap, GameObject[,] gameObjects)
        {

            RectangleWithTexture rectangleWithTex = new(RectangleWithTexture.Rectangle, textureMap.GetTexturePoints(Resources.Fire));
            (int x, int y) index = Index;

            for (int i = 0; i < _fireCount; i++)
            {
                if (MoveHelper == MoveHelper.Right) index.x++;
                else if (MoveHelper == MoveHelper.Left) index.x++;
                else if (MoveHelper == MoveHelper.Up) index.x++;
                else if (MoveHelper == MoveHelper.Down) index.x++;

                rectangleWithTex.Rectangle = gameObjects[index.y, index.x].RectangleWithTexture.Rectangle;

                Fires.Add(new Fire
                (
                    rectangleWithTex,
                    Texture,
                    index,
                    Damage
                ));
            }
            foreach ( Fire f in Fires )
            {
                f.Visible = false;
            }

            
        }

        public override void Render()
        {
            base.Render();
            foreach ( Fire f in Fires ) { f.Render(); }
        }
    }
}
