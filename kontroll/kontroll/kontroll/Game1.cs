using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
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

        SpawnManager spawnManager = new SpawnManager();
        UserInterface userInterface = new UserInterface();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            CreateHighScoreFile();
            AssetManager.Load(Content, GraphicsDevice);
            GameObjectManager.Add(new Player());
            //GameObjectManager.Add(new PowerUp(new Vector2(200, 0), 4));
            //GameObjectManager.Add(new PowerUp(new Vector2(400, 0), 4));
           // GameObjectManager.Add(new Ship(new Vector2(300, 100), 0, 3, 128));
            //GameObjectManager.Add(new Drone(new Vector2(0, 0), -1, -180));
            //GameObjectManager.Add(new Drone(new Vector2(0, 0), 1, 0));
            //Globals.gameOver = true;
            menuState = MenuState.start;
            userInterface = new UserInterface();
        }

        public void CreateHighScoreFile()
        {
            if (!File.Exists("highscore.hi"))
            {
                File.Create("highscore.hi").Dispose();
                StreamWriter sw = new StreamWriter("highscore.hi");
                sw.Write("100000");
                sw.Dispose();
            }
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

                if (Globals.gameOver && keyboard.IsKeyDown(Keys.Space))
                {
                    GameObjectManager.gameObjects.Clear();
                    GameObjectManager.Add(new Player());
                    spawnManager = new SpawnManager();
                    userInterface = new UserInterface();
                }

                // Star
                if (Globals.Randomizer.Next(0, 101) < 5)
                {
                    GameObjectManager.Add(new Star());
                }

                spawnManager.Update();
                userInterface.Update();
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(10, 10, 10));
            spriteBatch.Begin(SpriteSortMode.FrontToBack, null);
            if (menuState == MenuState.start)
            {
                spriteBatch.DrawString(AssetManager.spriteFont, "Press enter to start or escape to quit", new Vector2(100, 200), Color.White);
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

            spriteBatch.Begin();
            userInterface.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
