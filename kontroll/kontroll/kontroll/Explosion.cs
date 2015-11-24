using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace kontroll
{
    class Explosion : GameObject
    {
        public Explosion(Vector2 position)
            : base()
        {
            this.Position = position;

            MaxAnimationCount = 4;
            MaxFrame = 5;

            Depth = 1;

            SpriteCoords = new Point(1, Frame(6, 32));
            SpriteSize = new Point(32, 32);
            Texture = AssetManager.spritesheet;
        }

        public override void Update()
        {
            SpriteCoords = new Point(Frame(CurrentFrame, 32), SpriteCoords.Y);

            Animate();

            if (CurrentFrame == MaxFrame - 1)
            {
                GameObjectManager.Remove(this);
            }

            base.Update();
        }
    }
}
