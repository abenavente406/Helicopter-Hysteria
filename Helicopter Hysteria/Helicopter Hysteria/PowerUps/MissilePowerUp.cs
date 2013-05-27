using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Helicopter_Hysteria.Weapons;

namespace Helicopter_Hysteria
{
    class MissilePowerUp : PowerUp
    {
        public MissilePowerUp(Vector2 pos, Texture2D texture)
            : base(pos, texture, 60, 55) { }

        public override void StartEffect(Entities.Player sender, EventArgs e)
        {
            sender.EquippedWeapon = new MissileLauncher(sender, sender.ShootKey);
        }

        public override void EndEffect(Entities.Player sender, EventArgs e)
        {
            sender.EquippedWeapon = new Pistol(sender, sender.ShootKey);
        }
    }
}
