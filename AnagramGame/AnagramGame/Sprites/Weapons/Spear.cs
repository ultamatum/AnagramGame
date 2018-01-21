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
    //Stats for the spear weapon
    class Spear : Weapon
    {
        #region Constructor
        public Spear(ContentManager cm)
        {
            damage = 5;
            speed = 2.0f * 2f;
            knockback = 2000f * 0.75f;

            origin = new Vector2(4, 46);

            animations.Add("default", new Animation(cm.Load<Texture2D>("Player/Spear"), 9, 56, 1, 0, "default"));
            animations["default"].LoopAnimation = true;
            PlayAnimation("default");
        }
        #endregion
    }
}
