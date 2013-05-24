using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameHelperLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Helicopter_Hysteria.States
{
    public abstract partial class BaseGameState : GameState
    {
        protected Game1 gameRef;
        protected ContentManager content;
        protected SpriteFont defaultFont;

        public BaseGameState(Game game, GameStateManager manager)
            : base(game, manager)
        {
            gameRef = (Game1)game;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            content = gameRef.Content;

            defaultFont = content.Load<SpriteFont>("defaultFont");
        }

        protected void SwitchState(GameState targetState)
        {
            StateManager.TargetState = targetState;
            IsExiting = true;
        }
    }
}
