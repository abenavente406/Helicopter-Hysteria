using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameHelperLibrary.Shapes;
using GameHelperLibrary;
using Microsoft.Xna.Framework.Graphics;

namespace Helicopter_Hysteria.Entities
{
    public class Laser : Entity
    {
        Player owner;
        bool active = false;
        float angle = 0;

        public bool IsActive
        {
            get { return active; }
            set { active = value; }
        }

        public Laser(Player owner, Vector2 pos)
            : base(pos)
        {
            this.owner = owner;
            angle = owner.Angle;
        }

        protected override void SetTexture()
        {
            var tex = new DrawableRectangle(Game1.graphics.GraphicsDevice, new Vector2(1280, 10), Color.White, true).Texture;
            sprite = new Image(tex);
            width = tex.Width;
            height = tex.Height;
        }

        public override void Update(GameTime gameTime)
        {
            this.pos = owner.Position - new Vector2(width, 0);
        }

        public override void Draw(SpriteBatch batch, GameTime gametime)
        {
            if (active)
                batch.Draw(sprite.ImageTexture, Position, null, Color.Red, angle, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
        }
    }
}
