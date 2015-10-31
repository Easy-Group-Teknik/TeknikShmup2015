using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace kontroll
{
    class SimpleProjectile : Projectile
    {
        public enum Pattern { Straight, Wave, Spiral };

        private Pattern pattern;

        private float cosCount;
        private float spiralSpeed;
        private float spiralAngle;

        private Vector2 spiralPosition;

        public SimpleProjectile(Vector2 position, float angle, float speed, Color color, Pattern pattern, bool enemy)
            : base(position, angle, speed, enemy)
        {
            SpriteCoords = new Point(Frame(1, 32), 1);
            SpriteSize = new Point(8, 8);
            this.Color = color;
            this.pattern = pattern;

            if (this.pattern == Pattern.Spiral)
            {
                spiralPosition = this.Position;

                spiralSpeed = this.Speed;
                this.Speed = 0;
            }
        }

        public override void Update()
        {
            if (pattern == Pattern.Wave)
            {
                cosCount += 0.3f;
                Position += new Vector2((float)Math.Cos(cosCount) * 5, 0);
            }
            else if (pattern == Pattern.Spiral)
            {
                spiralAngle += 0.3f;
                Position = spiralPosition + new Vector2((float)Math.Cos(spiralAngle) * 32, (float)Math.Sin(spiralAngle) * 32) - new Vector2(16, 16);
                spiralPosition += new Vector2((float)Math.Cos(Angle) * spiralSpeed, (float)Math.Sin(Angle) * spiralSpeed);
            }

            base.Update();
        }

        public static Pattern RandomPattern
        {
            get
            {
                int tmp = Globals.Randomizer.Next(2);

                return (tmp == 0) ? Pattern.Straight : Pattern.Wave;
            }
        }
    }
}
