using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Helicopter_Hysteria.States;
using GameHelperLibrary;
using Helicopter_Hysteria.Weapons;

namespace Helicopter_Hysteria.Entities
{
    public enum AIStates
    {
        DODGE,
        ATTACK,
        ROAM
    }
    public class ComputerPlayer : Player
    {
        private AIStates AIState = AIStates.ROAM;
        private Vector2 destPos;
        private Color tint;
        private Random rand = new Random();

        public Vector2 DestinationPos
        {
            get { return destPos; }
            set
            {
                destPos = Vector2.Clamp(value, new Vector2(width / 2, height / 2),
                    new Vector2(Game1.GAME_WIDTH - width / 2, Game1.GAME_HEIGHT - height / 2));
            }
        }

        public ComputerPlayer() : base(PlayerIndex.Two)
        {
            // Set the position to be on the right side of the screen
            Position = new Vector2(Game1.GAME_WIDTH - width, Game1.GAME_HEIGHT / 2 - height / 2);

            // Tint is chosen from the IntroState
            tint = IntroState.player2Color;
            sprite.Tint = tint;

            // Computer player will be facing left
            dir = -1;
            flipped = true;

            destPos = new Vector2(rand.Next(width / 2 + 5, Game1.GAME_WIDTH - width / 2), 
                rand.Next(height / 2 + 5, Game1.GAME_HEIGHT - height / 2));
        }

        protected override void SetTexture()
        {
            sprite = new Image(Game1.content.Load<Texture2D>("helicopter2"));
            width = sprite.Width;
            height = sprite.Height;
        }

        public override void Update(GameTime gameTime)
        {
            Player player = GetNeareastPlayer();

            switch (AIState)
            {
                case AIStates.DODGE:

                    break;
                case AIStates.ATTACK:
                    
                    break;
                case AIStates.ROAM:
                    if (!(Vector2.Distance(DestinationPos, Position) < 5))
                    {
                        var dir = DestinationPos - Position;
                        dir.Normalize();
                        Position += dir * 5f;
                    }
                    else
                    {
                        destPos = new Vector2(rand.Next(width / 2 + 5, Game1.GAME_WIDTH - width / 2),
                            rand.Next(height / 2 + 5, Game1.GAME_HEIGHT - height / 2));
                    }
                    break;
                default:
                    AIState = AIStates.DODGE;
                    break;
            }

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

            equippedWeapon.Update(gameTime);
        }

        private Player GetNeareastPlayer()
        {
            Player result = null;
            float distance = float.MaxValue;
            foreach (Player p in GameplayState.Players)
            {
                if (p == this) continue;

                if (Vector2.Distance(Position, p.Position) < distance)
                {
                    result = p;
                    distance = Vector2.Distance(Position, p.Position);
                }
            }
            return result;  
        }

        protected override void HandleInput()
        {
            return;
        }

        public override void Draw(SpriteBatch batch, GameTime gametime)
        {
            base.Draw(batch, gametime);
        }
    }
}
