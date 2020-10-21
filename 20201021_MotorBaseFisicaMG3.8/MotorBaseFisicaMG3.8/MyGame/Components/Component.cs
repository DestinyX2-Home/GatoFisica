using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MotorBaseFisicaMG38.MyGame
{
    public abstract class Component
    {
        public abstract void Draw(GameTime gametime, SpriteBatch spriteBatch);

        public abstract void Update(GameTime gameTime);
    }
}
