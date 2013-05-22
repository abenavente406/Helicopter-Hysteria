using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameHelperLibrary;

namespace Helicopter_Hysteria.Entities
{
    public abstract class Entity
    {
        protected int dir = -1; // -1: left, 1: right
        protected Image sprite;
        protected Vector2 pos;
        protected int width;
        protected int height;
        protected bool flipped = false;

        public virtual Vector2 Position
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

        public int Direction
        {
            get { return dir; }
        }

        public Image Sprite
        {
            get { return sprite; }
        }

        public Rectangle Bounds
        {
            get { return new Rectangle((int)pos.X, (int)pos.Y, width, height); }
        }

        public Entity(Vector2 pos)
        {
            this.pos = pos;
            SetTexture();
        }

        protected abstract void SetTexture();

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch batch, GameTime gametime)
        {
            sprite.Draw(batch, pos, flipped);
        }
    }
}
