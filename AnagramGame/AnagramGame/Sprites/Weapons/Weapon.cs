using Microsoft.Xna.Framework;

namespace PaisleyRangers.Sprites.Weapons
{
    public class Weapon : Sprite
    {
        #region Declarations
        public int damage = 0;
        public float speed = 1.0f;
        public float knockback = 1.0f;
        public float characterSpeed = 1.0f;
        public bool attacking = false;

        public Vector2 hitboxSize;

        /// <summary>
        /// These are the positions relative to the player that the weapons have to move to
        /// </summary>
        public Vector2[] idlePositions = new Vector2[8] {new Vector2(22,44), new Vector2(22, 44), new Vector2(22, 44), new Vector2(22, 44), new Vector2(22, 44), new Vector2(22, 44), new Vector2(22, 44), new Vector2(22, 44) };
        public Vector2[] idleAttackPositions = new Vector2[8] { new Vector2(22, 44), new Vector2(25,45), new Vector2(28,43), new Vector2(32,42), new Vector2(35,35), new Vector2(34,32), new Vector2(34,39), new Vector2(22, 44) };
        public Vector2[] runPositions = new Vector2[8] { new Vector2(24, 44), new Vector2(25,45), new Vector2(26,46), new Vector2(27,45), new Vector2(26,46), new Vector2(25,45), new Vector2(24,44), new Vector2(23,44) };
        public Vector2[] runAttackPositions = new Vector2[8] { new Vector2(24,44), new Vector2(26,46), new Vector2(31,44), new Vector2(34,42), new Vector2(38,37), new Vector2(36,34), new Vector2(37,42), new Vector2(23,44) };
        public Vector2[] jumpPositions = new Vector2[8] { new Vector2(25,44), new Vector2(25, 44), new Vector2(25, 44), new Vector2(25, 44), new Vector2(25, 44), new Vector2(25, 44), new Vector2(25, 44), new Vector2(25, 44) };
        public Vector2[] jumpAttackPositions = new Vector2[8] { new Vector2(25, 44), new Vector2(26,45), new Vector2(31,43), new Vector2(34,41), new Vector2(38,36), new Vector2(36,33), new Vector2(37,41), new Vector2(25,44) };
        public Vector2[] blockPositions = new Vector2[1] { new Vector2(41, 40)};

        /// <summary>
        /// These are the rotations that the weapons have to change to each frame
        /// </summary>
        public float[] idleRotations = new float[8] { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f };
        public float[] idleAttackRotations = new float[8] { 0.78539835f, 0.78539835f, 0.610865f, 0.471239f, 0.366519f, 0.261799f, 0.959931f, 1.48353f };
        public float[] runRotations = new float[8] { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f };
        public float[] runAttackRotations = new float[8] { 0.78539835f, 0.78539835f, 0.610865f, 0.471239f, 0.366519f, 0.261799f, 0.959931f, 1.48353f };
        public float[] jumpRotations = new float[8] { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f };
        public float[] jumpAttackRotations = new float[8] { 0.78539835f, 0.78539835f, 0.610865f, 0.471239f, 0.366519f, 0.261799f, 0.959931f, 1.48353f };
        public float[] blockRotations = new float[1] { 0.1f };
        #endregion

        #region Constructor
        public Weapon()
        {
            drawDepth = 0;
            colour = Color.White;
            scale = 2f;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Updates the weapon moving, rotating it to the correct position
        /// </summary>
        /// <param name="playerPosition">The current position of the player</param>
        /// <param name="currentFrame">The current frame of the current animation</param>
        /// <param name="animationID">The ID of the current animation</param>
        /// <param name="isFlipped">If the animation is flipped</param>
        public void Update(Vector2 playerPosition, int currentFrame, int animationID, bool isFlipped)
        {
            flipped = isFlipped;
            
            hitboxSize = new Vector2(31, 15);

            if (!flipped)
            {
                hitbox = new Rectangle(20, -10, (int)hitboxSize.X, (int)hitboxSize.X);
            } else
            {
                hitbox = new Rectangle(-30, -10, (int)hitboxSize.X, (int)hitboxSize.X);
            }
            
            //Sets the positions and rotations
            switch (animationID)
            {
                case 0:
                    worldLocation = playerPosition + idlePositions[currentFrame] * 2;
                    
                    if(flipped)
                    {
                        rotation = -idleRotations[currentFrame];
                    }
                    else
                    {
                        rotation = idleRotations[currentFrame];
                    }
                    break;
                case 1:
                    worldLocation = playerPosition + idleAttackPositions[currentFrame] * 2;
                    if(flipped)
                    {
                        rotation = -idleAttackRotations[currentFrame];
                    }
                    else
                    {
                        rotation = idleAttackRotations[currentFrame];
                    }
                    break;
                case 2:
                    worldLocation = playerPosition + runPositions[currentFrame] * 2;
                    if(flipped)
                    {
                        rotation = -runRotations[currentFrame];
                    }
                    else
                    {
                        rotation = runRotations[currentFrame];
                    }
                    break;
                case 3:
                    
                    if(flipped)
                    {
                        worldLocation = playerPosition + runAttackPositions[currentFrame] * 2;
                        rotation = -runAttackRotations[currentFrame];
                    }
                    else
                    {
                        rotation = runAttackRotations[currentFrame];
                        worldLocation = playerPosition + runAttackPositions[currentFrame] * 2;
                    }
                    break;
                case 4:
                    worldLocation = playerPosition + jumpPositions[currentFrame] * 2;
                    if (flipped)
                    {
                        rotation = -jumpRotations[currentFrame];
                    }
                    else
                    {
                        rotation = jumpRotations[currentFrame];
                    }
                    break;
                case 5:
                    worldLocation = playerPosition + jumpAttackPositions[currentFrame] * 2;
                    if (flipped)
                    {
                        rotation = -jumpAttackRotations[currentFrame];
                    }
                    else
                    {
                        rotation = jumpAttackRotations[currentFrame];
                    }
                    break;
                case 6:
                    enabled = false;
                    return;
                case 7:
                    //The way the block works we need to have the image size minus the positions because its further over in the image
                    if (flipped)
                    {
                        worldLocation = playerPosition + (new Vector2(62 - blockPositions[0].X, blockPositions[0].Y)) * 2;
                        rotation = -blockRotations[0];
                    }
                    else
                    {
                        worldLocation = playerPosition + blockPositions[0] * 2;
                        rotation = blockRotations[0];
                    }
                    break;
            }

            if(flipped)
            {
                drawDepth = 0.83f;
            }
            else
            {
                drawDepth = 0.82f;
            }
        }

        /// <summary>
        /// Checks the weapon collision with the target
        /// </summary>
        /// <param name="sprite">The sprite to hit</param>
        /// <param name="attackComingFrom">The location the attack is coming from</param>
        public void checkCollision(Player defending, Player attacking)
        {
            if(hitbox.Intersects(defending.hitbox) && this.attacking)
            {
                defending.Damage(damage);
                defending.Knockback(knockback, attacking.WorldLocation);
                if (defending.health <= 0 && defending.dying == false)
                {
                    defending.dying = true;
                    attacking.score++;
                    defending.score--;
                }
            }
        }
        #endregion
    }
}
