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

        private int hitCount;

        public float ShootAngle { get; set; }

        public bool invisible;

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
                    if (!invisible && hitCount <= 0)
                    {
                        Health -= p.Damage;
                        hitCount = 1;
                    }

                    p.OnCollision();
                }
            }

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
