using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tile_Engine
{
    public static class Camera
    {
        #region Declarations
        private static Vector2 position = Vector2.Zero;
        private static Vector2 viewPortSize = Vector2.Zero;
        private static Rectangle worldRectangle = new Rectangle(0, 0, 0, 0);
        #endregion

        #region Properties
        /// <summary>
        /// Returns or sets the camera position
        /// </summary>
        public static Vector2 Position
        {
            get { return position; }
            set
            {
                position = new Vector2(MathHelper.Clamp(value.X, worldRectangle.X, worldRectangle.Width - ViewPortWidth),
                    MathHelper.Clamp(value.Y, worldRectangle.Y, worldRectangle.Height - ViewPortHeight));
            }
        }

        /// <summary>
        /// Returns or sets the size of the world rectangle
        /// </summary>
        public static Rectangle WorldRectangle
        {
            get { return worldRectangle; }
            set { worldRectangle = value; }
        }

        /// <summary>
        /// Returns or sets the width of the viewport
        /// </summary>
        public static int ViewPortWidth
        {
            get { return (int)viewPortSize.X; }
            set { viewPortSize.X = value; }
        }

        /// <summary>
        /// Returns or sets the height of the viewport
        /// </summary>
        public static int ViewPortHeight
        {
            get { return (int)viewPortSize.Y; }
            set { viewPortSize.Y = value; }
        }

        /// <summary>
        /// Returns the viewport as a rectangle
        /// </summary>
        public static Rectangle ViewPort
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, ViewPortWidth, ViewPortHeight);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Move the camera by a specified amount.
        /// </summary>
        /// <param name="offset">Offset to move the camera by</param>
        public static void Move(Vector2 offset)
        {
            Position += offset;
        }

        /// <summary>
        /// Returns if the bounds are within the viewport
        /// </summary>
        /// <param name="bounds">The bounds of the object</param>
        /// <returns></returns>
        public static bool ObjectIsVisible(Rectangle bounds)
        {
            return (ViewPort.Intersects(bounds));
        }

        /// <summary>
        /// Converts a world position to a position on the screen.
        /// </summary>
        /// <param name="worldLocation">The position to convert</param>
        /// <returns></returns>
        public static Vector2 WorldToScreen(Vector2 worldLocation)
        {
            return worldLocation - position;
        }

        /// <summary>
        /// Converts a rectangle's world position to a screen position.
        /// </summary>
        /// <param name="worldRectangle">The rectangle to convert</param>
        /// <returns></returns>
        public static Rectangle WorldToScreen(Rectangle worldRectangle)
        {
            return new Rectangle(worldRectangle.Left - (int)position.X, worldRectangle.Top - (int)position.Y, worldRectangle.Width, worldRectangle.Height);
        }

        /// <summary>
        /// Converts a position on the screen to a position in the world.
        /// </summary>
        /// <param name="screenLocation">The vector to convert</param>
        /// <returns></returns>
        public static Vector2 ScreenToWorld(Vector2 screenLocation)
        {
            return screenLocation + position;
        }

        /// <summary>
        /// Converts a rectangle's screen position to a position in the world.
        /// </summary>
        /// <param name="screenRectangle">The rectangle to convert</param>
        /// <returns></returns>
        public static Rectangle ScreenToWorld(Rectangle screenRectangle)
        {
            return new Rectangle(screenRectangle.Left + (int)position.X, screenRectangle.Top + (int)position.Y, screenRectangle.Width, screenRectangle.Height);
        }
        #endregion
    }
}
