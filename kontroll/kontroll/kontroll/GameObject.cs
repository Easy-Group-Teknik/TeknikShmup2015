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
        // Public properties (för jävla många idiot Tom)
        public Vector2 Position { get; set; }
        public Vector2 Orgin { get { return new Vector2(Texture.Width / 2, Texture.Height / 2); } }

        public Rectangle Hitbox { get { return new Rectangle((int)(Position.X - Orgin.X), (int)(Position.Y - Orgin.Y), Texture.Width, Texture.Height); } }

        public float Angle { get; set; }
        public float Speed { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; }
        public float Depth { get; set; }

        public Color Color { get; set; }

        public Texture2D Texture { get; set; }

        // Constructor(s)
        public GameObject()
        {
            this.Color = Color.White;
        }

        // Method(s)
        public virtual void Update()
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color, Rotation, Orgin, Scale, SpriteEffects.None, Depth);
        }
    }
}
