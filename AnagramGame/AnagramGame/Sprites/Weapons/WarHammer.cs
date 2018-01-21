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
    //Stats for the War Hammer weapon
    class WarHammer : Weapon
    {
        #region Constructor
        public WarHammer(ContentManager cm)
        {
            damage = 9;
            speed = 2.0f * 0.5f;
            knockback = 2000f * 1f;
            characterSpeed *= 0.6f;

            origin = new Vector2(15, 38);
            
            hitboxSize = new Vector2(31, 15);

            animations.Add("default", new Animation(cm.Load<Texture2D>("Player/WarHammer"), 31, 51, 1, 0, "default"));
            animations["default"].LoopAnimation = true;
            PlayAnimation("default");
        }
        #endregion
    }
}
