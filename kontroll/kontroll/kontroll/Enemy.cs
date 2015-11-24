using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace kontroll
{
    abstract class Enemy : GameObject
    {
        public const int MAX_HITCOUNT = 8;

        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public int FireRate { get; set; }
        public int MaxFireRate { get; set; }
        public int ShootIntervall { get; set; }
        public int BurstSize { get; set; }
        public int Worth { get; set; }

        public Projectile Projectile { get; set; }

        public Vector2 border;

        private bool negativeBorder;
        public bool dontCheckBorders;

        private int hitCount;

        public float ShootAngle { get; set; }

        public Enemy()
            : base()
        {
            Texture = AssetManager.spritesheet;
        }

        public override void Update()
        {
            base.Update();

            foreach (Projectile p in GameObjectManager.gameObjects.Where(item => item is Projectile))
            {
                if (p.Hitbox.Intersects(Hitbox) && !p.enemy)
                {
                    if (hitCount <= 0)
                    {
                        Health -= p.Damage;
                        hitCount = 1;
                    }

                    p.OnCollision();
                }
            }

            // Spring ner alla gränser eller
            if(!dontCheckBorders) UpdateBorderCheck();
            CheckHealth();
        }

        public void UpdateShoot()
        {
            FireRate += 1;

            if (Position.Y >= 480 + SpriteSize.Y) GameObjectManager.Remove(this);

            if (FireRate >= MaxFireRate)
            {
                if (BurstSize == 0)
                {
                    GameObjectManager.Add(Projectile);
                }
                FireRate = 0;
            }

            if (BurstSize > 1)
            {
                for (int i = 0; i < BurstSize; i++)
                {
                    if (FireRate == MaxFireRate - i * ShootIntervall)
                    {
                        GameObjectManager.Add(Projectile);
                    }
                }
            }
        }

        public void UpdateBorderCheck()
        {
            if(negativeBorder)
            {
                if (Position.X <= border.X)
                    GameObjectManager.Remove(this);
            }
            else
            {
                if (Position.X >= border.X && Position.Y >= border.Y)
                    GameObjectManager.Remove(this);
            }
        }

        public Vector2 RemoveOnSide(float angle)
        {
            Vector2 border = Vector2.Zero;

            if (Globals.RadianToDegree(angle) == -180)
            {
                border = new Vector2(-SpriteSize.X, 0);
                negativeBorder = true;
            }
            if (Globals.RadianToDegree(angle) == -270)
            {
                border = new Vector2(0, 480 + SpriteSize.Y);
                negativeBorder = false;
            }
            if (Globals.RadianToDegree(angle) == 0)
            {
                border = new Vector2(800 + SpriteSize.X, 0);
                negativeBorder = false;
            }

            return border;
        }

        public void CheckHealth()
        {
            if (Health <= 0)
            {
                GameObjectManager.Add(new Explosion(Position));
                foreach (Player p in GameObjectManager.gameObjects.Where(item => item is Player))
                {
                    p.Score += Worth;
                }
                GameObjectManager.Remove(this);
            }

            if (hitCount >= 1)
            {
                Color = Color.Red;
                hitCount += 1;
            }

            if (hitCount >= MAX_HITCOUNT)
            {
                Color = Color.White;
                hitCount = 0;
            }
        }
    }
}
