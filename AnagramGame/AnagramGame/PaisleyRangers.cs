using PaisleyRangers.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PaisleyRangers.Sprites;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace PaisleyRangers
{
    public class PaisleyRangers : Microsoft.Xna.Framework.Game
    {
        #region Declarations
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        const int SCREENWIDTH = 1920;
        const int SCREENHEIGHT = 1080;
        const bool FULLSCREEN = true;
        public static bool changedState = true;

        public static int selectedLevel = 3;

        enum gamestate {menu, gamePlay, characterSelect, levelSelect, settings, results};
        static gamestate currentGameState = gamestate.menu;

        public static GamePadState[] pads = new GamePadState[4];
        public static GamePadState[] prevGamePadState = new GamePadState[4];
        public static Player[] players = new Player[4];

        public static SoundEffect hammerHitSFX, spearHitSFX, swordHitSFX, maceHitSFX, deathSFX, bellsSFX, jumpSFX, selectSFX, moveSelectSFX,
                                  newCharSFX;
        public static Song bgMusic;

        public static bool SFXEnabled = true;
        public static bool musicEnabled = true;

        Menu menu = new Menu(SCREENWIDTH, SCREENHEIGHT);
        GamePlay game = new GamePlay();
        CharacterSelect charSelect = new CharacterSelect();
        LevelSelect levelSelect = new LevelSelect(SCREENWIDTH, SCREENHEIGHT);
        Settings settings = new Settings();
        Results results = new Results();
        #endregion

        #region Constructor
        public PaisleyRangers()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = SCREENWIDTH;
            graphics.PreferredBackBufferHeight = SCREENHEIGHT;
            graphics.IsFullScreen = FULLSCREEN;
        }
        #endregion

        #region Initialization
        protected override void Initialize()
        {
            hammerHitSFX = Content.Load<SoundEffect>("Sound/hammer");
            swordHitSFX = Content.Load<SoundEffect>("Sound/sword");
            jumpSFX = Content.Load<SoundEffect>("Sound/jump");
            deathSFX = Content.Load<SoundEffect>("Sound/death");
            maceHitSFX = Content.Load<SoundEffect>("Sound/mace");
            spearHitSFX = Content.Load<SoundEffect>("Sound/spear");
            selectSFX = Content.Load<SoundEffect>("Sound/select");
            moveSelectSFX = Content.Load<SoundEffect>("Sound/changedSelect");
            newCharSFX = Content.Load<SoundEffect>("Sound/newChar");
            
            bgMusic = Content.Load<Song>("Sound/main");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(bgMusic);

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent() {}
        #endregion

        #region Public Methods
        //Switch to the correct update method for the corresponding gamestate
        protected override void Update(GameTime gameTime)
        {
            pads[0] = GamePad.GetState(PlayerIndex.One);
            pads[1] = GamePad.GetState(PlayerIndex.Two);
            pads[2] = GamePad.GetState(PlayerIndex.Three);
            pads[3] = GamePad.GetState(PlayerIndex.Four);

            switch (currentGameState)
            {
                case gamestate.menu:
                    if (changedState)
                    {
                        menu.Init(Content);
                        changedState = false;
                    }
                    menu.Update();
                    break;
                case gamestate.gamePlay:
                    if(changedState)
                    {
                        game.Init(Content, SCREENWIDTH, SCREENHEIGHT, players, selectedLevel);
                        changedState = false;
                    }
                    game.Update(gameTime, pads);
                    break;
                case gamestate.characterSelect:
                    if (changedState)
                    {
                        charSelect.Init(Content);
                        changedState = false;
                    }
                    charSelect.Update(gameTime, players);
                    break;
                case gamestate.levelSelect:
                    if (changedState)
                    {
                        levelSelect.Init(Content);
                        changedState = false;
                    }
                    levelSelect.Update();
                    break;
                case gamestate.settings:
                    if (changedState)
                    {
                        settings.Init(Content);
                        changedState = false;
                    }
                    settings.Update();
                    break;
                case gamestate.results:
                    if (changedState)
                    {
                        results.Init(Content, players);
                        changedState = false;
                    }
                    results.Update(gameTime, pads[0]);
                    break;
            }

            prevGamePadState = pads;

            base.Update(gameTime);
        }
        
        //Get the correct draw method for the corresponding gamestate we are in
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(0, 228, 255));
            if (!changedState)
            {
                switch (currentGameState)
                {
                    case gamestate.menu:
                        menu.Draw(spriteBatch);
                        break;
                    case gamestate.gamePlay:
                        game.Draw(spriteBatch);
                        break;
                    case gamestate.characterSelect:
                        charSelect.Draw(spriteBatch, players);
                        break;
                    case gamestate.levelSelect:
                        levelSelect.Draw(spriteBatch);
                        break;
                    case gamestate.settings:
                        settings.Draw(spriteBatch);
                        break;
                    case gamestate.results:
                        results.Draw(spriteBatch);
                        break;
                }
            }

            base.Draw(gameTime);
        }

        /// <summary>
        /// Set the gamestate to the ID provided
        /// </summary>
        /// <param name="stateID">The ID of the state to change to {0 = Menu, 1 = Play Game, 2 = Character Select, 3 = Level Select, 4 = Settings, 5 = Results Screen}</param>
        public static void SetGameState(int stateID)
        {
            changedState = true;

            switch(stateID)
            {
                case 0:
                    currentGameState = gamestate.menu;
                    break;
                case 1:
                    currentGameState = gamestate.gamePlay;
                    break;
                case 2:
                    currentGameState = gamestate.characterSelect;
                    break;
                case 3:
                    currentGameState = gamestate.levelSelect;
                    break;
                case 4:
                    currentGameState = gamestate.settings;
                    break;
                case 5:
                    currentGameState = gamestate.results;
                    break;
            }
        }
        #endregion
    }
}
