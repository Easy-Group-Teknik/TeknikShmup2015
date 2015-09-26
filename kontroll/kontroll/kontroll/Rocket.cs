using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace kontroll
{
    class Rocket : Projectile
    {
        public enum Type { Homing, Straight, Slowing}

        private float decay;

        private Type type;

        private Vector2 target;

        public Rocket(Vector2 position, float angle, float speed, float decay, Type type, Vector2 target, bool enemy)
            : base(position, angle, speed, enemy)
        {
            this.decay = decay;
            SpriteCoords = new Point(Frame(1, 32), 10);
            SpriteSize = new Point(8, 8);

            this.type = type;

            this.Rotation = angle;

            this.target = target;
        }

        public override void Update()
        {
            if (type == Type.Homing)
            {
                Angle = (float)Math.Atan2(Position.Y - target.Y, Position.X - target.X);
            }
            else if (type == Type.Slowing)
            {
                Speed = (Speed >= decay) ? Speed - decay : Speed;
            }
            base.Update();
        }
    }
}
