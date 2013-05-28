using System;
using System.Collections.Generic;
using Helicopter_Hysteria.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameHelperLibrary;

namespace Helicopter_Hysteria.Weapons
{
    #region Delegates
    /// <summary>
    /// Handles when the trigger is pulled
    /// </summary>
    /// <param name="sender">The owner of the weapon</param>
    /// <param name="e">Leave null for now</param>
    public delegate void OnTriggerHandler(Player sender, EventArgs e);
    #endregion

    /// <summary>
    /// Here is our weapon class. All guns will inherit from this class which is why it includes basic
    /// attributes that weapons would have.
    /// </summary>
    public abstract class Weapon
    {
        #region Fields
        // This event is called when the trigger 
        //    is pulled (or shootkey is pressed)
        // ------------------------------------
        public event OnTriggerHandler OnTrigger;

        // The owner of the gun
        // --------------------
        protected Player owner;

        // The key on the keyboard that triggers
        // the gun
        // -------------------------------------
        protected Keys shootKey;

        // How strong the gun is
        protected float damage;

        // How long it takes for the gun to cool down
        //-------------------------------------------
        protected float coolDownTime;
        protected float coolDownTimeTicks;

        // If the gun has been fired... Used when limiting
        // rapid fire to be fire-on-click
        // -----------------------------------------------
        protected bool fired = false;

        // Texture of the bullet
        // ---------------------
        protected Texture2D bulletTex;

        // Bullets shot by the gun on the field
        // ------------------------------------
        protected List<Bullet> bullets = new List<Bullet>();
        #endregion

        #region Properties
        /// <summary>
        /// Bullets shot by the gun on the field
        /// </summary>
        public List<Bullet> Bullets
        {
            get { return bullets; }
        }

        /// <summary>
        /// How strong the weapon is (amount of damage)
        /// </summary>
        public float Damage
        {
            get { return damage; }
        }
        #endregion

        #region Initialization
        public Weapon(Player owner, Keys shootKey, float damage, float coolDownTime)
        {
            // You know the deal: this.<variable> = variable;
            // ------------------------------------------------
            this.owner = owner;
            this.damage = damage;
            this.shootKey = shootKey;
            this.coolDownTime = coolDownTime;
            coolDownTimeTicks = coolDownTime;

            SetBulletTexture();

            OnTrigger += OnFire;
        }

        /// <summary>
        /// Every class inheriting from this should set the bullet texture
        /// here (if possible). You may also set it in the constructor of 
        /// the weapon. (See the laser gun class)
        /// </summary>
        protected abstract void SetBulletTexture();
        #endregion

        #region Update and Logic
        /// <summary>
        /// Called when the OnTrigger event is triggered
        /// </summary>
        /// <param name="sender">Owner of the weapon</param>
        /// <param name="e">Leave null for now</param>
        protected abstract void OnFire(Player sender, EventArgs e);

        public virtual void Update(GameTime gameTime)
        {
            // Decrease the cool down time because when coolDownTimeTicks == 0,
            // the gun is allowed to fire
            // ----------------------------------------------------------------
            if (coolDownTimeTicks > 0) 
                coolDownTimeTicks -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // If the shoot key is pressed or the Xbox trigger is pulled
            // ---------------------------------------------------------
            if (InputHandler.KeyDown(shootKey) || InputHandler.ButtonDown(Buttons.RightTrigger, owner.PlayerIndex))
            {
                // Only shoot if the gun is cooled down
                if (coolDownTimeTicks < 0)
                {
                    OnTrigger(owner, null);
                    coolDownTimeTicks = coolDownTime;
                    fired = true;
                }
            }
            else
            {
                fired = false;
            }

            // Here's the bullets's foreach method with a lambda!
            // ---------------------------------------------------
            // Update each of the guns bullets and if the bullet's
            // destroyMe variable returns true, delete the bullet.
            // ---------------------------------------------------
            bullets.ForEach((b) =>
            {
                b.Update(gameTime);
                if (b.DestroyMe)
                {
                    // If the gun is a missile launcher, make an explosion on impact
                    if (this.GetType() == typeof(MissileLauncher))
                    {
                        EffectManager.AddExplosion(b.Position, Vector2.Zero, 15, 20, 4, 6, 40f, 50, new Color(1.0f, 0.3f, 0f, 0.5f), Color.Black * 0f);
                    }
                    // Otherwise, make sparks
                    else
                    {
                        EffectManager.AddSparksEffect(b.Position, new Vector2(400));
                    }
                    bullets.Remove(b);
                }
            });
        }
        #endregion

        #region Draw
        public virtual void Draw(SpriteBatch batch, GameTime gameTime)
        {
            foreach (Bullet b in bullets)
                b.Draw(batch, gameTime);
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Shoots a bullet from the gun. __May be overriden__
        /// </summary>
        public virtual void Shoot()
        {
            bullets.Add(new Bullet(owner, bulletTex));
        }
        #endregion
    }
}
