using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaisleyRangers.GameStates
{
    class Settings
    {
        #region Declarations
        private Texture2D background;

        int screenwidth = 1920;
        int screenheight = 1080;

        Color Colselected = Color.Cyan;
        Color Colunselected = Color.DarkGray;
        Color musicOn;
        Color musicOff;
        Color soundOn;
        Color soundOff;
        Color colSelMusic;
        Color colSelSound;
 
        bool SelMusic = true;
        bool SelSound = false;

        private int optionTimer = 20;
        #endregion

        #region Constructor
        public Settings() { }
        #endregion

        #region Initialisation
        public void Init(ContentManager cm) {
            background = cm.Load<Texture2D>("Menus/Options Screen");
        }
        #endregion

        #region Public Methods
        public void Update()
        {
            optionTimer--;

            if(GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.B) && optionTimer <= 0)
            {
                PaisleyRangers.SetGameState(0);
            }

            if (SelMusic) {
                colSelMusic = Colselected;
                colSelSound = Colunselected;
                if ((GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y <= -0.3f || GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed||
                    GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y >= 0.3f || GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed) && optionTimer <= 0)
                {
                    SelMusic = false;
                    SelSound = true;
                    optionTimer = 20;
                }
                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) && optionTimer <= 0)
                {
                    PaisleyRangers.musicEnabled = !PaisleyRangers.musicEnabled;
                    optionTimer = 20;
                    if (PaisleyRangers.musicEnabled)
                    {
                        MediaPlayer.Play(PaisleyRangers.bgMusic);
                    }
                    if (!PaisleyRangers.musicEnabled)
                    {
                        MediaPlayer.Stop();
                    }
                }
            }
            if (SelSound)
            {
                colSelMusic = Colunselected;
                colSelSound = Colselected;
                if ((GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y <= -0.3f || GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed ||
                    GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y >= 0.3f || GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed )&& optionTimer <= 0)
                {
                    SelMusic = true;
                    SelSound = false;
                    optionTimer = 20;
                }
                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) && optionTimer <= 0)
                {
                    PaisleyRangers.SFXEnabled = !PaisleyRangers.SFXEnabled;
                    optionTimer = 20;
                    
                }
            }

            if (PaisleyRangers.musicEnabled)
            {
                musicOn = Colselected;
                musicOff = Colunselected;
            }
            if (!PaisleyRangers.musicEnabled)
            {
                musicOn = Colunselected;
                musicOff = Colselected;
            }

            if (PaisleyRangers.SFXEnabled)
            {
                soundOn = Colselected;
                soundOff = Colunselected;
            }
            if (!PaisleyRangers.SFXEnabled)
            {
                soundOn = Colunselected;
                soundOff = Colselected;
            }

        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin(samplerState: SamplerState.PointClamp);

            sb.Draw(background, Vector2.Zero, Color.White);

            sb.DrawString(Menu.font, "MUSIC: ", new Vector2(screenwidth / 2 - 350, screenheight / 2 - 200), colSelMusic, 0f, Vector2.Zero, 7, SpriteEffects.None, 0);
            sb.DrawString(Menu.font, "ON", new Vector2(screenwidth / 2 + 50, screenheight / 2 - 200), musicOn, 0f, Vector2.Zero, 7, SpriteEffects.None, 0);
            sb.DrawString(Menu.font, "OFF", new Vector2(screenwidth / 2 + 200, screenheight / 2 - 200), musicOff, 0f, Vector2.Zero, 7, SpriteEffects.None, 0);
            sb.DrawString(Menu.font, "SOUND: ", new Vector2(screenwidth / 2 - 350, screenheight / 2 - 100), colSelSound, 0f, Vector2.Zero, 7, SpriteEffects.None, 0);
            sb.DrawString(Menu.font, "ON", new Vector2(screenwidth / 2 + 50, screenheight / 2 - 100), soundOn, 0f, Vector2.Zero, 7, SpriteEffects.None, 0);
            sb.DrawString(Menu.font, "OFF", new Vector2(screenwidth / 2 + 200, screenheight / 2 - 100), soundOff, 0f, Vector2.Zero, 7, SpriteEffects.None, 0);

            sb.End();
        }
        #endregion
    }
}
