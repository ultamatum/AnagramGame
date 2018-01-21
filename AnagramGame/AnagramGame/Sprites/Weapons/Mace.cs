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
    //Stats for the mace weapon
    class Mace : Weapon
    {
        #region Constructor
        public Mace(ContentManager cm)
        {
            damage = 8;
            speed = 2f * 1.4f;
            knockback = 2000f * 1.5f;
            characterSpeed *= 1.25f;

            origin = new Vector2(7, 33);

            animations.Add("default", new Animation(cm.Load<Texture2D>("Player/Mace"), 15, 40, 1, 0, "default"));
            animations["default"].LoopAnimation = true;
            PlayAnimation("default");
        }
        #endregion
    }
}
