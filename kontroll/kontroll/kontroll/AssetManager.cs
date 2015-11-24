using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace kontroll
{
    class AssetManager
    {
        static public Texture2D spritesheet;

        static public SpriteFont spriteFont;

        static public Texture2D pixel;

        static public void Load(ContentManager content, GraphicsDevice graphicsDevice)
        {
            spritesheet = content.Load<Texture2D>("spritesheet");
            spriteFont = content.Load<SpriteFont>("bitmapFont");

            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
        }
    }
}
