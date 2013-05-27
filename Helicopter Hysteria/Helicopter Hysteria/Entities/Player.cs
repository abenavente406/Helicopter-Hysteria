using System;
using GameHelperLibrary;
using Helicopter_Hysteria.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Helicopter_Hysteria.States;
using GameHelperLibrary.Shapes;

namespace Helicopter_Hysteria.Entities
{
    public class Player : Entity
    {
        #region Fields
        private SpriteFont font;
        private float maxHealth = 3000f;
        private float health = 3000f;
        private Weapon equippedWeapon;
        private Color tint = Color.White;
        private float maxspeed = 5.0f;
        private PlayerIndex playerIndex;
        private Keys shootingKey;
        private Keys[] movingKeys;  // Index: 0 - up, 1 - down, 2 - left, 3 - right
        private Vector2 gunPos;
        private float angle = 0.0f;
        private Texture2D lifeBg;
        private Texture2D lifeFg;
        private bool superSpeed = false;
        private bool tracking = false;
        private bool forceField = false;
        protected Vector2 velocity;
        protected Animation anim;
        #endregion

        #region Properties

        public float Health
        {
            get { return health; }
            set { health = MathHelper.Clamp(value, 0, maxHealth); }
        }

        public bool Dead
        {
            get { return Health <= 0; }
        }

        public PlayerIndex PlayerIndex
        {
            get { return playerIndex; }
        }

        public override Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = Vector2.Clamp(value, 
                    new Vector2(width / 2, height / 2),
                    new Vector2(Game1.GAME_WIDTH - width / 2, Game1.GAME_HEIGHT - height / 2)); 
            }
        }

        public override Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)Position.X - width / 2, (int)Position.Y - height / 2,
                    width, height);
            }
        }

        public Vector2 GunOffset
        {
            get { return gunPos; }
            set { gunPos = value; }
        }

        public bool Flipped
        {
            get { return flipped; }
        }

        public float Angle
        {
            get { return angle; }
        }

        public Vector2 Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = Vector2.Clamp(value, new Vector2(-maxspeed, -maxspeed), new Vector2(maxspeed, maxspeed));
            }
        }

        public Weapon EquippedWeapon
        {
            get { return equippedWeapon; }
            set { equippedWeapon = value; }
        }

        public Keys ShootKey
        {
            get { return shootingKey; }
        }

        public float PercentHealth
        {
            get { return health / maxHealth; }
        }

        public bool SuperSpeed
        {
            get { return superSpeed; }
            set { superSpeed = value; }
        }

        public bool Tracking
        {
            get { return tracking; }
            set { tracking = value; }
        }

        public bool ForceField
        {
            get { return forceField; }
            set { forceField = value; }
        }
        #endregion

        #region Initialize
        public Player(PlayerIndex playerIndex)
            : base(Vector2.Zero)
        {
            this.playerIndex = playerIndex;

            if (playerIndex == PlayerIndex.One)
            {
                Position = new Vector2(0, Game1.GAME_HEIGHT / 2 - height / 2);
                shootingKey = Keys.Tab;
                movingKeys = new Keys[4] { Keys.W, Keys.S, Keys.A, Keys.D };
                tint = IntroState.player1Color;
                sprite.Tint = tint;
                dir = 1;
                GunOffset = new Vector2(width / 2, 0);
            }
            else if (playerIndex == PlayerIndex.Two)
            {
                Position = new Vector2(Game1.GAME_WIDTH - width, Game1.GAME_HEIGHT / 2 - height / 2);
                shootingKey = Keys.RightControl;
                movingKeys = new Keys[4] { Keys.Up, Keys.Down, Keys.Left, Keys.Right };
                tint = IntroState.player2Color;
                sprite.Tint = tint;
                flipped = true;
                dir = -1;
                GunOffset = new Vector2(-width / 2, 0);
            }
            else
            {
                throw new Exception("Unsupported PlayerIndex - please only use indeces one and two");
            }
            equippedWeapon = new LaserGun(this, shootingKey);

            font = Game1.content.Load<SpriteFont>("defaultFont");

            Texture2D[] imgs = new Texture2D[] {
                Game1.content.Load<Texture2D>("helicopter1"),
                Game1.content.Load<Texture2D>("helicopter2")
            };

            anim = new Animation(imgs, 20f);

            var tex = new DrawableRectangle(Game1.graphics.GraphicsDevice, new Vector2(10, 10), Color.White, true).Texture;
            lifeBg = tex;
            lifeFg = tex;
        }

        protected override void SetTexture()
        {
            sprite = new Image(Game1.content.Load<Texture2D>("helicopter2"));
            width = sprite.Width;
            height = sprite.Height;
        }
        #endregion

        #region Update and Input
        public override void Update(GameTime gameTime)
        {
            maxspeed = !superSpeed ? 5f : 10f;

            var epsilon = .005f;

            if (!GamePad.GetState(playerIndex, GamePadDeadZone.Circular).IsConnected)
            {
                if (Math.Abs(0 - angle) > epsilon && ((!InputHandler.KeyDown(movingKeys[2]) && !InputHandler.KeyDown(movingKeys[3]))))
                {
                    if (angle < 0)
                    {
                        angle += .01f;
                    }
                    else if (angle > 0)
                    {
                        angle -= .01f;
                    }
                }
            }
            else
            {
                if (Math.Abs(0 - angle) > epsilon && ((!InputHandler.ButtonDown(Buttons.LeftThumbstickLeft, playerIndex) && 
                    !InputHandler.ButtonDown(Buttons.LeftThumbstickRight, playerIndex))))
                {
                    if (angle < 0)
                    {
                        angle += .01f;
                    }
                    else if (angle > 0)
                    {
                        angle -= .01f;
                    }
                }
            }

            if (Math.Abs(velocity.X) > epsilon && !InputHandler.KeyDown(movingKeys[2]) && !InputHandler.KeyDown(movingKeys[3]))
            {
                if (velocity.X > 0)
                {
                    velocity.X -= .1f;
                }
                else if (velocity.X < 0)
                {
                    velocity.X += .1f;
                }
            }

            HandleInput();
            equippedWeapon.Update(gameTime);

            foreach (Player p in GameplayState.Players)
            {
                if (p == this) continue;

                if (p.equippedWeapon.GetType() == typeof(LaserGun))
                {
                    var weapon = (LaserGun)p.equippedWeapon;
                    if (weapon.Laser.Bounds.Intersects(Bounds) && weapon.Laser.IsActive)
                        Damage(p.equippedWeapon.Damage);
                }
                else
                {
                    p.equippedWeapon.Bullets.ForEach((b) =>
                    {
                        if (b.Bounds.Intersects(Bounds))
                        {
                            Damage(p.equippedWeapon.Damage);
                            b.DestroyMe = true;
                        }
                    });
                }
            }
        }

        private void HandleInput()
        {
            var newVelocity = Vector2.Zero;
            
            if (InputHandler.KeyDown(movingKeys[0]) || InputHandler.ButtonDown(Buttons.LeftThumbstickUp, playerIndex))
            {
                pos.Y -= maxspeed;
            }
            if (InputHandler.KeyDown(movingKeys[1]) || InputHandler.ButtonDown(Buttons.LeftThumbstickDown, playerIndex))
            {
                pos.Y += maxspeed;
            }
            if (InputHandler.KeyDown(movingKeys[2]) || InputHandler.ButtonDown(Buttons.LeftThumbstickLeft, playerIndex))
            {
                newVelocity.X -= .5f;
                if (angle > -.08f) angle -= .005f;
            }
            if (InputHandler.KeyDown(movingKeys[3]) || InputHandler.ButtonDown(Buttons.LeftThumbstickRight, playerIndex))
            {
                newVelocity.X += .5f;
                if (angle < .08f) angle += .005f;
            }

            Velocity += newVelocity;
            Position += Velocity;
        }
        #endregion

        #region Draw
        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            equippedWeapon.Draw(batch, gameTime);
            
            // Draw the player
            SpriteEffects e = flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            anim.Draw(batch, gameTime, pos.X, pos.Y, angle, new Vector2(width / 2, height / 2), tint,1.0f, flipped);
            DrawGUI(batch);
        }

        private void DrawGUI(SpriteBatch batch)
        {
            if (playerIndex == PlayerIndex.One)
            {
                batch.DrawString(font, "Player 1: " + health.ToString(), new Vector2(10, 40), Color.White);
                batch.Draw(lifeBg, new Rectangle(10, 10, 200, 30), Color.Gray * .6f);
                batch.Draw(lifeFg, new Rectangle(10, 10, (int)(200 * PercentHealth), 30), Color.Red);
            }
            else if (playerIndex == PlayerIndex.Two)
            {
                batch.DrawString(font, "Player 2: " + health.ToString(), new Vector2(Game1.GAME_WIDTH - font.MeasureString(
                    "Player 2: " + health.ToString()).X - 10, 40), Color.White);
                batch.Draw(lifeBg, new Rectangle(1070, 10, 200, 30), Color.Gray * .6f);
                batch.Draw(lifeFg, new Rectangle(1070, 10, (int)(200 * PercentHealth), 30), Color.Red);
            }
        }
        #endregion

        #region Helper Methods
        public void Damage(float amount)
        {
            Health -= amount;
        }
        #endregion
    }
}