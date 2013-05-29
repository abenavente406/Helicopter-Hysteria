using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helicopter_Hysteria.Entities;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Helicopter_Hysteria.Weapons
{
    public class MissileLauncher : Weapon
    {
        public MissileLauncher(Player owner, Keys shootKey)
            : base(owner, shootKey, 70f, 500F)
        {
        }

        protected override void SetBulletTexture()
        {
            bulletTex = Game1.content.Load<Texture2D>("missile");
        }

        protected override void OnFire(Player sender, EventArgs e)
        {
            SoundManager.MissileLaunchInstance.Play();
            Shoot();
        }
    }
}
