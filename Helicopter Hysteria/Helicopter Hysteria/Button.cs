using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameHelperLibrary;
using GameHelperLibrary.Shapes;

namespace Helicopter_Hysteria
{
    public class Button
    {
        public event EventHandler OnClick;
        public event EventHandler OnMouseEnter;
        public event EventHandler OnMouseLeave;

        private string name;
        private string text = "";
        private SpriteFont font;
        private Texture2D overlay;
        private Texture2D border;
        private Texture2D background;
        private float overlayOpacity = 0.0f;
        private Point pos;
        private int width = 10;
        private int height = 10;
        private bool flipped = false;
        private bool autoSize = true;
        private int marginLeft = 4;
        private int marginRight = 4;
        private int marginTop = 4;
        private int marginBottom = 4;

        public bool AutoSize
        {
            get { return autoSize; }
            set { autoSize = value; }
        }

        public Rectangle Bounds
        {
            get { return new Rectangle(pos.X, pos.Y, width, height); }
        }

        public Point Position
        {
            get { return pos; }
            set { pos = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String Text
        {
            get { return text; }
            set
            {
                text = value;

                if (autoSize)
                {
                    width = (int)font.MeasureString(text).X + marginLeft + marginRight;
                    height = (int)font.MeasureString(text).Y + marginTop + marginBottom;
                }
            }
        }

        public Texture2D BackgroundImage
        {
            get { return background; }
            set { background = value; }
        }

        public bool FlipImage
        {
            get { return flipped; }
            set { flipped = value; }
        }

        public Button(SpriteFont font)
        {
            this.font = font;

            overlay = new DrawableRectangle(Game1.graphics.GraphicsDevice,
                new Vector2(width, height), Color.White, true).Texture;
            border = new DrawableRectangle(Game1.graphics.GraphicsDevice,
                new Vector2(width, height), Color.Black, false).Texture;

            OnMouseEnter += HighlightButton;
            OnMouseLeave += UnHighlighButton;
            OnClick += (delegate(object sender, EventArgs e)
            {
                System.Diagnostics.Debug.WriteLine("OnClick does not have a function associated with it.");
            });
        }

        public void Update(GameTime gameTime)
        {
            if (Bounds.Contains(InputHandler.MousePos))
            {
                OnMouseEnter(this, null);
                if (InputHandler.MouseButtonPressed(MouseButton.LeftButton))
                    OnClick(this, null);
            }
            else
            {
                OnMouseLeave(this, null);
            }
        }

        public void Draw(SpriteBatch batch, GameTime gameTime)
        {
            SpriteEffects flip = flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            if (background != null)
                batch.Draw(background, Bounds, null, Color.White, 0f, Vector2.Zero, flip, 1f);
            batch.Draw(overlay, Bounds, Color.White * overlayOpacity);

            batch.DrawString(font, text, new Vector2(pos.X + marginLeft, pos.Y + marginTop), Color.Black);
        }

        private void HighlightButton(object sender, EventArgs e)
        {
            overlayOpacity = 0.3f;
        }

        private void UnHighlighButton(object sender, EventArgs e)
        {
            overlayOpacity = 0.0f;
        }
    }
}