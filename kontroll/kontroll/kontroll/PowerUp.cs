using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace kontroll
{
    class PowerUp : GameObject
    {
        private int type;

        private float sinCount;

        public PowerUp(Vector2 position, int type)
            : base()
        {
            this.Position = position;
            this.type = type;

            SpriteCoords = new Point(34, 34);
            SpriteSize = new Point(24, 16);

            Texture = AssetManager.spritesheet;

            Speed = 1;

            Depth = 0.9f;
        }

        public override void Update()
        {
            sinCount += 0.05f;

            Position += new Vector2((float)Math.Sin(sinCount) * 2, Speed);

            foreach (Player p in GameObjectManager.gameObjects.Where(item => item is Player))
            {
                if (p.Hitbox.Intersects(Hitbox))
                {
                    if(type != 4) p.GunType = type + 1;
                    else
                    {
                        if (GameObjectManager.gameObjects.Where(item => item is Drone).Count() == 0)
                        {
                            GameObjectManager.Add(new Drone(new Vector2(0, 0), -1, -180));
                        }
                        else if (GameObjectManager.gameObjects.Where(item => item is Drone).Count() == 1)
                        {
                            GameObjectManager.Add(new Drone(new Vector2(0, 0), 1, 0));
                        }
                        else
                        {
                            p.GunType = 1;
                        }
                    }
                    GameObjectManager.Remove(this);
                }
            }

            foreach (Drone d in GameObjectManager.gameObjects.Where(item => item is Drone))
            {
                if (d.Hitbox.Intersects(Hitbox))
                {
                    if (type != 4) d.GunType = type + 1;
                    else d.GunType = type + 1;
                    GameObjectManager.Remove(this);
                }
            }
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(Texture, Position, new Rectangle(66+Frame(type, 24), 34, 24, 16), Color.White, 0, new Vector2(12, 8), 1, SpriteEffects.None, Depth+0.1f);
        }
    }
}
