using PaisleyRangers.Sprites.Animations;
using PaisleyRangers.Sprites.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tile_Engine;

namespace PaisleyRangers.Sprites
{
    public class Player : Sprite
    {
        #region Declarations
        private Vector2 fallSpeed = new Vector2(0, 25);
        private float jumpSpeed = 1000f; //The amount to change the Y velocity by when jumping

        public int selectedWeapon = 0;
        public int score = 0;

        private bool dead = false;
        public bool dying;
        private bool doubleJumped = false;
        public bool isReady = false;
        
        private Texture2D spritesheet;

        public Weapon[] weapon = new Weapon[5];
        
        GamePadState gamePadState;
        GamePadState prevGamePadState;
        #endregion

        #region Properties
        /// <summary>
        /// Returns if the player is dead or not
        /// </summary>
        public bool Dead
        {
            get { return dead; }
            set { dead = value; }
        }

        /// <summary>
        /// Gets / sets the players current colour
        /// </summary>
        public Color Colour
        {
            get { return colour; }
            set { colour = value; }
        }
        #endregion

        #region Constructor
        //Initialises everything needed for the player
        public Player()
        {
            frameWidth = 64;
            frameHeight = 64;
            drawDepth = 0.825f;
            acceleration = 100f;
            scale = 2f;
            collisionRectangle = new Rectangle(23, 19, 24, 44);

            enabled = true;
            codeBasedBlocks = false;
        }

        //Loads all animations and adds them to the dictionary
        public void Init(ContentManager content)
        {
            spritesheet = content.Load<Texture2D>("Player/Spritesheet");

            //Adding all the animations to the dictionary
            #region Animation Dictionary
            animations.Add("idle", new Animation(spritesheet, 64, 64, 8, 0, "idle"));
            animations["idle"].LoopAnimation = true;
            animations["idle"].FrameLength = 0.05f;

            animations.Add("idle attack", new Animation(spritesheet, 64, 64, 8, 1, "idle attack"));
            animations["idle attack"].LoopAnimation = false;
            animations["idle attack"].FrameLength = 0.05f;
            animations["idle attack"].NextAnimation = "idle";

            animations.Add("run", new Animation(spritesheet, 64, 64, 8, 2, "run"));
            animations["run"].LoopAnimation = true;
            animations["run"].FrameLength = 0.02f;

            animations.Add("run attack", new Animation(spritesheet, 64, 64, 8, 3, "run attack"));
            animations["run attack"].LoopAnimation = false;
            animations["run attack"].FrameLength = 0.05f;
            animations["run attack"].NextAnimation = "run";

            animations.Add("jump", new Animation(spritesheet, 64, 64, 8, 4, "jump"));
            animations["jump"].LoopAnimation = false;
            animations["jump"].FrameLength = 0.05f;
            animations["jump"].NextAnimation = "falling";

            animations.Add("falling", new Animation(spritesheet, 64, 64, 1, 4, "falling"));
            animations["falling"].LoopAnimation = false;
            animations["falling"].FrameLength = 0.05f;
            animations["falling"].NextAnimation = "falling";

            animations.Add("jump attack", new Animation(spritesheet, 64, 64, 8, 5, "jump attack"));
            animations["jump attack"].LoopAnimation = false;
            animations["jump attack"].FrameLength = 0.05f;
            animations["jump attack"].NextAnimation = "idle";

            animations.Add("die", new Animation(spritesheet, 64, 64, 8, 6, "die"));
            animations["die"].LoopAnimation = false;
            animations["die"].FrameLength = 0.05f;
            animations["die"].NextAnimation = "idle";

            animations.Add("blocking", new Animation(spritesheet, 64, 64, 1, 7, "blocking"));
            animations["blocking"].LoopAnimation = true;

            PlayAnimation("jump");
            #endregion

            weapon[0] = new Unarmed(content);
            weapon[1] = new Sword(content);
            weapon[2] = new Spear(content);
            weapon[3] = new Mace(content);
            weapon[4] = new WarHammer(content);

            acceleration *= weapon[selectedWeapon].characterSpeed;
        }
        #endregion

        #region Public Methods
        public void Update(GameTime gameTime, GamePadState curGamePadState)
        {
            //Doesn't update position or anything when on character select
            if (!waitingSelection)
            {
                gamePadState = curGamePadState;

                //Doesn't update anything if the player is dead
                if (!Dead)
                {
                    string newAnimation = "idle";
                    weapon[selectedWeapon].attacking = false;
                    
                    //Updates the players movement info and animations
                    #region Movement Checks
                    #region Left and Right
                    if ((gamePadState.ThumbSticks.Left.X < -0.3f) && !blocking)
                    {
                        flipped = true;
                        if(onGround) newAnimation = "run";
                        velocity.X -= acceleration;
                    }

                    if ((gamePadState.ThumbSticks.Left.X > 0.3f) && !blocking)
                    {
                        flipped = false;
                        if (onGround) newAnimation = "run";
                        velocity.X += acceleration;
                    }
                    #endregion

                    #region Jumping
                    if (gamePadState.Buttons.A == ButtonState.Pressed && onGround && !blocking)
                    {
                        if (prevGamePadState.Buttons.A == ButtonState.Released)
                        {
                            velocity.Y = -jumpSpeed;
                            newAnimation = "jump";
                            if (PaisleyRangers.SFXEnabled) PaisleyRangers.jumpSFX.Play();
                        }
                    }

                    if (gamePadState.Buttons.A == ButtonState.Pressed && !doubleJumped && !onGround && !blocking)
                    {
                        if (prevGamePadState.Buttons.A == ButtonState.Released)
                        {
                            velocity.Y = -jumpSpeed;
                            newAnimation = "jump";
                            if (PaisleyRangers.SFXEnabled) PaisleyRangers.jumpSFX.Play();
                            doubleJumped = true;
                        }
                    }

                    if (gamePadState.Buttons.A == ButtonState.Released && !onGround)
                    {
                        if (prevGamePadState.Buttons.A == ButtonState.Pressed)
                        {
                            velocity.Y *= 0.25f;
                        }
                    }

                    if(currentAnimation == "jump")
                    {
                        newAnimation = "jump";
                    }

                    if(velocity.Y > 0 && !onGround)
                    {
                        newAnimation = "falling";
                    }
                    

                    if (onGround && doubleJumped) doubleJumped = false;
                    #endregion

                    #region Attacking
                    if (gamePadState.Buttons.X == ButtonState.Pressed && onGround && currentAnimation == "idle")
                    {
                        if (prevGamePadState.Buttons.X == ButtonState.Released)
                        {
                            newAnimation = "idle attack";
                            weapon[selectedWeapon].attacking = true;
                        }
                    }

                    if (gamePadState.Buttons.X == ButtonState.Pressed && onGround && currentAnimation == "run")
                    {
                        if (prevGamePadState.Buttons.X == ButtonState.Released)
                        {
                            newAnimation = "run attack";
                            weapon[selectedWeapon].attacking = true;
                        }
                    }
                    if (gamePadState.Buttons.X == ButtonState.Pressed && !onGround)
                    {
                        if (prevGamePadState.Buttons.X == ButtonState.Released)
                        {
                            newAnimation = "jump attack";
                            weapon[selectedWeapon].attacking = true;
                        }
                    }

                    if (gamePadState.Buttons.X == ButtonState.Pressed && !onGround)
                    {
                        if (prevGamePadState.Buttons.X == ButtonState.Released)
                        {
                            newAnimation = "jump attack";
                            weapon[selectedWeapon].attacking = true;
                        }
                    }

                    blocking = false;
                    if (gamePadState.Buttons.B == ButtonState.Pressed && onGround)
                    {
                        newAnimation = "blocking";
                        blocking = true;
                    }

                    if (currentAnimation == "idle attack")
                    {
                        newAnimation = "idle attack";
                    }
                    if (currentAnimation == "run attack")
                    {
                        newAnimation = "run attack";
                    }
                    if (currentAnimation == "jump attack")
                    {
                        newAnimation = "jump attack";
                    }

                    if (animations["idle attack"].CurrentFrame > 5 && animations["idle attack"].CurrentFrame < 7) weapon[selectedWeapon].attacking = true;
                    else if(animations["run attack"].CurrentFrame > 5 && animations["run attack"].CurrentFrame < 7) weapon[selectedWeapon].attacking = true;
                    else if(animations["jump attack"].CurrentFrame > 5 && animations["jump attack"].CurrentFrame < 7) weapon[selectedWeapon].attacking = true;
                    else weapon[selectedWeapon].attacking = false;
                    #endregion

                    if (health <= 0)
                    {
                        newAnimation = "die";
                    }
                    if(currentAnimation == "die")
                    {
                        newAnimation = "die";
                    }
                    if(currentAnimation == "die" && animations["die"].CurrentFrame >= 6)
                    {
                        dead = true;
                        dying = false;
                    }
                    #endregion

                    if (newAnimation != currentAnimation)
                    {
                        PlayAnimation(newAnimation);
                    }

                    velocity.X *= 0.85f;

                    prevGamePadState = gamePadState;
                }

                if(velocity.X > 0 && velocity.X <= 0.05)
                {
                    velocity.X = 0;
                }
                if (velocity.X < 0 && velocity.X >= -0.05)
                {
                    velocity.X = 0;
                }
                velocity += fallSpeed;
                RepositionCamera();
            }

            for (int i = 0; i < weapon.Length; i++)
            {
                if (i == selectedWeapon)
                {
                    weapon[i].Enabled = true;
                }
                else
                {
                    weapon[i].Enabled = false;
                }
            }

            drawDepth = 0.825f;

            base.Update(gameTime);
            
            weapon[selectedWeapon].Update(worldLocation, animations[currentAnimation].CurrentFrame, animations[currentAnimation].AnimationID, flipped);
        }

        public override void Draw(SpriteBatch sb)
        {
            weapon[selectedWeapon].Draw(sb);
            base.Draw(sb);
        }
        #endregion

        #region Helper Methods
        //Moves the camera to contain the character on the screen
        private void RepositionCamera()
        {
            int screenLocX = (int)Camera.WorldToScreen(worldLocation).X;

            if(screenLocX > (Camera.ViewPortWidth - Camera.ViewPortWidth / 8))
            {
                Camera.Move(new Vector2(screenLocX - (Camera.ViewPortWidth - Camera.ViewPortWidth / 8), 0));
            }
            if (screenLocX < (Camera.ViewPortWidth / 8))
            {
                Camera.Move(new Vector2(screenLocX - (Camera.ViewPortWidth / 8), 0));
            }
        }

        /// <summary>
        /// Changes if the player is able to move
        /// </summary>
        /// <param name="isPlayerWaiting"></param>
        public void IsWaiting(bool isPlayerWaiting)
        {
            waitingSelection = isPlayerWaiting;
        }

        /// <summary>
        /// Changes if the sprite be flipped
        /// </summary>
        /// <param name="value"></param>
        public void Flip(bool value)
        {
            flipped = value;
        }

        /// <summary>
        /// Changes the current animation
        /// </summary>
        /// <param name="anim">The name of the animation to change to</param>
        public void CurrentAnimation(string anim)
        {
            currentAnimation = anim;
        }
        #endregion
    }
}
