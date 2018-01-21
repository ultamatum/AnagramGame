using PaisleyRangers.Sprites.Animations;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaisleyRangers.Sprites.Weapons
{
    //A default weapon so that on the character select screen we can have no weapon being help
    class Unarmed : Weapon
    {
        #region Constructor
        public Unarmed(ContentManager cm)
        {
            damage = 0;
            speed = 1f;
            knockback = 1.0f;

            animations.Add("default", new Animation(cm.Load<Texture2D>("Player/Unarmed"), 11, 44, 1, 0, "default"));
            animations["default"].LoopAnimation = true;
            PlayAnimation("default");
        }
        #endregion
    }
}
