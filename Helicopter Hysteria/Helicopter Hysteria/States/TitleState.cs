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
        Button playButton1;
        Button playButton2;
        Button optionsButton;
        Button quitButton;
        Texture2D titleScreen;
        public Rectangle titleRectangle;
        SpriteFont skyFall;

        List<Button> buttons = new List<Button>();

        public TitleState(Game game, GameStateManager manager)
            : base(game, manager)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            var content = gameRef.Content;

            skyFall = content.Load<SpriteFont>("skyFall");
            titleScreen = content.Load<Texture2D>("titleScreen copy");
            titleRectangle = new Rectangle(0, 0, Game1.GAME_WIDTH, Game1.GAME_HEIGHT);

            playButton1 = new Button(skyFall);
            playButton1.Name = "btnPlay1";
            playButton1.Text = "Single Player";

            playButton2 = new Button(skyFall);
            playButton2.Name = "btnPlay2";
            playButton2.Text = "Multiplayer";

            optionsButton = new Button(skyFall);
            optionsButton.Name = "btnOptions";
            optionsButton.Text = "Settings";

            quitButton = new Button(skyFall);
            quitButton.Name = "btnQuit";
            quitButton.Text = "Quit";

            buttons.Add(playButton1);
            buttons.Add(playButton2);
            buttons.Add(optionsButton);
            buttons.Add(quitButton);

            Point position = new Point(Game1.GAME_WIDTH / 2, (int)(Game1.GAME_HEIGHT / 2.8));

            foreach (Button b in buttons)
            {
                Point offset = new Point(b.Width / 2, 0);
                b.Position = new Point(position.X - offset.X, position.Y);
                position.Y += b.Height + 30;
                b.OnClick += HappensWhenTheButtonIsClicked;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            buttons.ForEach(b => b.Update(gameTime));
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = gameRef.spriteBatch;
            spriteBatch.Begin();
            {
                spriteBatch.Draw(titleScreen, titleRectangle, Color.White);
                buttons.ForEach(b => b.Draw(spriteBatch, gameTime));                
                FadeOutRect.Draw(spriteBatch, Vector2.Zero, FadeOutColor);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// This method is called when a button is clicked... As long as you assign it
        /// after the += operator
        /// </summary>
        /// <param name="sender">The button that is clicked</param>
        /// <param name="e">Leave null for now</param>
        private void HappensWhenTheButtonIsClicked(object sender, EventArgs e)
        {
           
            var btn = (Button)sender;
            if (btn.Name == "btnPlay2")
                SwitchState(new IntroState(gameRef, StateManager));
            else if (btn.Name == "btnPlay1")
                SwitchState(new GameplayState(gameRef, StateManager, true));
            else if (btn.Name == "btnQuit")
                gameRef.Exit();
            else if (btn.Name == "btnOptions")
                ; // This is not a mistake
        }
    }
}
