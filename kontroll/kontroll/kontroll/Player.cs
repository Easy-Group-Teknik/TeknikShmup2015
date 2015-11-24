using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.InteropServices;

namespace kontroll
{
    class Player : GameObject
    {
        public enum LifeKey
        {
            CapsLock = 0x14,
            NumLock = 0x90,
            ScrollLock = 0x91
        };

        private const int MAX_RESPAWN_COUNT = 128;
        private const float FRICTION = 0.9f;

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
        public int InvisibleCount { private get; set; }

        public int MaxFireRate { get; set; }

        public int Score { get; set; }

        private int fireRate;

        public Action gunType;

        public int Lives { get; set; }

        public bool dead;

        public Laser laser;

        private float velocityY;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        static extern bool SetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags,
        UIntPtr dwExtraInfo);

        //[DllImport("user32.dll")]
        //static extern UInt32 SendInput(UInt32 nInputs, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] INPUT[] pInputs, Int32 cbSize);

        const int VK_F5 = 0x14;
        public const Int32 WM_SYSCOMMAND = 0x0112;
        public const Int32 SC_SCREENSAVE = 0xF020;
        const UInt32 WM_KEYDOWN = 0x0100;
        IntPtr capsLock = new IntPtr(0x14);

        IntPtr handle = FindWindow(null, "kontroll");

        public void Activate(LifeKey life)
        {
            keybd_event((byte)life, 0x45, 0x0001, (UIntPtr)0);  
        }

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

            gunType = () => Globals.SimpelShot(this, this.Speed+4, -(float)Math.PI/2, SimpleProjectile.Pattern.Straight);

            MaxFireRate = 16;

            Speed = 7;
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
                //SendMessage(this.handle, WM_KEYDOWN, capsLock, handle);
               // SendMessage(this.handle, (UInt32)WM_KEYDOWN, (IntPtr)capsLock, handle);
                //keybd_event((byte)LifeKey.CapsLock, 0x45, 0x0001, (UIntPtr)0);  
                if (Position.Y < 480 - Orgin.Y) velocityY += 5.3f;
                SendMessage(this.handle, (UInt32)(WM_KEYDOWN), (IntPtr)capsLock, (IntPtr)0);
                gunType();
                fireRate = 1;
                //Globals.Beep();
            }
            //SendMessage(this.handle, (UInt32)0x5B, (IntPtr)0x0100, handle);
        }

        public override void Update()
        {
            if(!dead) Input();

            if (gunType == Globals.LaserShot) MaxFireRate = 128;

            Position += new Vector2(0, velocityY);

            velocityY *= FRICTION;

            if (Position.Y >= 480 - Orgin.Y)
            {
                velocityY = 0;
            }

            if (dead)
            {
                respawnCount += 1;

                if(respawnCount % 16 == 0)
                GameObjectManager.Add(new Explosion(Position));

                Position = new Vector2(Position.X, Globals.Lerp(Position.Y, 620, 0.04f));

                if (respawnCount >= MAX_RESPAWN_COUNT && Lives > 0)
                {
                    dead = false;
                    gunType = () => Globals.SimpelShot(this, this.Speed + 4, -(float)Math.PI / 2, SimpleProjectile.Pattern.Straight);
                    MaxFireRate = 16;
                    respawnCount = 0;

                    InvisibleCount = 128*4;
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
                    if(InvisibleCount <= 0) dead = true;
                }
            }

            foreach(Projectile p in GameObjectManager.gameObjects.Where(item => item is Projectile))
            {
                if (p.Hitbox.Intersects(Hitbox) && p.enemy)
                {
                    GameObjectManager.Remove(p);
                    if (InvisibleCount <= 0) dead = true;
                }
            }

            if(laser != null) laser.Update();

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

            if (InvisibleCount >= 1)
            {
                InvisibleCount += 1;
                if (InvisibleCount >= 128*5) InvisibleCount = 0;
            }

            base.Update();
        }

        public void TurnOffLight()
        {
            if (Lives == 3) Activate(LifeKey.NumLock);
            if (Lives == 2) Activate(LifeKey.CapsLock);
            if (Lives == 1) Activate(LifeKey.ScrollLock);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if(laser != null) laser.Draw(spriteBatch);
            if(InvisibleCount > 0) spriteBatch.Draw(AssetManager.spritesheet, Position, new Rectangle(1, 232, 32, 32), Color.White, 0, new Vector2(16, 16), 1, SpriteEffects.None, Depth+0.1f);
        }
    }
}
