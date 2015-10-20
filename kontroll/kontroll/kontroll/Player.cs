using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace kontroll
{
    class Player : GameObject
    {
        private const int MAX_RESPAWN_COUNT = 128;

        private KeyboardState keyboard;
        private KeyboardState prevKeyboard;

        private Keys left = Keys.Left;
        private Keys right = Keys.Right;
        private Keys up = Keys.Up;
        private Keys down = Keys.Down;
        private Keys fire = Keys.X;
        private Keys leftTrigger = Keys.A;
        private Keys rightTrigger = Keys.S;

        private int respawnCount;
        private int invisibleCount;

        public int MaxFireRate { get; set; }

        public int Score { get; set; }

        private int fireRate;

        public int GunType { get; set; }
        public Action gunType;

        public int Lives { get; set; }

        public bool dead;

        public Laser laser;

        public Player()
            : base()
        {
            Texture = AssetManager.spritesheet;
            SpriteSize = new Point(32, 32);
            SpriteCoords = new Point(1, 1);
            Position = new Vector2(400, 400);

            Lives = 3;
            dead = false;

            Globals.gameOver = false;

            gunType = () => Globals.SimpelShot(this, this.Speed+4, -(float)Math.PI/2);

            MaxFireRate = 16;

            Speed = 5;
            Depth = 0.6f;
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

            if (keyboard.IsKeyDown(fire) && !prevKeyboard.IsKeyDown(fire) && fireRate <= 0)
            {
                gunType();
                fireRate = 1;
            }
        }

        public override void Update()
        {
            if(!dead) Input();

            //gunType = Globals.LaserShot;

            if (gunType == Globals.LaserShot) MaxFireRate = 128;

            if (dead)
            {
                respawnCount += 1;

                if(respawnCount % 16 == 0)
                GameObjectManager.Add(new Explosion(Position));

                Position = new Vector2(Position.X, Globals.Lerp(Position.Y, 620, 0.04f));

                if (respawnCount >= MAX_RESPAWN_COUNT && Lives > 0)
                {
                    dead = false;
                    GunType = 0;
                    respawnCount = 0;

                    invisibleCount = 1;
                    Lives -= 1;

                    Position = new Vector2(400, 240);
                }
            }

            Globals.gameOver = (Lives <= 0);

            foreach (Enemy e in GameObjectManager.gameObjects.Where(item => item is Enemy))
            {
                if (e.Hitbox.Intersects(Hitbox)) 
                {
                    e.Health = 0;
                    if(invisibleCount <= 0) dead = true;
                }
            }

            foreach(Projectile p in GameObjectManager.gameObjects.Where(item => item is Projectile))
            {
                if (p.Hitbox.Intersects(Hitbox) && p.enemy)
                {
                    GameObjectManager.Remove(p);
                    if (invisibleCount <= 0) dead = true;
                }
            }

            if(laser != null) laser.Update();

            //laser = new Laser(Position, Vector2.Zero, new Color(Globals.Randomizer.Next(0, 255), Globals.Randomizer.Next(0, 255), Globals.Randomizer.Next(0, 255), Globals.Randomizer.Next(0, 255)), false);

            fireRate = (fireRate >= MaxFireRate) ? 0 : fireRate;
            fireRate = (fireRate >= 1) ? fireRate + 1 : fireRate;

            if (fireRate >= 1 && fireRate <= MaxFireRate / 4 && gunType == Globals.LaserShot)
            {
                laser = new Laser(Position, Position + new Vector2(0, -500), new Color(Globals.Randomizer.Next(0, 255), Globals.Randomizer.Next(0, 255), Globals.Randomizer.Next(0, 255), Globals.Randomizer.Next(0, 255)), true);
            }
            else
            {
                laser = null;
            }

            if (invisibleCount >= 1)
            {
                invisibleCount += 1;
                if (invisibleCount >= 128) invisibleCount = 0;
            }

            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if(laser != null) laser.Draw(spriteBatch);
            if(invisibleCount > 0) spriteBatch.Draw(AssetManager.spritesheet, Position, new Rectangle(1, 232, 32, 32), Color.White, 0, new Vector2(16, 16), 1, SpriteEffects.None, Depth+0.1f);
        }
    }
}
