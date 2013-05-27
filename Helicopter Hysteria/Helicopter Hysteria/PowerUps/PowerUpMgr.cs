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
using Helicopter_Hysteria.States;

namespace Helicopter_Hysteria
{
    public class PowerUpMgr
    {

        public Texture2D speedPowerUpImage,
            heatSeekingPowerUpImage,
            healthPowerUpImage,
            forceFieldPowerUpImage,
            deathRainPowerUpImage,
            missilewaeponImage;
        private Random rand = new Random();
        float seconds, delttime;
        List<PowerUp> powerUps = new List<PowerUp>();

        public PowerUpMgr(Game game)
        {
            var content = ((Game1)game).Content;
            seconds = (float)rand.Next(8000, 13000);

            speedPowerUpImage = content.Load<Texture2D>("Power Ups//speedPowerup");
            //heatSeekingPowerUpImage = content.Load<Texture2D>("Power Ups//burstPowerUp");
            healthPowerUpImage = content.Load<Texture2D>("Power Ups//healthPowerup");
            missilewaeponImage = content.Load<Texture2D>("Power Ups//missilesPowerup");
            //forceFieldPowerUpImage = content.Load<Texture2D>("Power Ups//godPowerUp");
            //deathRainPowerUpImage = content.Load<Texture2D>("Power Ups//rapidFirePowerUp");

        }
        public void addPowerUp(PowerUp p)
        {
            powerUps.Add(p);

        }
        public void Update(GameTime gametime)
        {
            delttime += (float)gametime.ElapsedGameTime.TotalMilliseconds;
            if (delttime > seconds)
            {
                seconds = rand.Next(5000, 10000);
                delttime = 0f;
                addPowerUp(generateRandom());
            }

            powerUps.ForEach((p) =>
          {
              p.Update(gametime);
              if (p.destroyMe) { powerUps.Remove(p); }
          });
        }

        public void Draw(SpriteBatch batch)
        {
            powerUps.ForEach((p) =>
                {
                    p.Draw(batch);

                });
        }

        public Vector2 generateRandomPos()
        {
            return new Vector2(rand.Next(30,1100), rand.Next(30,680));
        }

        public PowerUp generateRandom()
        {
            var num = rand.NextDouble();

            if (num > .1 && num < .3)
                return new Speed(generateRandomPos(), speedPowerUpImage);
            else if (num > .3 && num < .6)
                return new Health(generateRandomPos(), healthPowerUpImage);

            else return new MissilePowerUp(generateRandomPos(), missilewaeponImage); 
        }


    }
}
