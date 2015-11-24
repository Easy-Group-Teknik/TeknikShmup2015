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

        private Texture2D deathFade;
        private float deathFadeOpacity;

        public UserInterface()
        {
            this.deathFade = AssetManager.pixel;
            this.deathFadeOpacity = 0;
            this.displayScore = 0;
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
                        deathFadeOpacity = 0;
                        spriteBatch.DrawString(AssetManager.spriteFont, "GET READY!", new Vector2(400, 220), Color.White, 0, new Vector2(AssetManager.spriteFont.MeasureString("GET READY!").X/2, 0), 1, SpriteEffects.None, 1);
                    }
                    else
                    {
                        deathFadeOpacity += 0.01f;
                        spriteBatch.Draw(deathFade, new Rectangle(0, 0, 800, 480), null, Color.Red * deathFadeOpacity, 0f, new Vector2(0, 0), SpriteEffects.None, 0);
                        spriteBatch.DrawString(AssetManager.spriteFont, "GAME OVER!", new Vector2(400, 220), Color.Black, 0, new Vector2(AssetManager.spriteFont.MeasureString("GAME OVER!").X / 2, 0), 1, SpriteEffects.None, 1);
                        spriteBatch.DrawString(AssetManager.spriteFont, "HIGHSCORE: " + Globals.Highscore.ToString() + ((Globals.Highscore == p.Score) ? " - NEW HIGHSCORE!" : ""), new Vector2(400, 260), (Globals.Highscore == p.Score) ? Color.White : Color.White, 0, new Vector2(AssetManager.spriteFont.MeasureString("HIGHSCORE: " + Globals.Highscore.ToString() + ((Globals.Highscore == p.Score) ? "NEW HIGHSCORE!" : "")).X / 2, 0), 1, SpriteEffects.None, 1);
                        spriteBatch.DrawString(AssetManager.spriteFont, "YOUR SCORE: " + p.Score.ToString(), new Vector2(400, 300), Color.White, 0, new Vector2(AssetManager.spriteFont.MeasureString("YOUR SCORE: " + p.Score.ToString()).X / 2, 0), 1, SpriteEffects.None, 1);
                    }
                }
            }
        }
    }
}
