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
    public enum MenuState { start, game }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        MenuState menuState;

        KeyboardState keyboard;
        KeyboardState prevKeyboard;

        SpriteFont spriteFont;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            AssetManager.Load(Content);
            GameObjectManager.Add(new Player());
            GameObjectManager.Add(new PowerUp(new Vector2(200, 0), 4));
            GameObjectManager.Add(new PowerUp(new Vector2(400, 0), 4));
            GameObjectManager.Add(new Ship(new Vector2(100, 100), 0, 3, 128));
            //GameObjectManager.Add(new Drone(new Vector2(0, 0), -1, -180));
            //GameObjectManager.Add(new Drone(new Vector2(0, 0), 1, 0));

            menuState = MenuState.start;

            spriteFont = Content.Load<SpriteFont>("SpriteFont20");
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            prevKeyboard = keyboard;
            keyboard = Keyboard.GetState();

            if (menuState == MenuState.start)
            {
                if (keyboard.IsKeyDown(Keys.Enter))
                {
                    menuState = MenuState.game;
                }
            }

            if (menuState == MenuState.game)
            {
                GameObjectManager.Update();

                // Star
                if (Globals.Randomizer.Next(0, 101) < 5)
                {
                    GameObjectManager.Add(new Star());
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(50, 50, 50));
            spriteBatch.Begin(SpriteSortMode.FrontToBack, null);
            if (menuState == MenuState.start)
            {
                spriteBatch.DrawString(spriteFont, "Press enter to start or escape to quit", new Vector2(100, 200), Color.White);
            }

            if (menuState == MenuState.game)
            {
                if (keyboard.IsKeyDown(Keys.P))
                {
                    menuState = MenuState.start;
                }

                foreach (GameObject g in GameObjectManager.gameObjects)
                {
                    g.Draw(spriteBatch);
                } 
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
