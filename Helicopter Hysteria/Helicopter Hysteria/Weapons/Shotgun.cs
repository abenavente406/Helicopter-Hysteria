using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helicopter_Hysteria.Entities;
using Microsoft.Xna.Framework.Input;

namespace Helicopter_Hysteria.Weapons
{
    public class Shotgun : Weapon
    {
        public Shotgun(Player owner, Keys shootKey)
            : base(owner, shootKey, 10f, 50f) { }

        protected override void SetBulletTexture()
        {
            throw new NotImplementedException();
        }

        protected override void OnFire(Player sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void Shoot()
        {
            bullets.Add(new Bullet(owner, bulletTex));
            bullets.Add(new Bullet(owner, bulletTex));
            bullets.Add(new Bullet(owner, bulletTex));
        }
    }
}
