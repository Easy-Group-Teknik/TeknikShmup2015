using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace kontroll
{
    class SimpleProjectile : Projectile
    {
        public SimpleProjectile(Vector2 position, float angle, float speed, Color color)
            : base(position, angle, speed)
        {
            SpriteCoords = new Point(Frame(1, 32), 1);
            SpriteSize = new Point(8, 8);
            this.Color = color;
        }
    }
}
