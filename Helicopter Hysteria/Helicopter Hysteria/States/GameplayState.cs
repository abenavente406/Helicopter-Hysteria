using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameHelperLibrary;
using Helicopter_Hysteria.Entities;

namespace Helicopter_Hysteria.States
{
    public class GameplayState : BaseGameState
    {
        static List<Player> players = new List<Player>();

        public static List<Player> Players
        {
            get { return players; }
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
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            players.ForEach((p) =>
            {
                p.Update(gameTime);
            });
        }

        public override void Draw(GameTime gameTime)
        {
            var batch = gameRef.spriteBatch;

            batch.Begin();
            players.ForEach((p) =>
            {
                p.Draw(batch, gameTime);
            });
            batch.End();
        }
    }
}
