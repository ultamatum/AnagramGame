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
    class LevelSelect
    {
        //Declaration of objects for the level selection screen
        #region Declarations
        private int currentChoice = 0;
        private int screenWidth;
        private int screenHeight;
        private int optionTimer = 20;

        private Texture2D background;
        private Texture2D arenaBattle;
        private Texture2D terraceBattle;
        private Texture2D towerBattle;
        private Texture2D selectionBox;

        private SpriteFont font;

        private string[] levels = new string[] { "TOWER BATTLE", "TERRACE BATTLE", "ARENA BATTLE" };
        #endregion

        #region Constructor
        //Screen width and height
        public LevelSelect(int screenWidth, int screenHeight)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
        }
        #endregion

        #region Public Methods
        //Load all the content thats needed
        public void Init(ContentManager cm)
        {
            arenaBattle = cm.Load<Texture2D>("Menus/Level Select/Arena Battle");
            terraceBattle = cm.Load<Texture2D>("Menus/Level Select/Terrace Battle");
            towerBattle = cm.Load<Texture2D>("Menus/Level Select/Tower Battle");
            background = cm.Load<Texture2D>("Menus/Map Selection Screen");
            font = cm.Load<SpriteFont>("Pixel Font");
            selectionBox = cm.Load<Texture2D>("Menus/Level Select/Current Selection");
        }
        
        public void Update()
        {
            optionTimer--;

            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) && optionTimer <= 0)
            {
                if (PaisleyRangers.SFXEnabled) PaisleyRangers.selectSFX.Play();
                //Switches the currently selected map
                switch (currentChoice)
                {
                    case 0:
                        PaisleyRangers.selectedLevel = 0;
                        break;
                    case 1:
                        PaisleyRangers.selectedLevel = 1;
                        break;
                    case 2:
                        PaisleyRangers.selectedLevel = 2;
                        break;
                }
                PaisleyRangers.SetGameState(1);

                optionTimer = 0;
            }

            //Go back to the character select
            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.B) && optionTimer <= 0)
            {
                PaisleyRangers.SetGameState(2);
            }

            if ((GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X >= 0.3f || GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed) && optionTimer <= 0)
            {
                currentChoice++;
                optionTimer = 20;
            }
            if ((GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X <= -0.3f || GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed) && optionTimer <= 0)
            {
                currentChoice--;
                optionTimer = 20;
            }

            if (currentChoice >= levels.Length) currentChoice = 0;
            else if (currentChoice < 0) currentChoice = levels.Length - 1;
        }
        
        public void Draw(SpriteBatch sb)
        {
            sb.Begin(samplerState: SamplerState.PointClamp);
            sb.Draw(background, Vector2.Zero, Color.White);
            sb.Draw(arenaBattle, new Vector2(1536 - 250, 358), Color.White);
            sb.Draw(terraceBattle, new Vector2(960 - 250, 358), Color.White);
            sb.Draw(towerBattle, new Vector2(384 - 250, 358), Color.White);
            sb.DrawString(font, "TOWER BATTLE", new Vector2(270, 782), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            sb.DrawString(font, "TERRACE BATTLE", new Vector2(830, 782), Color.White, 0f, Vector2.Zero,2f, SpriteEffects.None, 0f);
            sb.DrawString(font, "ARENA BATTLE", new Vector2(1425, 782), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            //Draws the selection indicator
            switch(currentChoice)
            {
                case 0:
                    sb.Draw(selectionBox, new Vector2(84, 308), Color.White);
                    break;
                case 1:
                    sb.Draw(selectionBox, new Vector2(660, 308), Color.White);
                    break;
                case 2:
                    sb.Draw(selectionBox, new Vector2(1236, 308), Color.White);
                    break;
            }
            sb.End();
        }
        #endregion
    }
}
