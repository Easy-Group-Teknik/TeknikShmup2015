using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace kontroll
{
    class UserInterface
    {
        private float displayScore;

        public UserInterface()
        {
            
        }

        public void Update()
        {
            foreach (Player p in GameObjectManager.gameObjects.Where(item => item is Player))
            {
                displayScore = Globals.Lerp(displayScore, p.Score, 0.1f);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(AssetManager.spriteFont, "SCORE: " + Convert.ToInt32(displayScore), new Vector2(10, 10), Color.White);

            foreach (Player p in GameObjectManager.gameObjects.Where(item => item is Player))
            {
                if (p.dead)
                {
                    if (p.Lives > 0)
                    {
                        spriteBatch.DrawString(AssetManager.spriteFont, "GET READY!", new Vector2(400, 220), Color.Gold, 0, new Vector2(AssetManager.spriteFont.MeasureString("GET READY!").X/2, 0), 1, SpriteEffects.None, 1);
                    }
                    else
                    {
                        spriteBatch.DrawString(AssetManager.spriteFont, "GAME OVER!", new Vector2(400, 220), Color.Red, 0, new Vector2(AssetManager.spriteFont.MeasureString("GAME OVER!").X / 2, 0), 1, SpriteEffects.None, 1);
                    }
                }
            }
        }
    }
}
