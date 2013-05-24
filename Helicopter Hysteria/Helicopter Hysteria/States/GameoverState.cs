using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameHelperLibrary;
using GameHelperLibrary.Shapes;

namespace Helicopter_Hysteria.States
{
    public class GameoverState : TitleState
    {
        private Texture2D overlay;
        private float opacity = 0;
        private float onTime = 0f;
        private float maxTime = 5000f;
        private SpriteFont font;

        public GameoverState(Game game, GameStateManager manager)
            : base(game, manager) { }

        protected override void LoadContent()
        {
            base.LoadContent();

            overlay = new DrawableRectangle(gameRef.GraphicsDevice, new Vector2(Game1.GAME_WIDTH, Game1.GAME_HEIGHT), Color.White, true).Texture;
            font = gameRef.Content.Load<SpriteFont>("skyFall");
        }

        public override void Update(GameTime gameTime)
        {
            if (onTime < maxTime)
                onTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            else
                StateManager.ChangeState(new TitleState(gameRef, StateManager));

            if (opacity < 1.0)
            {
                opacity += .005f;
            }
            else
            {
                opacity = 1.0f;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            gameRef.spriteBatch.Begin();
            gameRef.spriteBatch.Draw(overlay, Vector2.Zero, Color.Black * opacity);
            gameRef.spriteBatch.DrawString(font, "G A M E    O V E R", new Vector2(Game1.GAME_WIDTH / 2 - font.MeasureString(
                "G A M E    O V E R").X / 2, Game1.GAME_HEIGHT / 2), Color.White);
            gameRef.spriteBatch.End();
        }
    }
}
