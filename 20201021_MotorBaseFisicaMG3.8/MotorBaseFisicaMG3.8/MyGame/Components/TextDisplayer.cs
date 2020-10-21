using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MotorBaseFisicaMG38.MyGame.Components
{
    class TextDisplayer : Component
    {
        #region  Fields
        private SpriteFont _font;
        private Texture2D _texture;
        #endregion

        #region Properties
        public float Scale = 1f;
        public Color PenColour { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)(_texture.Width * Scale), (int)(_texture.Height * Scale));
            }
        }
        public string Text { get; set; }
        #endregion

        public TextDisplayer(Texture2D texture, SpriteFont font)
        {
            _texture = texture;
            _font = font;
            PenColour = Color.Black;
        }

        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            var colour = Color.White;

            spriteBatch.Draw(_texture, Rectangle, colour);

            if (!string.IsNullOrEmpty(Text))
            {
                try
                {
                    var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                    var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                    spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour);
                }
                catch (Exception e)
                {

                }
            }
        }

        public override void Update(GameTime gameTime)
        {

        }

        public void ChangeText(string newText)
        {
            Text = newText;
        }

        public Vector2 stringSize()
        {
            return new Vector2(_font.MeasureString(Text).X, _font.MeasureString(Text).Y);
        }
    }
}
