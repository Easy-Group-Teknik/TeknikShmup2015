using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace kontroll
{
    class Player : GameObject
    {
        private KeyboardState keyboard;
        private KeyboardState prevKeyboard;

        private Keys left = Keys.Left;
        private Keys right = Keys.Right;
        private Keys up = Keys.Up;
        private Keys down = Keys.Down;
        private Keys fire = Keys.X;
        private Keys leftTrigger = Keys.A;
        private Keys rightTrigger = Keys.S;

        private int fireRate;
        private int maxFireRate;
        private int gunType;

        public Player()
            : base()
        {
            Texture = AssetManager.spritesheet;
            SpriteSize = new Point(32, 32);
            SpriteCoords = new Point(1, 1);
            Position = new Vector2(100, 100);

            Speed = 5;
        }

        public void Input()
        {
            prevKeyboard = keyboard;
            keyboard = Keyboard.GetState();

            if(keyboard.IsKeyDown(left) && Position.X >= Speed+Orgin.X)
            {
                Position += new Vector2(-Speed, 0);
            }

            if (keyboard.IsKeyDown(right) && Position.X <= 800 - Speed - Orgin.X)
            {
                Position += new Vector2(Speed, 0);
            }

            if (keyboard.IsKeyDown(up) && Position.Y >= Orgin.Y)
            {
                Position += new Vector2(0, -Speed);
            }

            if (keyboard.IsKeyDown(down) && Position.Y <= 480 - Orgin.Y)
            {
                Position += new Vector2(0, Speed);
            }
        }

        public override void Update()
        {
            Input();
            base.Update();
        }
    }
}
