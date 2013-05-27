using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helicopter_Hysteria.Entities;
using Microsoft.Xna.Framework.Input;
using GameHelperLibrary.Shapes;
using Microsoft.Xna.Framework;
using GameHelperLibrary;

namespace Helicopter_Hysteria.Weapons
{
    public class LaserGun : Weapon
    {
        Laser laser;

        public Laser Laser
        {
            get { return laser; }
        }

        public LaserGun(Player owner, Keys shootKey)
            : base(owner, shootKey, 5f, 10f)
        {
            laser = new Laser(owner, owner.Position);
        }

        protected override void SetBulletTexture()
        {
            var tex = new DrawableRectangle(Game1.graphics.GraphicsDevice, new Vector2(1366, 5), Color.Red, true).Texture;
            bulletTex = tex;
        }

        protected override void OnFire(Player sender, EventArgs e)
        {
            laser.IsActive = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!InputHandler.KeyDown(shootKey) && !InputHandler.ButtonDown(Buttons.RightTrigger, owner.PlayerIndex))
                laser.IsActive = false;
            laser.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch, GameTime gameTime)
        {
            laser.Draw(batch, gameTime);
        }
        public override void Shoot()
        {
            return;
        }
    }
}
