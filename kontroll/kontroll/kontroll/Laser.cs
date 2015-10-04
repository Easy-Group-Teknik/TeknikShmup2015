using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace kontroll
{
    class Laser 
    {
        public Vector2 Position { get; set; }
        public Vector2 Target { get; set; }

        public bool enemy;

        private Color color;

        public Laser(Vector2 position, Vector2 target, Color color, bool enemy)
        {
            this.Position = position;
            this.Target = target;
            this.color = color;
            this.enemy = enemy;
        }

        // :^(
        public void Update()
        {
            if (enemy)
            {
                foreach (Enemy e in GameObjectManager.gameObjects.Where(item => item is Enemy))
                {
                    if (Intersects(e.Hitbox)) e.Health = 0;
                }
            }
            else
            {
                foreach (Player p in GameObjectManager.gameObjects.Where(item => item is Player))
                {
                    p.dead = true;
                }
            }
        }

        public bool Intersects(Rectangle hitbox)
        {
            float distance = Globals.DistanceTo(Position, Target);
            float angle = (float)Math.Atan2(Target.Y - Position.Y, Target.X - Position.X);
            
            Vector2 laserPoint;

            for (int i = 0; i < distance; i++)
            {
                laserPoint = new Vector2(Position.X + (float)Math.Cos(angle) * i, Position.Y + (float)Math.Sin(angle) * i);
                if(new Rectangle((int)laserPoint.X, (int)laserPoint.Y, 2, 2).Intersects(hitbox))
                {
                    return true;
                }
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float distance = Globals.DistanceTo(Position, Target);
            float angle = (float)Math.Atan2(Target.Y - Position.Y, Target.X - Position.X);

            for (int i = 0; i < distance; i++)
            {
                spriteBatch.Draw(AssetManager.spritesheet, Position + new Vector2((float)Math.Cos(angle) * i, (float)Math.Sin(angle) * i), new Rectangle(43, 1, 2, 2), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0.5f);
            }
        }
    }
}
