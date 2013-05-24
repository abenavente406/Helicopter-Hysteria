using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameHelperLibrary;
using GameHelperLibrary.Shapes;

namespace Helicopter_Hysteria.Entities
{
    public class Particle : Entity
    {
        private Vector2 velocity;
        private Vector2 acceleration;
        private float MAXSPEED = 30f;
        private float initialDuration;
        private float remainingDuration;
        private Color initialColor;
        private Color finalColor;
        private Color tintColor;

        public bool Expired { get; set; }

        public float ElapsedDuration
        {
            get { return initialDuration - remainingDuration; }
        }

        public float DurationProgress
        {
            get { return ElapsedDuration / initialDuration; }
        }

        public bool IsActive
        {
            get { return remainingDuration > 0; }
        }


        public Particle(Vector2 pos, Texture2D texture, Vector2 vel, Vector2 accel, float duration,float maxSpeed,
            Color initalColor, Color endColor)
            : base(pos)
        {
            initialDuration = duration;
            remainingDuration = duration;
            this.velocity = vel;
            this.acceleration = accel;
            this.initialColor = initalColor;
            this.finalColor = endColor;
            this.MAXSPEED = maxSpeed;
            sprite = new Image(texture);
            width = sprite.Width;
            height = sprite.Height;
        }

        protected override void SetTexture()
        {
            return;
        }

        public override void Update(GameTime gameTime)
        {
            if (remainingDuration <= 0)
            {
                Expired = true;
            }

            if (!Expired)
            {
                velocity += acceleration; 
                if (velocity.Length() > MAXSPEED)
                {
                    Vector2 vel = velocity;
                    vel.Normalize();
                    velocity = vel * MAXSPEED;
                }
                tintColor = Color.Lerp(initialColor, finalColor, DurationProgress);
                remainingDuration--;

                Position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public override void Draw(SpriteBatch batch, GameTime gametime)
        {
            if (IsActive)
            {
                batch.Draw(
                        sprite.ImageTexture,
                        Position,
                        null,
                        tintColor,
                        0f,
                        Vector2.Zero,
                        1.0f,
                        SpriteEffects.None,
                        0.0f);
            }
        }
    }
}
