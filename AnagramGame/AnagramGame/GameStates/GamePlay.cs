using PaisleyRangers.Sprites;
using PaisleyRangers.Tile_Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tile_Engine;

namespace PaisleyRangers.GameStates
{
    class GamePlay
    {
        #region Declarations
        private Player[] players = new Player[4];

        private Vector2[] spawns = new Vector2[4] { new Vector2(380, -200), new Vector2(860, -200), new Vector2(1340, -200), new Vector2(1820, -200) };

        private float gameTimer = 0f;

        private int levelID = 0;
        private int minutes = 0;
        private int seconds = 0;

        private int screenWidth;
        private int screenHeight;
        private Texture2D UIBase, UI1,UI2,UI3, UI4, UIH;
        private SpriteFont font;
        #endregion

        #region Constructor
        public GamePlay() { }
        #endregion

        #region Initialization
        public void Init(ContentManager cm, int sw, int sh, Player[] players, int levelID)
        {
            screenWidth = sw;
            screenHeight = sh;

            //Match length
            minutes = 2;
            seconds = 0;

            this.players = players;
            this.levelID = levelID;

            TileMap.Initialize(cm.Load<Texture2D>("Tileset"));
            UIBase = cm.Load<Texture2D>("IngameUI/UI_Base");
            UI1 = cm.Load<Texture2D>("IngameUI/UI_P1");
            UI2 = cm.Load<Texture2D>("IngameUI/UI_P2");
            UI3 = cm.Load<Texture2D>("IngameUI/UI_P3");
            UI4 = cm.Load<Texture2D>("IngameUI/UI_P4");
            UIH = cm.Load<Texture2D>("IngameUI/UI_Health");

            font = cm.Load<SpriteFont>("Pixel Font");

            //World initialization
            Camera.WorldRectangle = new Rectangle(0, 0, TileMap.MapWidth * TileMap.TileWidth, TileMap.MapHeight * TileMap.TileHeight);
            Camera.ViewPortWidth = 1920;
            Camera.ViewPortHeight = 1080;

            LevelManager.Initialize(cm);
            LevelManager.LoadLevel(levelID, cm);

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].Enabled) RespawnPlayer(i);
            }
        }
        #endregion

        #region Public Methods
        public void Update(GameTime gameTime, GamePadState[] padStates)
        {
            #region Timer Updates
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            gameTimer += elapsed;

            //If 1 second has elapsed lower the timer
            if(gameTimer >= 1f)
            {
                seconds--;
                if(seconds < 0)
                {
                    minutes--;
                    seconds = 59;
                }
                gameTimer = 0;
            }

            //When the timer reaches 0 switch to the results screen
            if (minutes <= 0 && seconds <= 0)
            {
                PaisleyRangers.SetGameState(5);
            }
            #endregion

            #region Player Updates
            for (int i = 0; i < players.Length; i++)
            {
                players[i].Update(gameTime, padStates[i]);
                for(int x = 0; x < players.Length; x++)
                {
                    if (x != i && players[x].dying == false && players[i].Enabled && players[x].Enabled)
                    {
                        players[i].weapon[players[i].selectedWeapon].checkCollision(players[x], players[i]);
                    }
                }
                if(players[i].Dead && players[i].dying == false)
                {
                    RespawnPlayer(i);
                }
            }
            #endregion
        }


        public void Draw(SpriteBatch sp)
        {
            sp.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);
            TileMap.Draw(sp);
            for (int i = 0; i < players.Length; i++)
            {
                if(players[i].Enabled)
                {
                    players[i].Draw(sp);
                }
            }

            sp.DrawString(font, minutes + ":" + seconds.ToString().PadLeft(2, '0'), new Vector2(screenWidth / 2 - ((font.MeasureString(minutes + ":" + seconds).X * 7) / 2), 10), Color.White, 0, Vector2.Zero, 7, SpriteEffects.None, 0f);

            //Draw UI
            #region UI Draw
            Vector2 UIP1 = new Vector2(10, 10);
            Vector2 UIP2 = new Vector2(screenWidth - 280, 10);
            Vector2 UIP3 = new Vector2(10, screenHeight - 84);
            Vector2 UIP4 = new Vector2(screenWidth - 280, screenHeight - 84);
            
            sp.Draw(UI1, UIP1, null, null, null, 0, null, players[0].Colour, SpriteEffects.None, 0f);
            sp.Draw(UIH, new Rectangle((int)UIP1.X + 148, (int)UIP1.Y +36, 100 * players[0].health / 100, 24), Color.White);
            sp.Draw(UIBase, UIP1, null, null, null, 0, null, Color.White, SpriteEffects.None, 0.1f);
            if (players[1].Enabled)
            {
                sp.Draw(UI2, UIP2, null, null, null, 0, null, players[1].Colour, SpriteEffects.None, 0f);
                sp.Draw(UIH, new Rectangle((int)UIP2.X + 148, (int)UIP2.Y + 36, 100 * players[1].health / 100, 24), Color.White);
                sp.Draw(UIBase, UIP2, null, null, null, 0, null, Color.White, SpriteEffects.None, 0.1f);
            }
            if (players[2].Enabled)
            {
                sp.Draw(UI3, UIP3, null, null, null, 0, null, players[2].Colour, SpriteEffects.None, 0f);
                sp.Draw(UIH, new Rectangle((int)UIP3.X + 148, (int)UIP3.Y + 36, 100 * players[2].health / 100, 24), Color.White);
                sp.Draw(UIBase, UIP3, null, null, null, 0, null, Color.White, SpriteEffects.None, 0.1f);
            }
            if (players[3].Enabled)
            {
                sp.Draw(UI4, UIP4, null, null, null, 0, null, players[3].Colour, SpriteEffects.None, 0f);
                sp.Draw(UIH, new Rectangle((int)UIP4.X + 148, (int)UIP4.Y + 36, 100 * players[3].health / 100, 24), Color.White);
                sp.Draw(UIBase, UIP4, null, null, null, 0, null, Color.White, SpriteEffects.None, 0.1f);
            }
            #endregion
            sp.End();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Respawns player to their initial spawn location with full health
        /// </summary>
        /// <param name="i">The player ID</param>
        private void RespawnPlayer(int i)
        {
            if(i == 3)
            {
                players[i].WorldLocation = spawns[2];
            }
            else
            {

                players[i].WorldLocation = spawns[i];
            }
            players[i].Dead = false;
            players[i].PlayAnimation("idle");
            players[i].health = 100;
        }
        #endregion
    }
}
