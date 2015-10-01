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

        public int MaxHealth { private get; set; }
        public int Health { get; set; }
        public int FireRate { get; set; }
        public int MaxFireRate { get; set; }
        public int ShootIntervall { get; set; }
        public int BurstSize { get; set; }

        public Projectile projectile { get; set; }

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
                    if (invisible)
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

            if (FireRate >= MaxFireRate)
            {
                if (BurstSize == 0)
                {
                    GameObjectManager.Add(projectile);
                }
                FireRate = 0;
            }

            if (BurstSize > 1)
            {
                for (int i = 0; i < BurstSize; i++)
                {
                    if (FireRate == MaxFireRate - i * ShootIntervall)
                    {
                        GameObjectManager.Add(projectile);
                    }
                }
            }
        }

        public void CheckHealth()
        {
            if (Health <= 0)
            {
                // TODO: add effect eller
                GameObjectManager.Remove(this);
            }

            if (hitCount >= 1)
            {
                Color = Color.Red;
                hitCount += 1;
            }

            hitCount = (hitCount >= MAX_HITCOUNT) ? 0 : hitCount;
        }
    }
}
