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
        private Texture2D overlay;
        private Texture2D border;
        private Texture2D background;
        private float overlayOpacity = 0.0f;
        private Point pos;
        private int width;
        private int height;
        private bool flipped = false;
        private GameState owner;

        public Rectangle Bounds
        {
            get { return new Rectangle(pos.X, pos.Y, width, height); }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public Texture2D Background
        {
            get { return background; }
            set { background = value; }
        }

        public bool Flipped
        {
            get { return flipped; }
            set { flipped = value; }
        }

        public Button(Point pos, int width, int height, GameState owner)
        {
            this.pos = pos;
            this.width = width;
            this.height = height;
            this.owner = owner;

            overlay = new DrawableRectangle(owner.GraphicsDevice,
                new Vector2(width, height), Color.White, true).Texture;
            border = new DrawableRectangle(owner.GraphicsDevice,
                new Vector2(width, height), Color.Black, false).Texture;

            OnMouseEnter += HighlightButton;
            OnMouseLeave += UnHighlighButton;
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
        }

        public void HighlightButton(object sender, EventArgs e)
        {
            overlayOpacity = 0.3f;
        }

        public void UnHighlighButton(object sender, EventArgs e)
        {
            overlayOpacity = 0.0f;
        }
    }
}