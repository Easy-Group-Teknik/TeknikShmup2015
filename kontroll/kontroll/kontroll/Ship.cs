using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace kontroll
{
    class Ship : Enemy
    {
        private SimpleProjectile.Pattern pattern;

        public Ship(Vector2 position, float angle, float speed, int maxFireRate)
            : base()
        {
            this.Position = position;
            this.Speed = speed;
            this.Angle = angle;

            ShootAngle = this.Angle;

            Rotation = this.Angle;

            SpriteCoords = new Point(1, Frame(2, 32));
            SpriteSize = new Point(32, 32);

            MaxHealth = 1;
            Health = MaxHealth;

            this.MaxFireRate = maxFireRate;
            ShootIntervall = 8;
            BurstSize = 4;
            this.border = RemoveOnSide(this.Angle);
            Worth = 500;

            pattern = SimpleProjectile.RandomPattern;
        }

        public override void Update()
        {
            ShootAngle = Rotation;

            Projectile = new SimpleProjectile(Position, ShootAngle, 6, Color.Tomato, pattern, true);
            UpdateShoot();

            Position += Velocity;

            base.Update();
        }
    }
}
