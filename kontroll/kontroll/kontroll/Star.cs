using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace kontroll
{
    class Star : GameObject
    {
        // Constructor(s)
        public Star()
            : base()
        {
            this.Position = new Vector2(Globals.Randomizer.Next(0, 800), 0);
            this.Texture = AssetManager.spritesheet;
            this.SpriteSize = new Point(3, 3);
            this.SpriteCoords = new Point(1, Frame(5, 32));
            this.Speed = Globals.Randomizer.Next(2, 5);
            this.Depth = 1 - (1 / (Speed - 1));
        }

        // Method(s)
        public override void Update()
        {
            base.Update();

            Position += new Vector2(0, Speed);
            if (Position.Y > 480)
            {
                GameObjectManager.Remove(this);
            }
        }
    }
}
