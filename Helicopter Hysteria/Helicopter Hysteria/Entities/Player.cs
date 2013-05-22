using System;
using GameHelperLibrary;
using Helicopter_Hysteria.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Helicopter_Hysteria.Entities
{
    public class Player : Entity
    {
        private Weapon equippedWeapon;
        private Color tint = Color.White;
        private float speed = 5.0f;
        private PlayerIndex playerIndex;
        private Keys shootingKey;
        private Keys[] movingKeys;  // Index: 0 - up, 1 - down, 2 - left, 3 - right

        public override Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = Vector2.Clamp(value, 
                    new Vector2(0, 0),
                    new Vector2(Game1.GAME_WIDTH - width, Game1.GAME_HEIGHT - height)); 
            }
        }

        public bool Flipped
        {
            get { return flipped; }
        }

        public Player(PlayerIndex playerIndex)
            : base(Vector2.Zero)
        {
            this.playerIndex = playerIndex;

            if (playerIndex == PlayerIndex.One)
            {
                Position = new Vector2(0, Game1.GAME_HEIGHT / 2 - height / 2);
                shootingKey = Keys.Tab;
                movingKeys = new Keys[4] { Keys.W, Keys.S, Keys.A, Keys.D };
                tint = Color.LightBlue;
                sprite.Tint = tint;
                dir = 1;
            }
            else if (playerIndex == PlayerIndex.Two)
            {
                Position = new Vector2(Game1.GAME_WIDTH - width, Game1.GAME_HEIGHT / 2 - height / 2);
                shootingKey = Keys.RightControl;
                movingKeys = new Keys[4] { Keys.Up, Keys.Down, Keys.Left, Keys.Right };
                tint = Color.LightSalmon;
                sprite.Tint = tint;
                flipped = true;
                dir = -1;
            }
            else
            {
                throw new Exception("Unsupported PlayerIndex - please only use indeces one and two");
            }

            equippedWeapon = new GatlingGun(this, shootingKey);
        }

        protected override void SetTexture()
        {
            sprite = new Image(Game1.content.Load<Texture2D>("helicopter2"));
            width = sprite.Width;
            height = sprite.Height;
        }

        public override void Update(GameTime gameTime)
        {
            HandleInput();
            equippedWeapon.Update(gameTime);
        }

        private void HandleInput()
        {
            var newPos = Vector2.Zero;

            if (InputHandler.ButtonDown(Buttons.LeftThumbstickUp, playerIndex)) newPos.Y -= speed;
            if (InputHandler.ButtonDown(Buttons.LeftThumbstickDown, playerIndex)) newPos.Y += speed;
            if (InputHandler.ButtonDown(Buttons.LeftThumbstickLeft, playerIndex)) newPos.X -= speed;
            if (InputHandler.ButtonDown(Buttons.LeftThumbstickRight, playerIndex)) newPos.X += speed;

            if (InputHandler.KeyDown(movingKeys[0])) newPos.Y -= speed;
            if (InputHandler.KeyDown(movingKeys[1])) newPos.Y += speed;
            if (InputHandler.KeyDown(movingKeys[2])) newPos.X -= speed;
            if (InputHandler.KeyDown(movingKeys[3])) newPos.X += speed;

            Position += newPos;
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            equippedWeapon.Draw(batch, gameTime);
            base.Draw(batch, gameTime);
        }
    }
}