using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace kontroll
{
    abstract class GameObject
    {
        public Vector2 Position { get; set; }
        public Vector2 Orgin { get; set; }

        public Rectangle Hitbox { get { return new Rectangle((int)(Position.X - Orgin.X), (int)(Position.Y - Orgin.Y), SpriteSize.X, SpriteSize.Y); } }

        public Point SpriteCoords { get; set; }
        public Point SpriteSize { get; set; }

        public float Angle { get; set; }
        public float Speed { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; }

        public float Depth { get; set; }

        public Color Color { get; set; }

        public Texture2D Sprite { get; set; }

        public override void Update()
        {

        }

        public void DrawSprite(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Position, new Rectangle(SpriteCoords.X, SpriteCoords.Y, SpriteSize.X, SpriteSize.Y), Color, Rotation, Orgin, Scale, SpriteEffects.None, Depth);
        }
    }
}
