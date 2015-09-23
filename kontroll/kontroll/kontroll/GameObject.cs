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
        public Vector2 Orgin { get { return new Vector2(SpriteSize.X / 2, SpriteSize.Y / 2); } }
        public Vector2 Velocity { get { return new Vector2((float)Math.Cos(Angle) * Speed, (float)Math.Sin(Angle) * Speed); } }

        public Rectangle Hitbox { get { return new Rectangle((int)(Position.X - Orgin.X), (int)(Position.Y - Orgin.Y), SpriteSize.X, SpriteSize.Y); } }

        public Point SpriteCoords { get; set; }
        public Point SpriteSize { get; set; }

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
            Scale = 1;
        }

        // Method(s)
        public virtual void Update()
        {

        }

        public int Frame(int cell, int size)
        {
            return cell * size + 1 + cell;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, new Rectangle(SpriteCoords.X, SpriteCoords.Y, SpriteSize.X, SpriteSize.Y), Color, Rotation, Orgin, Scale, SpriteEffects.None, Depth);
        }
    }
}
