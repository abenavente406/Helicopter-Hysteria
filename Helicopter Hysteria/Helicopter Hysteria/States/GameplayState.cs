using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameHelperLibrary;
using Helicopter_Hysteria.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace Helicopter_Hysteria.States
{
    public class GameplayState : BaseGameState
    {
        private static List<Player> players = new List<Player>();
        private static Texture2D backgroundImage;

        public static List<Player> Players
        {
            get { return players; }
        }

        public static Texture2D BackgroundImage
        {
            get { return backgroundImage; }
            set { backgroundImage = value; }
        }

        public GameplayState(Game game, GameStateManager manager)
            : base(game, manager)
        {
            
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            players.Add(new Player(PlayerIndex.One));
            players.Add(new Player(PlayerIndex.Two));

            SpriteSheet explosionSs = new SpriteSheet(content.Load<Texture2D>("explosion"), 32, 32, gameRef.GraphicsDevice);
            Texture2D[] imgs = new Texture2D[] { 
                explosionSs.GetSubImage(0, 0),
                explosionSs.GetSubImage(1, 0),
                explosionSs.GetSubImage(2, 0)
            };
            var explode = new Animation(imgs);
            EffectManager.Initialize(content.Load<Texture2D>("particle"), explode);
        }

        public override void Update(GameTime gameTime)
       {
            base.Update(gameTime);

            players.ForEach((p) =>
            {
                p.Update(gameTime);

                if (p.Dead) StateManager.ChangeState(new GameoverState(gameRef, StateManager));
            });

            EffectManager.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var batch = gameRef.spriteBatch;

            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
                DepthStencilState.Default, RasterizerState.CullNone);
            {
                batch.Draw(backgroundImage, new Rectangle(0, 0, Game1.GAME_WIDTH, Game1.GAME_HEIGHT), Color.White);
                players.ForEach((p) =>
                {
                    p.Draw(batch, gameTime);
                });
                EffectManager.Draw(batch, gameTime);
                FadeOutRect.Draw(batch, Vector2.Zero, FadeOutColor);
            }
            batch.End();
        }
    }
}
