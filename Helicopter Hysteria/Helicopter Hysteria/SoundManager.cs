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


namespace Helicopter_Hysteria
{
    public class SoundManager : Microsoft.Xna.Framework.GameComponent
    {
        #region Fields
        Game1 gameRef;

        private static Song themeMusic;
        private static SoundEffect powerUp;
        private static SoundEffect hit;
        private static SoundEffect boom;
        private static SoundEffect shot;
        private static SoundEffect shotgunShot;
        private static SoundEffect missileLaunch;
        #endregion

        #region Properties
        /// <summary>
        /// Sound effect of bullets hitting other players
        /// </summary>
        public static SoundEffectInstance Hit
        {
            get { var result = hit.CreateInstance(); result.Volume *= .1f; return result; }
        }

        /// <summary>
        /// Sound effect of missiles hitting other players
        /// </summary>
        public static SoundEffectInstance Boom
        {
            get { var result = boom.CreateInstance(); result.Volume *= .2f; return result; }
        }

        /// <summary>
        /// Sound effect of missile launch
        /// </summary>
        public static SoundEffectInstance MissileLaunchInstance
        {
            get { var result = missileLaunch.CreateInstance(); result.Volume *= .1f; return result; }
        }

        /// <summary>
        /// Sound effect of powerup
        /// </summary>
        public static SoundEffectInstance PowerUpInstance
        {
            get { var result = powerUp.CreateInstance(); result.Volume *= .4f; return result; }
        }

        /// <summary>
        /// Sound effect of shot gun
        /// </summary>
        public static SoundEffectInstance ShotgunShotInstance
        {
            get { var result = shotgunShot.CreateInstance(); result.Volume *= .1f; return result; }
        }

        /// <summary>
        /// Sound effect of pistol
        /// </summary>
        public static SoundEffectInstance PistolShotInstance
        {
            get { var result = shot.CreateInstance(); result.Volume *= .1f; result.Pitch *= .1f; return result; }
        }

        /// <summary>
        /// Sound effect of gatling gun
        /// </summary>
        public static SoundEffectInstance GatlingGunShotInstance
        {
            get { var result = shot.CreateInstance(); result.Volume *= .1f; return result; }
        }

        public static Song ThemeMusic
        {
            get { return themeMusic; }
        }
        #endregion

        public SoundManager(Game game)
            : base(game)
        {
            gameRef = (Game1)game;
        }

        public override void Initialize()
        {
            base.Initialize();

            // ------------------------------
            // HOW TO MAKE A NEW SOUND EFFECT
            // ------------------------------
            // 1. First make a SoundEffect in the fields region and name it whatever.
            // 2. Next, load it with Content.Load<SoundEffect> like shown below
            // 3. Next, make a property in the properties section named however you 
            //    want the sound effect instance to be called
            // 4. Now follow what I did in the get method in case you want to change 
            //    the playing volumen of the sound effect.
            // 5. (optional) You can change IsLooping by result.IsLooping = true but
            //    I'm sure you knew how to do that

            themeMusic = gameRef.Content.Load<Song>("SFX\\The Recon Mission");
            powerUp = gameRef.Content.Load<SoundEffect>("SFX\\powerUp");
            hit = gameRef.Content.Load<SoundEffect>("SFX\\bulletHit");
            boom = gameRef.Content.Load<SoundEffect>("SFX\\boom");
            shot = gameRef.Content.Load<SoundEffect>("SFX\\baseShot");
            shotgunShot = gameRef.Content.Load<SoundEffect>("SFX\\shotgunShot");
            missileLaunch = gameRef.Content.Load<SoundEffect>("SFX\\missileShot");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public static void PlaySong(Song song)
        {
            MediaPlayer.Volume = .3f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);
        }

        public static void Stop()
        {
            MediaPlayer.Stop();
        }
    }
}
