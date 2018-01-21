using PaisleyRangers.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaisleyRangers.GameStates
{
    class Results
    {
        #region Declarations
        private Player[] players = new Player[4];

        private int[] places = new int[4];

        private int frameTimer = 320;

        private Texture2D firstBar, secondBar, thirdBar, fourthBar;
        private SpriteFont font;
        #endregion

        #region Constructor
        public Results() { }
        #endregion

        #region Initialisation
        public void Init(ContentManager cm, Player[] playerArray)
        {
            players = playerArray;

            frameTimer = 360;

            firstBar = cm.Load<Texture2D>("Menus/Win Screen/1st Place Bar");
            secondBar = cm.Load<Texture2D>("Menus/Win Screen/2nd Place Bar");
            thirdBar = cm.Load<Texture2D>("Menus/Win Screen/3rd Place Bar");
            fourthBar = cm.Load<Texture2D>("Menus/Win Screen/4th Place Bar");

            font = cm.Load<SpriteFont>("Pixel Font");

            for (int i = 0; i < players.Length; i++)
            {
                places[i] = 0;

                for (int y = 0; y < players.Length; y++)
                {
                    if (players[i].score > players[y].score && players[y].Enabled)
                    {
                        places[i]++;

                    }
                    else if (!players[y].Enabled)
                    {
                        places[i]++;
                    }
                }

                places[i] -= 4;
                places[i] = Math.Abs(places[i]);

                players[i].WorldLocation = new Vector2(240 + (480 * i), 800);
                players[i].IsWaiting(true);
            }
        }
        #endregion

        #region Public Methods
        public void Update(GameTime gameTime, GamePadState pad)
        {
            frameTimer--;

            if(pad.IsButtonDown(Buttons.A) && frameTimer <= 0)
            {
                PaisleyRangers.SetGameState(0);
            }

            for (int i = 0; i < players.Length; i++)
            {
                players[i].WorldLocation = new Vector2(180 + (480 * i), 800);
                players[i].Flip(true);
                players[i].CurrentAnimation("idle");
                players[i].Update(gameTime, GamePadState.Default);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin(samplerState: SamplerState.PointClamp);
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].Enabled)
                {
                    switch (places[i])
                    {
                        case 1:
                            sb.Draw(firstBar, new Vector2(0 + (480 * i), 0), Color.White);
                            sb.DrawString(font, "FIRST!", new Vector2((240 + (480 * i) - font.MeasureString("FIRST!").X * 7 / 2), 100 - font.MeasureString("FIRST!").Y * 7 / 2 - 5), Color.White, 0f, Vector2.Zero, 7f, SpriteEffects.None, 0f);
                            break;
                        case 2:
                            sb.Draw(secondBar, new Vector2(0 + (480 * i), 0), Color.White);
                            sb.DrawString(font, "SECOND!", new Vector2((240 + (480 * i) - font.MeasureString("SECOND!").X * 7 / 2), 100 - font.MeasureString("SECOND!").Y * 7 / 2 - 5), Color.White, 0f, Vector2.Zero, 7f, SpriteEffects.None, 0f);
                            break;
                        case 3:
                            sb.Draw(thirdBar, new Vector2(0 + (480 * i), 0), Color.White);
                            sb.DrawString(font, "THIRD!", new Vector2((240 + (480 * i) - font.MeasureString("THIRD!").X * 7 / 2), 100 - font.MeasureString("THIRD!").Y * 7 / 2 - 5), Color.White, 0f, Vector2.Zero, 7f, SpriteEffects.None, 0f);
                            break;
                        case 4:
                            sb.Draw(fourthBar, new Vector2(0 + (480 * i), 0), Color.White);
                            sb.DrawString(font, "FOURTH!", new Vector2((240 + (480 * i) - font.MeasureString("FOURTH!").X * 7 / 2), 100 - font.MeasureString("FOURTH!").Y * 7 / 2 - 5), Color.White, 0f, Vector2.Zero, 7f, SpriteEffects.None, 0f);
                            break;
                    }
                    players[i].Draw(sb);


                    sb.DrawString(font, "SCORE:", new Vector2(240 + (480 * i) - font.MeasureString("SCORE:").X * 7 / 2, 540 - font.MeasureString("SCORE:").Y * 7 / 2 - 5), Color.White, 0f, Vector2.Zero, 7f, SpriteEffects.None, 0f);
                    sb.DrawString(font, players[i].score.ToString(), new Vector2(240 + (480 * i) - font.MeasureString(players[i].score.ToString()).X * 7 / 2, 540 + font.MeasureString(players[i].score.ToString()).Y * 7 / 2 + 5), Color.White, 0f, Vector2.Zero, 7f, SpriteEffects.None, 0f);
               }
            }
            sb.End();
        }
        #endregion
    }
}
