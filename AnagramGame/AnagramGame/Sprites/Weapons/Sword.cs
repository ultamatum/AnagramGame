using PaisleyRangers.Sprites.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaisleyRangers.Sprites.Weapons
{
    //Stats for the sword weapon
    class Sword : Weapon
    {
        #region Constructor
        public Sword(ContentManager cm)
        {
            damage = 6;
            speed = 2.0f * 1.25f;
            knockback = 2000f * 1f;
            characterSpeed *= 0.8f;

            origin = new Vector2(3, 37);

            animations.Add("default", new Animation(cm.Load<Texture2D>("Player/Sword"), 11, 44, 1, 0, "default"));
            animations["default"].LoopAnimation = true;
            PlayAnimation("default");
        }
        #endregion
    }
}
