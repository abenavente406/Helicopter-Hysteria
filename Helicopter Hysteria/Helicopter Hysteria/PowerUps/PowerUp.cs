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
using Helicopter_Hysteria.Entities;
using Helicopter_Hysteria.States;

namespace Helicopter_Hysteria
{
    public delegate void OnEffectStartHandler(Player sender, EventArgs e);
    public delegate void OnEffectEndHandler(Player sender, EventArgs e);
    public abstract class PowerUp
    {
        public event OnEffectStartHandler OnEffectStart;
        public event OnEffectEndHandler OneEffectEnd;
        protected float duration = 0;
        protected const float MAXDURATION = 5000;
       public bool destroyMe;
        protected Player owner;
        protected Vector2 postion;
        protected Texture2D Image;
        protected int width, height;
        bool active = false;
        Random rand = new Random();
        public Rectangle Bounds
        {
            get { return new Rectangle((int)postion.X, (int)postion.Y, width, height); }

        }
        public PowerUp(Vector2 pos, Texture2D image, int width, int height)
        {
            postion = pos;
            Image = image;
            this.width = width;
            this.height = height;

            OnEffectStart += StartEffect;
            OneEffectEnd += EndEffect;
        }


        public abstract void StartEffect(Player sender, EventArgs e);
        public abstract void EndEffect(Player sender, EventArgs e);

        public virtual void Update(GameTime gametime)
        {
            if (!destroyMe && !active)
            {
                foreach (Player p in GameplayState.Players)
                {
                    if (p.Bounds.Intersects(Bounds))
                    {
                        duration = 0;
                        active = true;
                        owner = p;
                        OnEffectStart(owner, null);
                    }
                }
            }
            if (active)
            {
                duration += (float)gametime.ElapsedGameTime.Milliseconds;

            }
            if (duration >= MAXDURATION)
            {
                OneEffectEnd(owner, null);
                destroyMe = true;
            }



        }
        public virtual void Draw(SpriteBatch batch)
        {
            if (!active)
            {
                batch.Draw(Image, Bounds, Color.White);

            }
        }

    }
}