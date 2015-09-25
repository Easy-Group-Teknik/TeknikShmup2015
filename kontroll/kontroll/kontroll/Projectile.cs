using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace kontroll
{
    abstract class Projectile : GameObject
    {
        public int Damage { private get; set; }

        public bool enemy;

        public Projectile(Vector2 position, float angle, float speed, bool enemy)
        {
            this.Position = position;
            this.Angle = angle;
            this.Speed = speed;
            this.enemy = enemy;

            Texture = AssetManager.spritesheet;

            Scale = 1f;
            Color = Color.White;
        }

        public void CheckCollision()
        {

        }

        public override void Update()
        {
            Position += Velocity;

            CheckCollision();

            base.Update();
        }
    }
}
