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

        public int GunType { get; set; }

        public Player()
            : base()
        {
            Texture = AssetManager.spritesheet;
            SpriteSize = new Point(32, 32);
            SpriteCoords = new Point(1, 1);
            Position = new Vector2(100, 100);

            Speed = 5;
            Depth = 0.5f;
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

            if (keyboard.IsKeyDown(leftTrigger) && prevKeyboard.IsKeyUp(leftTrigger))
            {
                foreach (Drone d in GameObjectManager.gameObjects.Where(item => item is Drone))
                {
                    if (d.Tag == -1)
                    {
                        d.Shoot();
                    }
                }
            }

            if (keyboard.IsKeyDown(rightTrigger) && prevKeyboard.IsKeyUp(rightTrigger))
            {
                foreach (Drone d in GameObjectManager.gameObjects.Where(item => item is Drone))
                {
                    if (d.Tag == 1)
                    {
                        d.Shoot();
                    }
                }
            }

            if (keyboard.IsKeyDown(fire) && fireRate <= 0)
            {
                if (GunType == 0 && prevKeyboard.IsKeyUp(fire))
                {
                    GameObjectManager.add(new SimpleProjectile(Position, -(float)Math.PI / 2, Speed + 3, Color.Blue, SimpleProjectile.Pattern.Straight, false));
                }

                if (GunType == 1 && prevKeyboard.IsKeyUp(fire))
                {
                    for (int i = -1; i < 2; i++)
                    {
                        float angle = (-90 + i * 25) * (float)Math.PI/180;
                        GameObjectManager.add(new SimpleProjectile(Position, angle, Speed + 3, Color.Blue, SimpleProjectile.Pattern.Straight, false));
                    }
                }

                if (GunType == 2 && prevKeyboard.IsKeyUp(fire))
                {
                    GameObjectManager.add(new Rocket(Position, -(float)Math.PI / 2, 0, -0.2f, Rocket.Type.Slowing, Vector2.Zero, false));
                }

                fireRate = 1;
            }
        }

        public override void Update()
        {
            Input();

            fireRate = (fireRate >= MaxFireRate) ? 0 : fireRate;
            fireRate = (fireRate >= 1) ? fireRate + 1 : fireRate;

            base.Update();
        }

        public int MaxFireRate 
        {
            get
            {
                int tmp = 0;

                switch (GunType)
                {
                    case 0:
                        tmp = 16;
                        break;
                }

                return tmp;
            }
        }
    }
}
