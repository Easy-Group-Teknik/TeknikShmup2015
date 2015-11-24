using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace kontroll
{
    class PowerUp : GameObject
    {
        private int type;

        private float sinCount;

        public PowerUp(Vector2 position)
            : base()
        {
            this.Position = position;
            this.type = Globals.Randomizer.Next(0, 5);

            SpriteCoords = new Point(34, 34);
            SpriteSize = new Point(24, 16);

            Texture = AssetManager.spritesheet;

            Speed = 1;

            Depth = 0.9f;
        }

        public override void Update()
        {
            sinCount += 0.05f;

            Position += new Vector2((float)Math.Sin(sinCount) * 2, Speed);

            if (Position.Y >= 480+32) GameObjectManager.Remove(this);

            foreach (Player p in GameObjectManager.gameObjects.Where(item => item is Player))
            {
                if (p.Hitbox.Intersects(Hitbox))
                {
                    if (type != 4)
                    {
                        p.gunType = GetGunType(type + 1, p, p.Speed + 4, Globals.DegreesToRadian(-90));
                        p.MaxFireRate = 16 * (type + 1);
                        if (type + 1 == 4) p.MaxFireRate = 2;
                        //Console.WriteLine(type+1);
                    }
                    else
                    {
                        if (GameObjectManager.gameObjects.Where(item => item is Drone).Count() == 0)
                        {
                            GameObjectManager.Add(new Drone(new Vector2(0, 0), -1, -180));
                        }
                        else if (GameObjectManager.gameObjects.Where(item => item is Drone).Count() == 1)
                        {
                            GameObjectManager.Add(new Drone(new Vector2(0, 0), 1, 0));
                        }
                        else
                        {
                            p.gunType = GetGunType(1, p, p.Speed + 4, Globals.DegreesToRadian(-90));
                        }
                    }
                    GameObjectManager.Remove(this);
                }
            }

            foreach (Drone d in GameObjectManager.gameObjects.Where(item => item is Drone))
            {
                if (d.Hitbox.Intersects(Hitbox) && type != 4)
                {
                    d.gunType = GetGunType(type + 1, d, Speed + 4, d.ShootAngle);
                    d.MaxFireRate = 16 * (type + 1);
                    GameObjectManager.Remove(this);
                }
            }
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(Texture, Position, new Rectangle(66+Frame(type, 24), 34, 24, 16), Color.White, 0, new Vector2(12, 8), 1, SpriteEffects.None, Depth+0.1f);
        }

        public Action GetGunType(int gunType, GameObject g, float speed, float angle)
        {
            Action[] gunTypes = new Action[5] { () => Globals.SimpelShot(g, speed, angle, SimpleProjectile.Pattern.Straight), () => Globals.ShotgunShot(g, speed, angle), () => Globals.RocketShot(g, speed, angle), Globals.LaserShot, () => Globals.SimpelShot(g, speed, angle, SimpleProjectile.Pattern.Wave) };

            return gunTypes[gunType];
        }
    }
}
