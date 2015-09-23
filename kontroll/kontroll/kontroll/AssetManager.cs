using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace kontroll
{
    class AssetManager
    {
        static public Texture2D spritesheet;

        static public void Load(ContentManager content)
        {
            spritesheet = content.Load<Texture2D>("spritesheet");
        }
    }
}
