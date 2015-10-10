using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace kontroll
{
    class Bomber : Enemy
    {
        public Bomber(Vector2 position, float angle, float speed)
            : base()
        {
            this.Position = position;
            this.Speed = speed;
            this.Angle = angle;

            ShootAngle = Globals.DegreesToRadian(-270);

            SpriteCoords = new Point(1, 100);
            SpriteSize = new Point(32, 32);

            MaxHealth = 1;
            Health = MaxHealth;

            this.MaxFireRate = 48;

            Worth = 1000;
        }

        public override void Update()
        {
            Projectile = new Rocket(Position, ShootAngle, 0.1f, -0.05f, Rocket.Type.Slowing, Vector2.Zero, true);
            UpdateShoot();

            Position += Velocity;

            base.Update();
        }
    }
}
