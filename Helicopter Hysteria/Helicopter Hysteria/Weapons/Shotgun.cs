using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helicopter_Hysteria.Entities;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Helicopter_Hysteria.Weapons
{
    public class Shotgun : Weapon
    {
        public Shotgun(Player owner, Keys shootKey)
            : base(owner, shootKey, 10f, 50f) { }

        protected override void SetBulletTexture()
        {
            bulletTex = Game1.content.Load<Texture2D>("bullet");
        }

        protected override void OnFire(Player sender, EventArgs e)
        {
            if (!fired)
                Shoot();
        }

        public override void Shoot()
        {
            bullets.Add(new Bullet(owner, MathHelper.ToRadians(13) + owner.Angle, bulletTex));
            bullets.Add(new Bullet(owner, bulletTex));
            bullets.Add(new Bullet(owner, MathHelper.ToRadians(-13) + owner.Angle, bulletTex));
        }
    }
}
