using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kontroll
{
    abstract class Enemy : GameObject
    {
        public int MaxHealth { private get; set; }
        public int Health { get; set; }

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
                if (p.Hitbox.Intersects(Hitbox))
                {
                    if (invisible)
                    {
                        Health -= p.Damage;
                    }

                    p.OnCollision();
                }
            }

            CheckHealth();
        }

        public void CheckHealth()
        {
            if (Health <= 0)
            {
                // TODO: add effect eller
                GameObjectManager.Remove(this);
            }
        }
    }
}
