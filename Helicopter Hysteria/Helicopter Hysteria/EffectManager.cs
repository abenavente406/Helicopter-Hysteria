using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Helicopter_Hysteria.Entities;
using GameHelperLibrary;

namespace Helicopter_Hysteria
{
    static class EffectManager
    {
        public static List<Particle> Effects = new List<Particle>();
        public static Texture2D ParticleTexture;
        public static List<Texture2D> ExplosionFrames = new List<Texture2D>();

        static Random rand = new Random();

        public static void Initialize(Texture2D particleTexture, Animation explosionFrame)
        {
            ParticleTexture = particleTexture;

            ExplosionFrames.Clear();

            foreach (Texture2D frame in explosionFrame.Images)
            {
                ExplosionFrames.Add(frame);
            }
        }

        public static Vector2 randomDirection(float scale)
        {
            Vector2 direction;
            do
            {
                direction = new Vector2(
                rand.Next(0, 100) - 50,
                rand.Next(0, 100) - 50);
            } while (direction.Length() == 0);
            direction.Normalize();
            direction *= scale;
            return direction;
        }

        public static void AddExplosion(Vector2 location, Vector2 momentum, int minPointCount,
            int maxPointCount, int minPieceCount, int maxPieceCount,
            float pieceSpeedScale, int duration, Color initialColor, Color finalColor)
        {
            int pointSpeedMin = (int)pieceSpeedScale * 2;
            int pointSpeedMax = (int)pieceSpeedScale * 3;
            Vector2 pieceLocation = location -
            new Vector2(ExplosionFrames[0].Width / 2,
            ExplosionFrames[0].Height / 2);
            int pieces = rand.Next(minPieceCount, maxPieceCount + 1); for (int x = 0; x < pieces; x++)
            {
                Effects.Add(new Particle(
                    pieceLocation,
                    ExplosionFrames[rand.Next(0, ExplosionFrames.Count)],
                    randomDirection(pieceSpeedScale) + momentum,
                    Vector2.Zero,
                    duration,
                    30f,
                    initialColor,
                    finalColor));
            }
            int points = rand.Next(minPointCount, maxPointCount + 1);
            for (int x = 0; x < points; x++)
            {
                Effects.Add(new Particle(
                    location,
                    ParticleTexture,
                    randomDirection((float)rand.Next(
                    pointSpeedMin, pointSpeedMax)) + momentum,
                    Vector2.Zero,
                    duration,
                    30f,
                    initialColor, finalColor));
            }
        }

        public static void AddSparksEffect(
            Vector2 location,
            Vector2 impactVelocity)
        {
            int particleCount = rand.Next(10, 20);
            for (int x = 0; x < particleCount; x++)
            {
                Particle particle = new Particle(
                    location - (impactVelocity / 60),
                    ParticleTexture,
                    randomDirection((float)rand.Next(10, 20)),
                    Vector2.Zero,
                    30,
                    20,
                    Color.Yellow,
                    Color.Orange);
                Effects.Add(particle);
            }
        }
        
        public static void Update(GameTime gameTime)
        {
            for (int x = Effects.Count - 1; x >= 0; x--)
            {
                Effects[x].Update(gameTime);
                if (Effects[x].Expired)
                {
                    Effects.RemoveAt(x);
                }
            }
        }

        public static void Draw(SpriteBatch batch, GameTime gameTime)
        {
            foreach (Entity e in Effects)
            {
                e.Draw(batch, gameTime);
            }
        }
    }
}
