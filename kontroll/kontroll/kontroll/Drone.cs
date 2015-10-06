using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace kontroll
{
    class Drone : GameObject
    {
        const float MAX_DISTANCE = 32;

        public int Tag { get; private set; }

        private int fireRate;
        public int GunType { private get; set; }

        private float shootAngle;

        private Laser laser;

        public Drone(Vector2 position, int tag, float shootAngle)
            : base()
        {
            this.Position = position;
            this.Tag = tag;

            this.shootAngle = Globals.DegreesToRadian(shootAngle);

            SpriteCoords = new Point(1, Frame(1, 32));
            SpriteSize = new Point(24, 24);

            Speed = 0.2f;
            this.Depth = 0.5f;

            GunType = 3;

            Texture = AssetManager.spritesheet;
        }

        public void Shoot()
        {
            if (fireRate <= 0)
            {
                switch (GunType)
                {
                    case 0: 
                        GameObjectManager.Add(new SimpleProjectile(Position, shootAngle, 7, Color.Blue, SimpleProjectile.Pattern.Straight, false));
                        break;
                    case 1:
                        for (int i = -1; i < 2; i++)
                        {
                            float angle = (Globals.RadianToDegree(shootAngle) + i * 25) * (float)Math.PI / 180;
                            GameObjectManager.Add(new SimpleProjectile(Position, angle, 7, Color.Blue, SimpleProjectile.Pattern.Straight, false));
                        }
                        break;
                    case 2:
                        GameObjectManager.Add(new Rocket(Position, shootAngle, 0, -0.2f, Rocket.Type.Slowing, Vector2.Zero, false));
                        break;
                }
                fireRate = 1;
            }
        }

        public override void Update()
        {
            base.Update();

            fireRate = (fireRate >= 1) ? fireRate + 1 : 0;
            fireRate = (fireRate >= MaxFireRate) ? 0 : fireRate;

            foreach(Player p in GameObjectManager.gameObjects.Where(item => item is Player))
            {
                Position = new Vector2(Globals.Lerp(Position.X, p.Position.X + MAX_DISTANCE * Tag, Speed), Globals.Lerp(Position.Y, p.Position.Y, Speed));
            }

            if (laser != null) laser.Update();

            if (fireRate >= 1 && fireRate <= MaxFireRate / 4 && GunType == 3)
            {
                laser = new Laser(Position, Position + new Vector2((float)Math.Cos(shootAngle) * 800, (float)Math.Sin(shootAngle) * 800), new Color(Globals.Randomizer.Next(0, 255), Globals.Randomizer.Next(0, 255), Globals.Randomizer.Next(0, 255), Globals.Randomizer.Next(0, 255)), true);
            }
            else
            {
                laser = null;
            }
        }

        public int MaxFireRate
        {
            get
            {
                int tmp = 0;

                switch (GunType)
                {
                    case 0:
                        tmp = 16;
                        break;
                    case 2:
                        tmp = 32;
                        break;
                    case 3:
                        tmp = 64;
                        break;
                }

                return tmp;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if(laser != null) laser.Draw(spriteBatch);
        }
    }
}
