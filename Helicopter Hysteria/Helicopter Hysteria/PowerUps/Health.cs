using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GameHelperLibrary;

namespace Helicopter_Hysteria
{
    public class Health : PowerUp
    {

        public Health(Vector2 pos, Texture2D image)
            : base(pos, image, 60, 55)
        {
        }

        public override void StartEffect(Entities.Player sender, EventArgs e)
        {
            sender.Health += 100;

        }
        public override void EndEffect(Entities.Player sender, EventArgs e)
        {
            return;
        }
    }
}
