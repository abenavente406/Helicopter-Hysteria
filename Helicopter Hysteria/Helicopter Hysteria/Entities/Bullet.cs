using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameHelperLibrary;

namespace Helicopter_Hysteria.Entities
{
    public class Bullet : Entity
    {
        private Texture2D texture;
        private Player owner;
        private float speed = 7f;

        public bool DestroyMe { get; set; }

        public Bullet(Player owner, Texture2D texture)
            : base(owner.Position)
        {
            this.texture = texture;
            this.owner = owner;
            pos += new Vector2(3, owner.Height - owner.Height / 5);

            dir = owner.Direction;

            sprite = new Image(texture);
            width = sprite.Width;
            height = sprite.Height;

            flipped = owner.Flipped;
        }

        protected override void SetTexture()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            pos.X += speed * dir;

            if (pos.X < -width  || pos.X > Game1.GAME_WIDTH ||
                pos.Y < -height || pos.Y > Game1.GAME_HEIGHT)
                DestroyMe = true;
        }
    }
}
