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

namespace Helicopter_Hysteria.States
{
    public class IntroState : BaseGameState
    {

        public Texture2D screen1, screen2, screen3, screen4, random;
        public static Button player1left, player1right, player2left, player2right;
        private Texture2D backgroundImage, arrowkeyimage;
        private SpriteFont skyFallFont;
        private Random rand = new Random();
        private Rectangle helicopter1, helicopter2;
        public static Color player1Color = Color.White;
        public static Color player2Color = Color.LightBlue;
        int player1num = 0;
        int player2num = 0;

        List<Button> buttons = new List<Button>();

        private string[] buttonNames = new string[] 
        {
            "ButtonOne", 
            "ButtonTwo", 
            "ButtonThree", 
            "ButtonFour", 
            "ButtonRandom",
            "player1Left", 
            "player2Left", 
            "player1Right", 
            "player2Right" 
        };

        private Color[] playerColors = new Color[]
        {
            Color.Khaki,
            Color.Olive,
            Color.White,
            Color.Gray,
            Color.Tan,
            Color.DarkSlateGray,
            Color.LightBlue
        };

        public IntroState(Game game, GameStateManager gamestate)
            : base(game, gamestate)
        {
            

        }

        protected override void LoadContent()
        {
            var content = gameRef.Content;
            backgroundImage = content.Load<Texture2D>("Screen Images//IntroScreen");
            skyFallFont = content.Load<SpriteFont>("skyFall");
            arrowkeyimage = content.Load<Texture2D>("ArrowKey");
            helicopter1 = new Rectangle(170, 150, 250, 100);
            helicopter2 = new Rectangle(770, 150, 250, 100);

            #region contentload
            screen1 = content.Load<Texture2D>("Screen Images//background");
            screen2 = content.Load<Texture2D>("Screen Images//apocalypselondon");
            screen3 = content.Load<Texture2D>("Screen Images//screen3");
            screen4 = content.Load<Texture2D>("Screen Images//screen4");
            random = content.Load<Texture2D>("Screen Images//question mark");
            #endregion


            Texture2D[] backgrounds = new Texture2D[]
            {
                screen1, screen2, screen3, screen4, random, arrowkeyimage
            };

            Point startPoint = new Point(200, 550);

            for (int i = 0; i < 5; i++)
            {
                var button = new Button(skyFallFont);
                button.Position = startPoint;
                button.Width = 150;
                button.Height = 100;
                button.Name = buttonNames[i];
                button.BackgroundImage = backgrounds[i];
                button.OnClick += screenImage;
                buttons.Add(button);
                startPoint.X += 200;
            }

            startPoint = new Point(50, 200);
            for (int i = 5; i < 9; i++)
            {
                var button = new Button(skyFallFont);
                button.Position = startPoint;
                button.Width = 70;
                button.Height = 70;
                button.BackgroundImage = backgrounds[5];
                button.Name = buttonNames[i];
                button.OnClick += colorChange;
                button.FlipImage = buttonNames[i].ToLower().Contains("left");
                buttons.Add(button);
                startPoint.X += 600;
                if (buttonNames[i].Equals("player2Left"))
                    startPoint = new Point(450, 200);
            }
            
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Button b in buttons)
                b.Update(gameTime);

            if (player1num < 0) player1num = playerColors.Length - 1;
            if (player2num < 0) player2num = playerColors.Length - 1;
            player1num %= playerColors.Length;
            player2num %= playerColors.Length;

            colorsplayer1();
            colorsplayer2();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch batch = gameRef.spriteBatch;

            batch.Begin();
            batch.Draw(backgroundImage, new Rectangle(0, 0, Game1.GAME_WIDTH, Game1.GAME_HEIGHT), Color.White);
            batch.DrawString(skyFallFont, "Step one choose helo-color!", new Vector2(65, 50), Color.Black);
            batch.DrawString(skyFallFont, "Step two choose environment.", new Vector2(65, 400), Color.Black);

            foreach (Button b in buttons)
                b.Draw(batch, gameTime);

            batch.Draw(content.Load<Texture2D>("helicopter2"), helicopter1, player1Color);
            batch.Draw(content.Load<Texture2D>("helicopter2"), helicopter2, null, player2Color, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            FadeOutRect.Draw(batch, Vector2.Zero, FadeOutColor);
            batch.End();

            base.Draw(gameTime);
        }
        public void screenImage(object sender, EventArgs e)
        {
            var button = (Button)sender;
            switch (button.Name)
            {
                case "ButtonOne":
                    {
                        GameplayState.BackgroundImage = screen1;
                        break;
                    }
                case "ButtonTwo":
                    {
                        GameplayState.BackgroundImage = screen2;
                        break;
                    }
                case "ButtonThree":
                    {
                        GameplayState.BackgroundImage = screen3;
                        break;
                    }
                case "ButtonFour":
                    {
                        GameplayState.BackgroundImage = screen4;
                        break;
                    }
                case "ButtonRandom":
                    {
                        GameplayState.BackgroundImage = generateRandom();
                        break;
                    }
            }
            SwitchState(new GameplayState(gameRef, StateManager, false));
        }
        public Texture2D generateRandom()
        {
            var num = rand.NextDouble();

            if (num < .3 && num > .1)
            {
                return screen1;
            }
            else if (num < .5 && num > .3)
            {
                return screen2;
            }
            else if (num < .7 && num > .5)
            {
                return screen3;
            }
            else
                return screen4;


        }
        public void colorChange(object sender, EventArgs e)
        {
            var button = (Button)sender;
            switch (button.Name.ToLower())
            {
                case "player1left":
                    {
                        player1num--;
                        break;
                    }
                case "player1right":
                    {
                        player1num++;
                        break;
                    }
                case "player2left":
                    {
                        player2num--;
                        break;
                    }
                case "player2right":
                    {
                        player2num++;
                        break;
                    }
            }
        }

        public void colorsplayer1()
        {
            player1Color = playerColors[player1num];
        }
        
        public void colorsplayer2()
        {
            player2Color = playerColors[player2num];
        }
    }
}
