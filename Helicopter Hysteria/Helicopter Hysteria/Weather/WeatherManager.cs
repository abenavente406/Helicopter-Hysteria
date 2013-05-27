using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameHelperLibrary.Shapes;
using Helicopter_Hysteria.Entities;
using GameHelperLibrary;
using Helicopter_Hysteria.States;

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
        static Game1 gameRef;

        private static Texture2D rainTex;
        private static Weather weather = Weather.NORMAL;
        private static Vector2 windVel;
        private static Color bgColor;
        static Random rand = new Random();

        static float deltaTime = 0f;
        static float secondsToStrike = 0f;
        
        static List<RainDrop> rain = new List<RainDrop>();

        private static List<ILightning> bolts = new List<ILightning>();

        public static Vector2 WindForce
        {
            get { return windVel; }
        }

        public static Vector2 WindVelocity
        {
            get { return windVel; }
            set { windVel = value; }
        }

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

        public static Weather WeatherType
        {
            get { return weather; }
        }

        public static List<ILightning> Bolts
        {
            get { return bolts; }
        }

        public static void Initialize(Game game)
        {
            gameRef = (Game1)game;
            windVel = new Vector2(-.3f, 0);

            rainTex = new DrawableRectangle(gameRef.GraphicsDevice, new Vector2(1, 6), Color.White, true).Texture;
            secondsToStrike = rand.Next(2000, 20000);
        }

        public static void Update(GameTime gameTime)
        {
            switch (weather)
            {
                case Weather.STORM:
                    {
                        deltaTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                        for (int i = 0; i < 5; i++)
                            rain.Add(new RainDrop(new Vector2(rand.Next(Game1.GAME_WIDTH) * 1.2f, -4), 0, rainTex));

                        for (int i = rain.Count - 1; i >= 0; i--)
                        {
                            rain[i].Update(gameTime);
                            if (rain[i].DestroyMe) rain.RemoveAt(i);
                            ApplyWind(rain[i]);
                        }

                        if (deltaTime >= secondsToStrike)
                        {
                            var start = new Vector2(rand.Next(Game1.GAME_WIDTH), 0);
                            secondsToStrike = rand.Next(7000, 25000);
                            if (rand.NextDouble() < .4)
                            {
                                Player p = GetHighestPlayer(start);
                                var end = p.Position;
                                bolts.Add(new LightningBolt(start, end));
                                p.Damage(250);
                                EffectManager.AddExplosion(end, Vector2.Zero, 15, 20, 3, 5, 30f, 60, Color.White, Color.LightBlue * 0f);
                            }
                            else
                            {
                                var end = new Vector2(rand.Next(Game1.GAME_WIDTH), rand.Next(Game1.GAME_HEIGHT) / 2);
                                bolts.Add(new BranchLightning(start, end));
                            }

                            deltaTime = 0;
                        }

                        bolts.ForEach(b => b.Update());
                        break;
                    }
                case Weather.RAIN:
                    {
                        for (int i = 0; i < 5; i++)
                            rain.Add(new RainDrop(new Vector2(rand.Next(Game1.GAME_WIDTH) * 1.2f, -4), 0, rainTex));

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

        public static void ApplyWind(Entity entity)
        {
            entity.Position += WindForce;
        }

        public static Player GetHighestPlayer(Vector2 start)
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

        public static void ChangeWeather(Weather type)
        {
            weather = type;
        }
    }

    class RainDrop : Entity
    {
        float angle;
        bool destroyMe;
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
