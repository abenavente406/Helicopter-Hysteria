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

namespace Helicopter_Hysteria.States
{
    public class TitleState : BaseGameState
    {
        public Texture2D titleScreen;
        public Rectangle titleScreenRec;
        public Vector2 origin;
       
        public TitleState(Game game, GameStateManager manager)
            : base(game, manager)
        {
            origin = new Vector2(0, 0);
            titleScreenRec = new Rectangle(0, 0, Game1.GAME_WIDTH, Game1.GAME_HEIGHT);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            var content = gameRef.Content;
            titleScreen = content.Load<Texture2D>("Titlescreen");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
           
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = gameRef.spriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(titleScreen, titleScreenRec, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
