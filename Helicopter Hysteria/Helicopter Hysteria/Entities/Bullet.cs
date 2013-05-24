﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameHelperLibrary;
using Helicopter_Hysteria.States;

namespace Helicopter_Hysteria.Entities
{
    public class Bullet : Entity
    {
        private Texture2D texture;
        private Player owner;
        private Vector2 vel;
        private float speed = 7f;
        private float angle = 0.0f;
        private bool homing = false;

        public bool DestroyMe { get; set; }
        public Vector2 Origin { get; set; }

        public Vector2 Velocity
        {
            get { return vel; }
            set { vel = value; }
        }

        public Bullet(Player owner, Texture2D texture)
            : this(owner, owner.Angle, texture) { }

        public Bullet(Player owner, float angle, Texture2D texture, float speed = 10f)
            : base(owner.Position)
        {
            this.texture = texture;
            this.owner = owner;
            this.angle = angle;
            pos += owner.GunOffset;
            dir = owner.Direction;
            this.speed = speed;

            sprite = new Image(texture);
            width = sprite.Width;
            height = sprite.Height;
            flipped = owner.Flipped;

            Origin = new Vector2(width / 2, height / 2);
        }

        protected override void SetTexture()
        {
            return;
        }

        public override void Update(GameTime gameTime)
        {
            if (homing)
            {
                var target = GetNearestPlayer();

                var direction = (target.Position + new Vector2(target.Width / 2, target.Height / 2)) - Position;
                direction.Normalize();

                angle = dir < 0 ? (float)Math.Atan2(direction.Y, direction.X) - MathHelper.Pi :
                                  (float)Math.Atan2(direction.Y, direction.X);
            }

            Velocity = new Vector2((float)Math.Cos(angle) * speed * dir, (float)Math.Sin(angle) * speed * dir);
            pos += new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * speed * dir;
            
            if (pos.X < -width  || pos.X > Game1.GAME_WIDTH ||
                pos.Y < -height || pos.Y > Game1.GAME_HEIGHT)
                DestroyMe = true;
        }

        public override void Draw(SpriteBatch batch, GameTime gametime)
        {
            SpriteEffects e = flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            batch.Draw(sprite.ImageTexture, Position, null, Color.White,
                 angle, Origin, 1, e, 0f);
        }

        private Player GetNearestPlayer()
        {
            float distance = float.MaxValue;
            Player result = null;

            foreach (Player p in GameplayState.Players)
            {
                if (p == owner) continue; 
                var tempDistance = Vector2.Distance(p.Position, this.Position);
                if (tempDistance < distance)
                {
                    distance = tempDistance;
                    result = p;
                }
            }
            return result;
        }
    }
}
