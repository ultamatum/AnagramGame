using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PaisleyRangers.GameStates
{    
    class Menu
    {
        #region Declarations
        private int currentChoice = 0;
        private int screenWidth;
        private int screenHeight;
        private int optionTimer;

        private bool help = false;

        private Texture2D background;
        private Texture2D controller;
        public static SpriteFont font;
        private string[] options = new string[] { "PLAY", "HELP", "OPTIONS", "QUIT" };
        #endregion

        #region Constructor
        public Menu(int screenWidth, int screenHeight)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
        }
        #endregion

        #region Public Methods
        public void Init(ContentManager cm)
        {
            background = cm.Load<Texture2D>("Menus/Main Menu Screen");
            font = cm.Load<SpriteFont>("Pixel Font");
            controller = cm.Load<Texture2D>("Menus/controller layout");

            optionTimer = 20;
        }

        public void Update()
        {
            if (!help)
            {
                optionTimer--;

                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) && optionTimer <= 0)
                {
                    if(PaisleyRangers.SFXEnabled) PaisleyRangers.selectSFX.Play();

                    //Selection options
                    switch (currentChoice)
                    {
                        case 0:
                            PaisleyRangers.SetGameState(2);                 //Changes to the character select gamestate
                            break;
                        case 1:
                            help = true;
                            break;
                        case 2:
                            PaisleyRangers.SetGameState(4);                 //Changes to the settings screen
                            break;
                        case 3:
                            System.Environment.Exit(0);
                            break;
                    }
                }

                if ((GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y <= -0.3f || GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed) && optionTimer <= 0)
                {
                    if (PaisleyRangers.SFXEnabled) PaisleyRangers.moveSelectSFX.Play();
                    currentChoice++;
                    optionTimer = 20;
                }

                if ((GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y >= 0.3f || GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed) && optionTimer <= 0)
                {
                    if (PaisleyRangers.SFXEnabled) PaisleyRangers.moveSelectSFX.Play();
                    currentChoice--;
                    optionTimer = 20;
                }

                if (currentChoice >= options.Length) currentChoice = 0;
                else if (currentChoice < 0) currentChoice = options.Length - 1;
            }
            else
            {
                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.B))
                {
                    help = false;
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin(samplerState: SamplerState.PointClamp);
            Color colour;

            sb.Draw(background, Vector2.Zero, Color.White);

            if (help == true)
            {
                sb.Draw(controller, new Vector2((screenWidth / 2 - controller.Width / 2), 350), Color.White);
            }
            else
            {
                for (int i = 0; i < options.Length; i++)
                {
                    if (i == currentChoice)
                    {
                        colour = Color.Cyan;
                    }
                    else
                    {
                        colour = Color.White;
                    }
                    sb.DrawString(font, options[i], new Vector2(screenWidth / 2 - (font.MeasureString(options[i]).X * 9 / 2), screenHeight / 8 * (4 + i)), colour, 0f, Vector2.Zero, 9f, SpriteEffects.None, 1f);
                }
            }
            sb.End();
        }

        #endregion
    }
}
