using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helicopter_Hysteria.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameHelperLibrary;

namespace Helicopter_Hysteria.Weapons
{
    public delegate void OnTriggerHandler(Player sender, EventArgs e);

    public abstract class Weapon
    {
        public event OnTriggerHandler OnTrigger;

        protected Player owner;
        protected Keys shootKey;
        protected float damage;
        protected float coolDownTime;
        protected float coolDownTimeTicks;
        protected bool fired = false;
        protected Texture2D bulletTex;
        protected List<Bullet> bullets = new List<Bullet>();

        public List<Bullet> Bullets
        {
            get { return bullets; }
        }

        public Weapon(Player owner, Keys shootKey, float damage, float coolDownTime)
        {
            this.owner = owner;
            this.damage = damage;
            this.shootKey = shootKey;
            this.coolDownTime = coolDownTime;
            coolDownTimeTicks = coolDownTime;

            SetBulletTexture();

            OnTrigger += OnFire;
        }

        protected abstract void SetBulletTexture();
        protected abstract void OnFire(Player sender, EventArgs e);

        public virtual void Update(GameTime gameTime)
        {
            if (coolDownTimeTicks > 0) 
                coolDownTimeTicks -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (InputHandler.KeyDown(shootKey))
            {
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

            bullets.ForEach((b) =>
            {
                b.Update(gameTime);
                if (b.DestroyMe) bullets.Remove(b);
            });
        }

        public virtual void Shoot()
        {
            bullets.Add(new Bullet(owner, bulletTex));
        }

        public virtual void Draw(SpriteBatch batch, GameTime gameTime)
        {
            foreach (Bullet b in bullets)
                b.Draw(batch, gameTime);
        }
    }
}
