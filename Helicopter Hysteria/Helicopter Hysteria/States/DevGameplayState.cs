using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameHelperLibrary;
using Microsoft.Xna.Framework.Input;
using Helicopter_Hysteria.Weapons;

namespace Helicopter_Hysteria.States
{
    public class DevGameplayState : GameplayState
    {
        public DevGameplayState(Game game, GameStateManager manager)
            : base(game, manager) { }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputHandler.KeyPressed(Keys.NumPad1)) Players[0].EquippedWeapon = new Pistol(Players[0], Players[0].ShootKey);
            if (InputHandler.KeyPressed(Keys.NumPad2)) Players[0].EquippedWeapon = new GatlingGun(Players[0], Players[0].ShootKey);
            if (InputHandler.KeyPressed(Keys.NumPad3)) Players[0].EquippedWeapon = new Shotgun(Players[0], Players[0].ShootKey);
            if (InputHandler.KeyPressed(Keys.NumPad4)) Players[0].EquippedWeapon = new MissileLauncher(Players[0], Players[0].ShootKey);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
