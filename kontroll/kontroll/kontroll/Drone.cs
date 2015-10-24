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
        const int MAX_INSIVSIBLECOUNT = 128;

        public int Tag { get; private set; }

        private int fireRate;
        public int GunType { private get; set; }
        public Action gunType { get; set; }

        public int MaxFireRate { get; set; }

        private int invsibileCount;

        public float ShootAngle { get; set; }

        public bool dead;

        private Laser laser;

        public Drone(Vector2 position, int tag, float shootAngle)
            : base()
        {
            this.Position = position;
            this.Tag = tag;

            this.ShootAngle = Globals.DegreesToRadian(shootAngle);

            SpriteCoords = new Point(1, Frame(1, 32));
            SpriteSize = new Point(24, 24);

            Speed = 0.2f;
            this.Depth = 0.5f;

            gunType = () => Globals.SimpelShot(this, 7, this.ShootAngle);

            Texture = AssetManager.spritesheet;
        }

        public void Shoot()
        {
            if (fireRate <= 0)
            {
                gunType();
                fireRate = 1;
            }
        }

        public override void Update()
        {
            base.Update();

            //gunType = Globals.LaserShot;

            if (gunType == Globals.LaserShot) MaxFireRate = 128;

            fireRate = (fireRate >= 1) ? fireRate + 1 : 0;
            fireRate = (fireRate >= MaxFireRate) ? 0 : fireRate;

            if(invsibileCount <= MAX_INSIVSIBLECOUNT) invsibileCount += 1;

            foreach (Enemy e in GameObjectManager.gameObjects.Where(item => item is Enemy))
            {
                if (e.Hitbox.Intersects(Hitbox))
                {
                    e.Health = 0;
                    if (invsibileCount > MAX_INSIVSIBLECOUNT) dead = true;
                }
            }

            foreach (Projectile p in GameObjectManager.gameObjects.Where(item => item is Projectile))
            {
                if (p.Hitbox.Intersects(Hitbox) && p.enemy)
                {
                    GameObjectManager.Remove(p);
                    if (invsibileCount > MAX_INSIVSIBLECOUNT) dead = true;
                }
            }

            foreach(Player p in GameObjectManager.gameObjects.Where(item => item is Player))
            {
                if (p.dead) dead = p.dead;
                Position = new Vector2(Globals.Lerp(Position.X, p.Position.X + MAX_DISTANCE * Tag, Speed), Globals.Lerp(Position.Y, p.Position.Y, Speed));
            }

            if (dead)
            {
                GameObjectManager.Add(new Explosion(Position));
                GameObjectManager.Remove(this);
            }

            if (laser != null) laser.Update();

            if (fireRate >= 1 && fireRate <= 128 / 4 && gunType == Globals.LaserShot)
            {
                laser = new Laser(Position, Position + new Vector2((float)Math.Cos(ShootAngle) * 800, (float)Math.Sin(ShootAngle) * 800), new Color(Globals.Randomizer.Next(0, 255), Globals.Randomizer.Next(0, 255), Globals.Randomizer.Next(0, 255), Globals.Randomizer.Next(0, 255)), true);
            }
            else
            {
                laser = null;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if(laser != null) laser.Draw(spriteBatch);
            if (invsibileCount < MAX_INSIVSIBLECOUNT) spriteBatch.Draw(AssetManager.spritesheet, Position, new Rectangle(1, 232, 32, 32), Color.White, 0, new Vector2(16, 16), 1, SpriteEffects.None, Depth + 0.1f);
        }
    }
}
