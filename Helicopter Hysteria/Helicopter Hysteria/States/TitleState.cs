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
        Button testChangeStateButton;
        Button testQuitButton;
        Texture2D titleScreen;
        public Rectangle titleRectangle;
        SpriteFont skyFall;
        public TitleState(Game game, GameStateManager manager)
            : base(game, manager)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            var content = gameRef.Content;
            titleScreen = content.Load<Texture2D>("titleScreen copy");
            titleRectangle = new Rectangle(0, 0, Game1.GAME_WIDTH, Game1.GAME_HEIGHT);
            /****************************/
            /** HOW TO CREATE A BUTTON **/
            /****************************/
            
            /*****************************/
            /********** Step 1 ***********/ 
            /*** Initialize the button ***/
            /*****************************/
            // Parameter 1: Point that represents the location of the button (new Point(10, 0))
            // Parameter 2: the width of the button (150)
            // Parameter 3: the height of the button (50)
            // Parameter 4: the state that contains the button... (this)
            testChangeStateButton = new Button(new Point(542, 292), 240, 70, this);

            /*****************************/
            /*********** Step 2 **********/ 
            /****** Name the button ******/
            /*****************************/
            testChangeStateButton.Name = "btnTest";

            /******************************/
            /*********** Step 3 ***********/
            /**** Set the what happens ****/
            /******************************/
            // Make a function that happens when the button is clicked
            // This is the syntax.  HappensWhenTheButtonIsClicked is actually 
            // a function, but when assigning events, you don't include parenthesis
            testChangeStateButton.OnClick += HappensWhenTheButtonIsClicked;

            testQuitButton = new Button(new Point(542, 390), 240, 70, this);
            testQuitButton.Name = "btnQuit";
            testQuitButton.OnClick += HappensWhenTheButtonIsClicked;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Insert update code here
            testChangeStateButton.Update(gameTime);
            testQuitButton.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = gameRef.spriteBatch;
            var content = gameRef.Content;
            skyFall = content.Load<SpriteFont>("skyFall");
            spriteBatch.Begin();
            {
               spriteBatch.Draw(titleScreen, titleRectangle, Color.White);
               spriteBatch.DrawString(skyFall, "Start", new Vector2(550, 300), Color.Black);
               spriteBatch.DrawString(skyFall, "Exit", new Vector2(590, 400), Color.Black);
                testChangeStateButton.Draw(spriteBatch, gameTime);
                testQuitButton.Draw(spriteBatch, gameTime);

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
            if (btn.Name == "btnTest")
                SwitchState(new GameplayState(gameRef, StateManager));
            else if (btn.Name == "btnQuit")
                gameRef.Exit();
        }
    }
}
