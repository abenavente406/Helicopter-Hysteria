﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameHelperLibrary;
using Helicopter_Hysteria.Entities;
using Microsoft.Xna.Framework.Graphics;
using GameHelperLibrary.Shapes;
using Helicopter_Hysteria.Weather;
using Microsoft.Xna.Framework.Media;

namespace Helicopter_Hysteria.States
{
    public class GameplayState : BaseGameState
    {
        private bool singlePlayer = false;
        private static List<Player> players = new List<Player>();
        private static Texture2D backgroundImage;
        private static PowerUpMgr powerUpMgr;
        private Texture2D backgroundoverlay;
        private int elapsed = 0;
        private int secondsToWeatherChange = 0;
        private Random rand = new Random();

        public static List<Player> Players
        {
            get { return players; }
        }

        public static Texture2D BackgroundImage
        {
            get { return backgroundImage; }
            set { backgroundImage = value; }
        }

        public GameplayState(Game game, GameStateManager manager, bool singlePlayer)
            : base(game, manager)
        {
            this.singlePlayer = singlePlayer;
        }

        public override void Initialize()
        {
            base.Initialize();

            players.Clear();
            players.Add(new Player(PlayerIndex.One));

            if (singlePlayer)
                players.Add(new ComputerPlayer());
            else
                players.Add(new Player(PlayerIndex.Two));

            WeatherManager.Initialize(gameRef);
            WeatherManager.ChangeWeather(Weather.Weather.NORMAL);

            powerUpMgr = new PowerUpMgr(gameRef);

            secondsToWeatherChange = rand.Next(40000, 80000);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Art.Load(gameRef.Content);

            backgroundoverlay = new DrawableRectangle(GraphicsDevice, new Vector2(Game1.GAME_WIDTH, Game1.GAME_HEIGHT), Color.White, true).Texture;

            SpriteSheet explosionSs = new SpriteSheet(content.Load<Texture2D>("explosion"), 32, 32, gameRef.GraphicsDevice);
            Texture2D[] imgs = new Texture2D[] { 
                explosionSs.GetSubImage(0, 0),
                explosionSs.GetSubImage(1, 0),
                explosionSs.GetSubImage(2, 0)
            };
            var explode = new Animation(imgs);
            EffectManager.Initialize(content.Load<Texture2D>("particle"), explode);

            if (singlePlayer)
                backgroundImage = content.Load<Texture2D>("Screen Images\\apocalypselondon");
        }

        public override void Update(GameTime gameTime)
       {
            base.Update(gameTime);

            if (MediaPlayer.Volume > .1f) MediaPlayer.Volume -= .001f;

            elapsed += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsed >= secondsToWeatherChange)
            {
                var randNum = rand.Next(4);
                if (randNum == 4) WeatherManager.ChangeWeather(Weather.Weather.STORM);
                else if (randNum == 3) WeatherManager.ChangeWeather(Weather.Weather.RAIN);
                else if (randNum == 2) WeatherManager.ChangeWeather(Weather.Weather.SUNNY);
                else WeatherManager.ChangeWeather(Weather.Weather.NORMAL);

                elapsed = 0;
            }

            players.ForEach((p) =>
            {
                p.Update(gameTime);
                if (WeatherManager.WeatherType == Weather.Weather.STORM) WeatherManager.ApplyWind(p);
                if (p.IsDead) StateManager.ChangeState(new GameoverState(gameRef, StateManager));
            });

            EffectManager.Update(gameTime);
            WeatherManager.Update(gameTime);
            powerUpMgr.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var batch = gameRef.spriteBatch;

            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
                DepthStencilState.Default, RasterizerState.CullNone);
            {
                batch.Draw(backgroundImage, new Rectangle(0, 0, Game1.GAME_WIDTH, Game1.GAME_HEIGHT), Color.White);
                powerUpMgr.Draw(batch);
                players.ForEach((p) =>
                {
                    p.Draw(batch, gameTime);
                });
                batch.Draw(backgroundoverlay, Vector2.Zero, WeatherManager.BgColor);
                WeatherManager.Draw(batch, gameTime);
                EffectManager.Draw(batch, gameTime);
                FadeOutRect.Draw(batch, Vector2.Zero, FadeOutColor);
            }
            batch.End();

            batch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            {
                WeatherManager.Bolts.ForEach(b => b.Draw(batch));
            }
            batch.End();
        }
    }
}