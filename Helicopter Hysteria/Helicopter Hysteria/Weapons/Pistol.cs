using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helicopter_Hysteria.Entities;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Helicopter_Hysteria.Weapons
{
    public class Pistol : Weapon
    {
        public Pistol(Player owner, Keys shootKey)
            : base(owner, shootKey, 10f, 350f)
        {
        
        }

        protected override void SetBulletTexture()
        {
            bulletTex = Game1.content.Load<Texture2D>("bullet");
        }

        protected override void OnFire(Player sender, EventArgs e)
        {
            Shoot();
        }
    }
}
