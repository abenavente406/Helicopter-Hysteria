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
    
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static int GAME_WIDTH = 1280;
        public static  int GAME_HEIGHT = 720;
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        GameStateManager manager;
        InputHandler input;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferHeight = GAME_HEIGHT;
            graphics.PreferredBackBufferWidth = GAME_WIDTH;
            graphics.ApplyChanges();
        }


        protected override void Initialize()
        {

            input = new InputHandler(this);
            Components.Add(input);
            manager = new GameStateManager(this);
            Components.Add(manager);
            manager.ChangeState(new TitleState(this, manager));

            base.Initialize();
        }


        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);

        }
        protected override void UnloadContent()
        {

        }
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
