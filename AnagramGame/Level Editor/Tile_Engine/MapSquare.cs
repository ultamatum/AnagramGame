using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tile_Engine
{
    [Serializable]
    public class MapSquare
    {
        #region Declarations
        public int[] LayerTiles = new int[3];       //The amount of layers on the block.
        public string CodeValue = "";               //A value used to know what spawns in this block.
        public bool Passable = true;                //Whether or not the block is passable.
        #endregion

        #region Constructor
        //Sets the values of the square on the map to the correcct values.
        public MapSquare(int background, int interactive, int foreground, string code, bool passable)
        {
            LayerTiles[0] = background;
            LayerTiles[1] = interactive;
            LayerTiles[2] = foreground;
            CodeValue = code;
            Passable = passable;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Toggles the fact a block is passable.
        /// </summary>
        public void TogglePassable()
        {
            Passable = !Passable;
        }
        #endregion

    }
}
