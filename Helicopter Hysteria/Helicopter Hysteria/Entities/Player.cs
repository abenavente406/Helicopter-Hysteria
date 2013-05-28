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
    // -------------------------------------------------------------------
    // Dear Anthony,
    // I've become strangely attached to this class, please don't kill it.
    // - Anthony
    // -------------------------------------------------------------------
    public class Player : Entity
    {
        #region Fields
        private PlayerIndex playerIndex;
        private SpriteFont font;
        private Color tint = Color.White;
        private Animation playerAnim;

        private float maxHealth = 3000f;
        private float health = 3000f;

        private Vector2 velocity;
        private float maxspeed = 5.0f;
        private float angle = 0.0f;

        private Weapon equippedWeapon;
        private Vector2 gunPos;

        // Organize input keys into variables for resusability
        // ---------------------------------------------------
        private Keys shootingKey;
        // Indexes: 0 = up, 1 = down, 2 = left, 3 = right
        private Keys[] movingKeys; 

        // Textures for the life bar
        // -------------------------
        private Texture2D lifeBg;
        private Texture2D lifeFg;

        // Variables that are changed with power ups
        // -----------------------------------------
        private bool superSpeed = false;    // Speed goes up
        private bool tracking = false;      // Bullets seek other players
        private bool forceField = false;    // Force field blocks damage

        // Force field stuffs
        // ------------------
        private int forceFieldWidth = 0;
        private int forceFieldHeight = 0;
        private Texture2D forceFieldTex;
        #endregion

        #region Properties
        public float Health
        {
            get { return health; }
            set { health = MathHelper.Clamp(value, 0, maxHealth); }
        }

        /// <summary>
        ///  Gets if the player is dead
        /// </summary>
        public bool IsDead
        {
            get { return Health <= 0; }
        }

        /// <summary>
        /// Gets the working player index or Xbox control index
        /// </summary>
        public PlayerIndex PlayerIndex
        {
            get { return playerIndex; }
        }

        /// <summary>
        /// Gets the position (duh). Also clamps the 
        /// position to the x/y/width/height of the screen
        /// </summary>
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

        /// <summary>
        /// Gets the rectangular bounds
        /// </summary>
        public override Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)Position.X - width / 2, (int)Position.Y - height / 2,
                    width, height);
            }
        }

        /// <summary>
        /// Gets the amount to add to Position which shows where the gun is located
        /// </summary>
        public Vector2 GunOffset
        {
            get { return gunPos; }
            set { gunPos = value; }
        }

        /// <summary>
        /// Gets if the player's sprite should be flipped 
        /// </summary>
        public bool Flipped
        {
            get { return flipped; }
        }

        /// <summary>
        /// Gets the angle of flight
        /// </summary>
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

        /// <summary>
        /// Gets the key used to fire the players equipped weapon
        /// </summary>
        public Keys ShootKey
        {
            get { return shootingKey; }
        }

        /// <summary>
        /// Gets the percentage of health remaing (in between 0.0f - 1.0f)
        /// </summary>
        public float PercentHealth
        {
            get { return health / maxHealth; }
        }

        /// <summary>
        /// Gets and sets if the player should be moving FAST
        /// </summary>
        public bool SuperSpeed
        {
            get { return superSpeed; }
            set { superSpeed = value; }
        }

        /// <summary>
        /// Gets and sets if players bullets should seek another player
        /// </summary>
        public bool Tracking
        {
            get { return tracking; }
            set { tracking = value; }
        }

        /// <summary>
        /// Gets and sets if the player should have a force field on
        /// </summary>
        public bool ForceField
        {
            get { return forceField; }
            set { forceField = value; }
        }

        /// <summary>
        /// Gets the rectangular area guarding the player
        /// </summary>
        public Rectangle ForceFieldBounds
        {
            get
            {
                return new Rectangle((int)(Position.X - width / 2 - 30), (int)(Position.Y - height / 2 - 30),
                    forceFieldWidth, forceFieldHeight);
            }
        }
        #endregion

        #region Initialize
        public Player(PlayerIndex playerIndex)
            : base(Vector2.Zero)
        {
            this.playerIndex = playerIndex;

            // Initialize the player based on playerIndex
            // ---------------------------------------------------------------
            // I seperate the initialization of players into two if statements:
            // PlayerIndex.One and PlayerIndex.Two.  As of right now, I won't 
            // add any more (for laziness sake) and will keep initialization 
            // like this.  I'm sorry if this code offends you.
            // ---------------------------------------------------------------
            if (playerIndex == PlayerIndex.One)
            {
                // Set the position to be on the left side of the screen
                Position = new Vector2(0, Game1.GAME_HEIGHT / 2 - height / 2);

                // Set the shooting key based on playerIndex
                shootingKey = Keys.Tab;

                // Organize the moving keys
                movingKeys = new Keys[4] { Keys.W, Keys.S, Keys.A, Keys.D };

                // Tint is chosen from the IntroState
                tint = IntroState.player1Color;
                sprite.Tint = tint;

                // First player will be facing right
                dir = 1;
                flipped = false;

                GunOffset = new Vector2(width / 2, 0);
            }
            else if (playerIndex == PlayerIndex.Two)
            {
                // Set the position to be on the right side of the screen
                Position = new Vector2(Game1.GAME_WIDTH - width, Game1.GAME_HEIGHT / 2 - height / 2);

                // Set the shooting key based on playerIndex
                shootingKey = Keys.RightControl;

                // Organize the moving keys
                movingKeys = new Keys[4] { Keys.Up, Keys.Down, Keys.Left, Keys.Right };

                // Tint is chosen from the IntroState
                tint = IntroState.player2Color;
                sprite.Tint = tint;

                // Second player will be facing left
                dir = -1;
                flipped = true;

                GunOffset = new Vector2(-width / 2, 0);
            }
            else
            {
                // Remember how I said I only support PlayerIndex One and Two?
                // This is where I kindly remind you if you forget.
                // ----------------------------------------------------------
                throw new Exception("Unsupported PlayerIndex - please only use indeces one and two");
            }

            // Default weapons will be Pistols.  If you wanna change it, go ahead
            // ------------------------------------------------------------------
            equippedWeapon = new Pistol(this, shootingKey);

            // This font is used for the player information that is on screen
            // --------------------------------------------------------------
            font = Game1.content.Load<SpriteFont>("defaultFont");

            // And here I make the animation for the player. Easy Peasy.
            // ---------------------------------------------------------
            Texture2D[] imgs = new Texture2D[] {
                Game1.content.Load<Texture2D>("helicopter1"),
                Game1.content.Load<Texture2D>("helicopter2")
            };
            playerAnim = new Animation(imgs, 20f);

            // And here I have the basic rectangles used for the life bar.
            // The life bar stuff is pretty basic right now
            // ----------------------------------------------------------
            var tex = new DrawableRectangle(Game1.graphics.GraphicsDevice, new Vector2(10, 10), Color.White, true).Texture;
            lifeBg = tex;
            lifeFg = tex;

            // Set the force field size to be bigger than the player
            // -----------------------------------------------------
            forceFieldWidth = width + 30;
            forceFieldHeight = height + 30;

            // Temporary setting the force field texture to a white rectangle
            // --------------------------------------------------------------
            forceFieldTex = new DrawableRectangle(Game1.graphics.GraphicsDevice, new Vector2(forceFieldWidth, forceFieldHeight), Color.White, true).Texture;
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
            // If super speed is not enabled, max speed == 5, otherwise,
            // double that sucker!
            // --------------------------------------------------------
            maxspeed = !superSpeed ? 5f : 10f;

            // If you don't know what this is, don't touch.
            // --------------------------------------------
            var epsilon = .005f;

            // FUCK! I have repeated, ugly code. What was I thinking?
            // ------------------------------------------------
            // TODO: Remove ugly code, add nice code
            // ------------------------------------------------
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

            // If the player is not moving, slow him the down to 0
            // ---------------------------------------------------
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

            // Each weapon may have it's own update method. More OOP == better code
            // --------------------------------------------------------------------
            equippedWeapon.Update(gameTime);

            // Loop through each players' bullets (except my own) and if any hit me,
            // hurt me and remove the bullet from the field
            // --------------------------------------------------------------------
            foreach (Player p in GameplayState.Players)
            {
                // Why would I check if my bullets hit me? Well... there's probably many
                //     resons but for now, keep it my way.
                if (p == this) continue;

                // Laser guns are an exception to bullet checks... instead, they're just a 
                // giant rectangle.  The laser isn't removed if it's hitting me
                if (p.equippedWeapon.GetType() == typeof(LaserGun))
                {
                    var weapon = (LaserGun)p.equippedWeapon;
                    if (weapon.Laser.Bounds.Intersects(Bounds) && weapon.Laser.IsActive)
                        Damage(p.equippedWeapon.Damage);
                }
                else
                {
                    // Here's the bullet foreach loop, it just uses a fancy lambda expression
                    p.equippedWeapon.Bullets.ForEach((b) =>
                    {
                        // If there's no forcefield then KILL ME
                        if (!forceField)
                        {
                            if (b.Bounds.Intersects(Bounds))
                            {
                                Damage(p.equippedWeapon.Damage);
                                b.DestroyMe = true;
                            }
                        }
                        // If there is a force field then don't kill me
                        else
                        {
                            if (b.Bounds.Intersects(ForceFieldBounds))
                            {
                                b.DestroyMe = true;
                            }
                        }
                    });
                }
            }
        }

        /// <summary>
        /// Handles key inputs from the player
        /// </summary>
        private void HandleInput()
        {
            // Instead of changing pos directly, make it look nice by changing velocity
            var newVelocity = Vector2.Zero;
            
            if (InputHandler.KeyDown(movingKeys[0]) || InputHandler.ButtonDown(Buttons.LeftThumbstickUp, playerIndex))
            {
                // When changing y values, change pos directly
                pos.Y -= maxspeed;
            }
            if (InputHandler.KeyDown(movingKeys[1]) || InputHandler.ButtonDown(Buttons.LeftThumbstickDown, playerIndex))
            {
                pos.Y += maxspeed;
            }

            // By changing velocity, this allows us to add the slow down effect
            if (InputHandler.KeyDown(movingKeys[2]) || InputHandler.ButtonDown(Buttons.LeftThumbstickLeft, playerIndex))
            {
                newVelocity.X -= .5f;
                if (angle > -.08f) angle -= .005f;  // Magic numbers are bad and I should feel bad
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
            // Draw the weapon's bullets
            // -------------------------
            equippedWeapon.Draw(batch, gameTime);
            
            // Draw the player
            // ---------------
            // Check if we should flip the player without using if statements
            // --------------------------------------------------------------
            SpriteEffects e = flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            playerAnim.Draw(batch, gameTime, pos.X, pos.Y, angle, new Vector2(width / 2, height / 2), tint,1.0f, flipped);

            if (forceField)
            {
                // TODO: Draw a NICE force field texture over top the player texture
                batch.Draw(forceFieldTex, ForceFieldBounds, Color.White * .3f);
            }

            DrawGUI(batch);
        }

        /// <summary>
        /// Draw the player's information to the screen
        /// </summary>
        /// <param name="batch"></param>
        private void DrawGUI(SpriteBatch batch)
        {
            // I hope you can understand this code. If not, talk to me.
            // --------------------------------------------------------
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
        /// <summary>
        /// Hurts the player by a certain amount
        /// </summary>
        /// <param name="amount">Amount of damage</param>
        public void Damage(float amount)
        {
            Health -= amount;
        }
        #endregion
    }
}