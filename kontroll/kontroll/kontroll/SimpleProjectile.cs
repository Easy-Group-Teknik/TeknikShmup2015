using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace kontroll
{
    class SimpleProjectile : Projectile
    {
        public enum Pattern { Straight, Wave };

        private Pattern pattern;

        private float cosCount;

        public SimpleProjectile(Vector2 position, float angle, float speed, Color color, Pattern pattern)
            : base(position, angle, speed)
        {
            SpriteCoords = new Point(Frame(1, 32), 1);
            SpriteSize = new Point(8, 8);
            this.Color = color;
            this.pattern = pattern;
        }

        public override void Update()
        {
            if (pattern == Pattern.Wave)
            {
                cosCount += 0.1f;
                Position += new Vector2((float)Math.Cos(cosCount) * 5, 0);
            }

            base.Update();
        }
    }
}
