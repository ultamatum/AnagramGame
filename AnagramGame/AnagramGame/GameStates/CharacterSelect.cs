using PaisleyRangers.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PaisleyRangers.GameStates
{
    class CharacterSelect
    {
        #region Declarations
        private int[] optionTimer = new int[4] { 0, 0, 0, 0};
        private int activePlayers = 0;
        private int readyPlayers = 0;
        private int[] currentColourSelection = new int[4];
        private int[] currentWeaponSelection = new int[4];

        private bool[] colourSelected = new bool[4];
        private bool[] weaponSelected = new bool[4];
        private bool[] playerReady = new bool[4];

        //The array of all possible colours
        private Color[] colours = new Color[25] {
            new Color(254,6,24), new Color(34,228,40), new Color(1,106,202), new Color(138,1,202), new Color(202,105,1),
            new Color(185,110,66), new Color(63,127,96), new Color(17,199,162), new Color(204,16,209), new Color(199,81,1),
            new Color(81,185,111), new Color(90,182,62), new Color(146,175,40), new Color(188,138,19), new Color(196,57,1),
            new Color(6,254,190), new Color(215,34,167), new Color(154,254,6), new Color(207,66,132), new Color(194,34,1),
            new Color(0,0,0), new Color(255,255,255), new Color(122,122,122), Color.Gold, new Color(255,48,217)};
        private Color[] selectedColours = new Color[4];
        private Vector2[] colourPositions = new Vector2[25];
        private Vector2[] weaponPositions = new Vector2[4];

        private GamePadState[] prevpads;

        private SpriteFont font;

        private Texture2D colourHolder;
        private Texture2D colourPicker;
        private Texture2D background;
        private Texture2D weaponPicker;
        private Texture2D[] weaponHolder = new Texture2D[4];
        private Texture2D pressStart;
        #endregion

        #region Constructor
        public CharacterSelect() {}
        #endregion

        #region Initialisation
        public void Init(ContentManager cm)
        {
            background = cm.Load<Texture2D>("Menus/Character Selection Screen");
            colourHolder = cm.Load<Texture2D>("Menus/Character Select/Colour Picker");
            colourPicker = cm.Load<Texture2D>("Menus/Character Select/Currently Selected Colour");

            //The weapon selection boxes
            weaponHolder[0] = cm.Load<Texture2D>("Menus/Character Select/Sword Box");
            weaponHolder[1] = cm.Load<Texture2D>("Menus/Character Select/Spear Box");
            weaponHolder[2] = cm.Load<Texture2D>("Menus/Character Select/Mace Box");
            weaponHolder[3] = cm.Load<Texture2D>("Menus/Character Select/Warhammer Box");
            weaponPicker = cm.Load<Texture2D>("Menus/Character Select/Current Weapon Selection");

            pressStart = cm.Load<Texture2D>("Menus/Character Select/Press Start");

            font = cm.Load<SpriteFont>("Pixel Font");

            activePlayers = 0;
            readyPlayers = 0;

            //Reset all the player variables
            for (int i = 0; i < PaisleyRangers.players.Length; i++)
            {
                PaisleyRangers.players[i] = new Player();
                PaisleyRangers.players[i].Enabled = false;
                PaisleyRangers.players[i].IsWaiting(true);
                PaisleyRangers.players[i].Init(cm);
                PaisleyRangers.players[i].score = 0;
                PaisleyRangers.players[i].health = 0;
                PaisleyRangers.players[i].PlayAnimation("idle");
                PaisleyRangers.players[i].isReady = false;
                playerReady[i] = false;
                currentWeaponSelection[i] = 0;
                currentColourSelection[i] = 0;
                colourSelected[i] = false;
                weaponSelected[i] = false;
                optionTimer[i] = 20;
            }
        }
        #endregion

        #region Public Methods
        public void Update(GameTime gameTime, Player[] players)
        {
            GamePadState[] gamePads = PaisleyRangers.pads;

            for(int i = 0; i < gamePads.Length; i++)
            {
                optionTimer[i]--;

                #region Colour Picker Updates
                if (!colourSelected[i])
                {
                    if (gamePads[i].IsButtonDown(Buttons.Start) && players[i].Enabled == false)
                    {
                        if(PaisleyRangers.SFXEnabled)PaisleyRangers.newCharSFX.Play();
                        players[i].Enabled = true;
                        activePlayers += 1;
                    }

                    if (players[i].Enabled)
                    {
                        if (gamePads[i].IsButtonDown(Buttons.A) && optionTimer[i] <= 0)
                        {
                            if (PaisleyRangers.SFXEnabled) PaisleyRangers.selectSFX.Play();
                            selectedColours[i] = colours[currentColourSelection[i]];
                            colourSelected[i] = true;
                            optionTimer[i] = 20;
                        }

                        if (gamePads[i].IsButtonDown(Buttons.B) && optionTimer[i] <= 0)
                        {
                            players[i].Enabled = false;
                            activePlayers -= 1;
                            optionTimer[i] = 20;
                        }

                        if ((gamePads[i].ThumbSticks.Left.Y >= 0.3 || GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed) && optionTimer[i] <= 0)
                        {
                            if (PaisleyRangers.SFXEnabled) PaisleyRangers.moveSelectSFX.Play();
                            currentColourSelection[i]--;
                            optionTimer[i] = 20;
                        }

                        if ((gamePads[i].ThumbSticks.Left.Y <= -0.3 || GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed) && optionTimer[i] <= 0)
                        {
                            if (PaisleyRangers.SFXEnabled) PaisleyRangers.moveSelectSFX.Play();
                            currentColourSelection[i]++;
                            optionTimer[i] = 20;
                        }

                        if ((gamePads[i].ThumbSticks.Left.X >= 0.3 || GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed) && optionTimer[i] <= 0)
                        {
                            if (PaisleyRangers.SFXEnabled) PaisleyRangers.moveSelectSFX.Play();
                            currentColourSelection[i] += 5;
                            optionTimer[i] = 20;
                        }

                        if ((gamePads[i].ThumbSticks.Left.X <= -0.3 || GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed) && optionTimer[i] <= 0)
                        {
                            if (PaisleyRangers.SFXEnabled) PaisleyRangers.moveSelectSFX.Play();
                            currentColourSelection[i] -= 5;
                            optionTimer[i] = 20;
                        }

                        if (currentColourSelection[i] < 0) currentColourSelection[i] = 0;
                        if (currentColourSelection[i] >= colours.Length) currentColourSelection[i] = colours.Length - 1;
                    }

                    //Go back to the main menu
                    if (gamePads[0].IsButtonDown(Buttons.B) && optionTimer[0] <= 0 && activePlayers == 0)
                    {
                        PaisleyRangers.SetGameState(0);
                    }

                    prevpads = gamePads;
                }
                #endregion

                #region Weapon Picker Updates
                if (colourSelected[i] && !weaponSelected[i])
                {
                    if (gamePads[i].IsButtonDown(Buttons.A) && optionTimer[i] <= 0)
                    {
                        if (PaisleyRangers.SFXEnabled) PaisleyRangers.selectSFX.Play();
                        players[i].selectedWeapon = currentWeaponSelection[i];
                        weaponSelected[i] = true;
                        playerReady[i] = true;
                        readyPlayers++;
                        optionTimer[i] = 20;
                    }

                    if ((gamePads[i].ThumbSticks.Left.Y >= 0.3 || GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed) && optionTimer[i] <= 0)
                    {
                        if (PaisleyRangers.SFXEnabled) PaisleyRangers.moveSelectSFX.Play();
                        currentWeaponSelection[i]--;
                        optionTimer[i] = 20;
                    }

                    if ((gamePads[i].ThumbSticks.Left.Y <= -0.3 || GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed) && optionTimer[i] <= 0)
                    {
                        if (PaisleyRangers.SFXEnabled) PaisleyRangers.moveSelectSFX.Play();
                        currentWeaponSelection[i]++;
                        optionTimer[i] = 20;
                    }

                    if ((gamePads[i].ThumbSticks.Left.X >= 0.3 || GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed) && optionTimer[i] <= 0)
                    {
                        if (PaisleyRangers.SFXEnabled) PaisleyRangers.moveSelectSFX.Play();
                        currentWeaponSelection[i] += 2;
                        optionTimer[i] = 20;
                    }

                    if ((gamePads[i].ThumbSticks.Left.X <= -0.3 || GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed) && optionTimer[i] <= 0)
                    {
                        if (PaisleyRangers.SFXEnabled) PaisleyRangers.moveSelectSFX.Play();
                        currentWeaponSelection[i] -= 2;
                        optionTimer[i] = 20;
                }

                    if (currentWeaponSelection[i] <= 0) currentWeaponSelection[i] = 1;
                    if (currentWeaponSelection[i] > 4) currentWeaponSelection[i] = 4;

                    if (gamePads[i].IsButtonDown(Buttons.B) && optionTimer[i] <= 0)
                    {
                        colourSelected[i] = false;
                        currentColourSelection[i] = 0;
                        currentWeaponSelection[i] = 0;
                        optionTimer[i] = 20;
                    }

                    players[i].selectedWeapon = currentWeaponSelection[i];
                }
                #endregion

                if (playerReady[i])
                {
                    if (gamePads[i].IsButtonDown(Buttons.B) && optionTimer[i] <= 0)
                    {
                        playerReady[i] = false;
                        weaponSelected[i] = false;
                        currentWeaponSelection[i] = 1;
                        readyPlayers--;
                        optionTimer[i] = 20;
                    }
                }
            }

            for (int i = 0; i < players.Length; i++)
            {
                players[i].WorldLocation = new Vector2(195 + (478 * i), 411);
                players[i].Colour = colours[currentColourSelection[i]];
                players[i].selectedWeapon = currentWeaponSelection[i];
                players[i].Update(gameTime, GamePadState.Default);
            }

            if(activePlayers == readyPlayers && activePlayers != 0)
            {
                for(int i = 0; i < players.Length; i++)
                {
                    players[i].IsWaiting(false);
                }

                PaisleyRangers.SetGameState(3);
            }
        }

        public void Draw(SpriteBatch sb, Player[] players)
        {
            sb.Begin(SpriteSortMode.BackToFront, samplerState: SamplerState.PointClamp);
            sb.Draw(background, Vector2.Zero, null, null, null, 0, null, Color.White, SpriteEffects.None, 1f);

            for(int i = 0; i < 4; i++)
            {
                if (players[i].Enabled)
                {
                    players[i].Draw(sb);
                }
            }

            for (int i = 0; i < 4; i++)
            {
                #region Colour Selector Draw
                if (players[i].Enabled)
                {
                    if (!colourSelected[i])
                    {
                        int counter = 0;
                        for (int x = 0; x < 5; x++)
                        {
                            for (int y = 0; y < 5; y++)
                            {
                                colourPositions[counter] = new Vector2(((101 + (62 * x)) + (480 * i)), (620 + (62 * y)));
                                sb.Draw(colourHolder, colourPositions[counter], null, null, null, 0, null, colours[counter], SpriteEffects.None, 0.1f);
                                counter++;
                            }
                        }
                        counter = 0;

                        sb.Draw(colourPicker, new Vector2(colourPositions[currentColourSelection[i]].X - 10, colourPositions[currentColourSelection[i]].Y - 10), null, null, null, 0, null, Color.White, SpriteEffects.None, 0f);
                    }
                    else if (colourSelected[i] && !weaponSelected[i])
                    {
                        int counter = 0;
                        for (int x = 0; x < 2; x++)
                        {
                            for (int y = 0; y < 2; y++)
                            {
                                weaponPositions[counter] = new Vector2(((101 + (64 * (x + 1))) + (480 * i)), (620 + (64 * (y + 1))));
                                sb.Draw(weaponHolder[counter], weaponPositions[counter], null, null, null, 0, null, Color.White, SpriteEffects.None, 0.1f);
                                counter++;
                            }
                        }
                        counter = 0;


                        sb.Draw(colourPicker, new Vector2(weaponPositions[currentWeaponSelection[i] - 1].X - 8, weaponPositions[currentWeaponSelection[i] - 1].Y - 8), null, null, null, 0, null, Color.White, SpriteEffects.None, 0f);
                    }
                }
                else
                {
                    sb.Draw(pressStart, new Vector2(72 + (480 * i), 358), Color.White);
                }
                #endregion
            }

            sb.End();
        }
        #endregion
    }
}
