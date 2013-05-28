using System;
using System.Collections.Generic;
using GameHelperLibrary;
using GameHelperLibrary.Shapes;
using Helicopter_Hysteria.Entities;
using Helicopter_Hysteria.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Helicopter_Hysteria.Weather
{
    public enum Weather
    {
        RAIN,
        STORM,
        SUNNY,
        NORMAL
    }

    public static class WeatherManager
    {
        #region Fields
        private static Game1 gameRef;

        // Texture of the rain
        // -------------------
        private static Texture2D rainTex;

        // The state of the weather, see the enum for options
        // --------------------------------------------------
        private static Weather weather = Weather.NORMAL;

        // How fast the wind is blowing
        // ----------------------------------------------
        // TODO: Make wind velocity change, and add gusts
        // -----------------------------------------------
        private static Vector2 windVel;

        // Color overlayed onto the screen based on the weather
        // ---------------------------------------------------
        // Blue: rain, More Blue: storm, Orange: sun, White: none
        // ----------------------------------------------------
        private static Color bgColor;

        // Random number generator
        // -----------------------
        private static Random rand = new Random();

        // Amount of time passed since a lightning strike
        // ----------------------------------------------
        private static float elapsedLightningTime = 0f;

        // Time until next lightning strike
        // --------------------------------
        private static float secondsToStrike = 0f;

        // Holds all the rain particles on the screen. This
        // is pretty much a particle system.
        // ------------------------------------------------
        private static List<RainDrop> rain = new List<RainDrop>();

        // The lightning bolts on the screen
        // ---------------------------------
        private static List<ILightning> bolts = new List<ILightning>();
        #endregion

        #region Properties
        /// <summary>
        /// Gets and sets how strong the wind is
        /// </summary>
        public static Vector2 WindVelocity
        {
            get { return windVel; }
            set { windVel = value; }
        }

        /// <summary>
        /// Gets the overlay color based on the weather
        /// </summary>
        public static Color BgColor
        {
            get
            {
                switch (weather)
                {
                    case Weather.RAIN:
                        return new Color(0, 100, 200) * .3f;
                    case Weather.STORM:
                        return new Color(0, 100, 200) * .4f;
                    case Weather.SUNNY:
                        return new Color(100, 30, 0) * .3f;
                    default:
                        return Color.White * 0f;
                }
            }
        }

        /// <summary>
        /// Gets the current weather type
        /// </summary>
        public static Weather WeatherType
        {
            get { return weather; }
        }

        /// <summary>
        /// Gets the lightning bolts on the screen
        /// </summary>
        public static List<ILightning> Bolts
        {
            get { return bolts; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the weather manager. Must be called before the 
        /// class can work
        /// </summary>
        /// <param name="game"></param>
        public static void Initialize(Game game)
        {
            gameRef = (Game1)game;
            windVel = new Vector2(-.3f, 0);

            // For now, I'm setting the rain texture to small, blue rectangles
            // ---------------------------------------------------------------
            rainTex = new DrawableRectangle(gameRef.GraphicsDevice, new Vector2(1, 6), Color.White, true).Texture;

            // Lightning will strike in between 2 - 20 seconds
            // -----------------------------------------------
            secondsToStrike = rand.Next(2000, 20000);
        }
        #endregion

        #region Update and Logic
        public static void Update(GameTime gameTime)
        {
            switch (weather)
            {
                // If it's storming, there will be wind and lightning
                // --------------------------------------------------
                case Weather.STORM:
                    {
                        // Add seconds to the lightning timer
                        elapsedLightningTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                        // Add 5 rain drops per update
                        // ---------------------------
                        for (int i = 0; i < 5; i++)
                            rain.Add(new RainDrop(new Vector2(rand.Next(Game1.GAME_WIDTH) * 1.2f, -4), 0, rainTex));

                        // Update each rain drop and destroy it if need be
                        // -----------------------------------------------
                        for (int i = rain.Count - 1; i >= 0; i--)
                        {
                            rain[i].Update(gameTime);
                            if (rain[i].DestroyMe) rain.RemoveAt(i);
                            ApplyWind(rain[i]);
                        }

                        // If the lightning should strike, then strike dammit
                        // --------------------------------------------------
                        if (elapsedLightningTime >= secondsToStrike)
                        {
                            // The start location of the lightning bolt
                            var start = new Vector2(rand.Next(Game1.GAME_WIDTH), 0);

                            // A strike will occur again in 2 - 20 seconds
                            secondsToStrike = rand.Next(2000, 20000);

                            // There's a 40 percent chance that the nearest 
                            // player will get struck by the lightning bolt
                            // --------------------------------------------
                            if (rand.NextDouble() < .4)
                            {
                                // Find which player is closest to the lightning bolt.
                                // It is electricity after all.
                                Player p = GetNearestPlayer(start);

                                // The end position of the lightning bolt should be on the player
                                var end = p.Position;

                                // Add a new lightning bolt there
                                bolts.Add(new LightningBolt(start, end));

                                // Damage the player 250 points
                                p.Damage(250);

                                // Add a white and blue explosion
                                EffectManager.AddExplosion(end, Vector2.Zero, 15, 20, 3, 5, 30f, 60, Color.White, Color.LightBlue * 0f);
                            }
                            // If the lightning bolt is not destined to strike the player
                            // ----------------------------------------------------------
                            else
                            {
                                // The lightning bolt's end point will be a random point that won't hurt anyone
                                var end = new Vector2(rand.Next(Game1.GAME_WIDTH), rand.Next(Game1.GAME_HEIGHT) / 2);

                                // Add the new lightning bolt
                                bolts.Add(new LightningBolt(start, end));
                            }

                            // Reset the time since strike to be 0
                            // -----------------------------------
                            elapsedLightningTime = 0;
                        }

                        // Update each lightning bolt... Feat. Lambda Expressionz
                        // ------------------------------------------------------
                        bolts.ForEach(b => b.Update());
                        break;
                    }
                // If it's rainy, there is only rain, nothing else
                // -----------------------------------------------
                case Weather.RAIN:
                    {
                        // Add 5 rain drops per update
                        // ---------------------------
                        for (int i = 0; i < 5; i++)
                            rain.Add(new RainDrop(new Vector2(rand.Next(Game1.GAME_WIDTH) * 1.2f, -4), 0, rainTex));

                        // Update each rain drop
                        // ---------------------
                        for (int i = rain.Count - 1; i >= 0; i--)
                        {
                            rain[i].Update(gameTime);
                            if (rain[i].DestroyMe) rain.RemoveAt(i);
                        }

                        break;
                    }
                case Weather.SUNNY:
                    {
                        break;
                    }
                case Weather.NORMAL:
                    {
                        break;
                    }
            }
        }
        #endregion

        #region Draw
        public static void Draw(SpriteBatch batch, GameTime gameTime)
        {
            if (weather == Weather.RAIN || weather == Weather.STORM)
            {
                for (int i = rain.Count - 1; i >= 0; i--)
                {
                    rain[i].Draw(batch, gameTime);
                }
            }
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Add the wind's velocity to the entity's position
        /// </summary>
        /// <param name="entity"></param>
        public static void ApplyWind(Entity entity)
        {
            entity.Position += WindVelocity;
        }

        /// <summary>
        /// Gets the player nearest to the lightning bolt
        /// </summary>
        /// <param name="start">The point that starts the search</param>
        /// <returns>The nearest player.</returns>
        public static Player GetNearestPlayer(Vector2 start)
        {
            Player result = null;
            var distance = float.MaxValue;

            foreach (Player p in GameplayState.Players)
            {
                if (Vector2.Distance(start, p.Position) < distance)
                {
                    distance = Vector2.Distance(start, p.Position);
                    result = p;
                }
            }

            return result;
        }

        /// <summary>
        /// Changes the current weather
        /// </summary>
        /// <param name="type"></param>
        public static void ChangeWeather(Weather type)
        {
            weather = type;
        }
        #endregion
    }

    class RainDrop : Entity
    {
        // The angle the rain is drawn
        // ---------------------------
        float angle;

        // If the rain should be removed from the list
        // -------------------------------------------
        bool destroyMe;

        // How fast the rain is falling from the sky
        // -----------------------------------------
        Vector2 vel;

        public float Angle
        {
            get { return angle; }
        }

        public bool DestroyMe
        {
            get { return destroyMe; }
        }

        public RainDrop(Vector2 pos, float angle, Texture2D texture)
            : base(pos)
        {
            this.angle = angle;
            this.destroyMe = false;
            this.vel = new Vector2(-2, 20);

            this.angle = MathHelper.ToRadians(20);

            sprite = new Image(texture);
            width = sprite.Width;
            height = sprite.Height;
        }

        protected override void SetTexture()
        {
            return;
        }

        public override void Update(GameTime gameTime)
        {
            // All I'm doint is changing the position of the rain drop.
            // If it goes outside the screen, remove it
            // -------------------------------------------------------
            pos += new Vector2(vel.X, vel.Y);
            if (pos.Y > Game1.GAME_HEIGHT) 
                destroyMe = true;
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            batch.Draw(sprite.ImageTexture, pos, null, Color.LightBlue, angle, 
                new Vector2(sprite.ImageTexture.Width / 2, sprite.ImageTexture.Height / 2), 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
