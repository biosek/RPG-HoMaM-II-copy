﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HoMM.GameComponents;

namespace HoMM.CharacterClasses
{
    public class AnimatedSprite : ICloneable
    {
        #region Fields and Properties

        //Fieldy
        public Dictionary<AnimationKey, Animation> animations;
        AnimationKey currentAnimation;
        bool isAnimating;
        public Texture2D texture;
        Vector2 position;
        Vector2 velocity;
        float speed = 16f;
        
        //Property
        public AnimationKey CurrentAnimation
        {
            get { return currentAnimation; }
            set { currentAnimation = value; }
        }
        public int CurrentCharacter
        {
            get;
            set;
        }
        public bool IsAnimating
        {
            get { return isAnimating; }
            set { isAnimating = value; }
        }
        public int Width
        {
            get { return animations[currentAnimation].FrameWidth; }
        }
        public int Height
        {
            get { return animations[currentAnimation].FrameHeight; }
        }
        public float Speed
        {
            get { return speed; }
            set { speed = MathHelper.Clamp(speed, 1.0f, 16.0f); }
        }
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
            }
        }
        public Vector2 Velocity
        {
            get { return velocity; }
            set
            {
                velocity = value;
                if (velocity != Vector2.Zero)
                    velocity.Normalize();
            }
        }

        #endregion

        #region Constructors

        public AnimatedSprite() { }
        public AnimatedSprite(Texture2D texture, Dictionary<AnimationKey, Animation> animations)
        {
            this.CurrentAnimation = AnimationKey.Down;
            this.texture = texture;
            this.animations = animations;
        }

        #endregion

        #region Methods

        public void Update(GameTime gameTime)
        {
            //Pokud se ma postava animovat, tak se posle update dannej animaci aby se provedla
            if (isAnimating)
                animations[currentAnimation].Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Vykresleni
            spriteBatch.Draw(texture, position, animations[currentAnimation].CurrentFrameRect, Color.White);
        }

        /// <summary>
        /// Pomoci MathHelperu zakazuje postavicce prekrozeni hranice mapy
        /// </summary>
        public void LockToViewport()
        {
            position.X = MathHelper.Clamp(position.X, 0, Session.BackMap.WidthInPixels - Width);
            position.Y = MathHelper.Clamp(position.Y, 0, Session.BackMap.HeightInPixels - Height);
        }

        /// <summary>
        /// Naklonovani animaci a celeho objektu
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            Dictionary<AnimationKey, Animation> anim = new Dictionary<AnimationKey, Animation>();
            anim.Add(AnimationKey.Down, (Animation)animations[AnimationKey.Down].Clone());
            anim.Add(AnimationKey.Up, (Animation)animations[AnimationKey.Up].Clone());
            anim.Add(AnimationKey.Left, (Animation)animations[AnimationKey.Left].Clone());
            anim.Add(AnimationKey.Right, (Animation)animations[AnimationKey.Right].Clone());

            AnimatedSprite animationClone = new AnimatedSprite(this.texture, anim);
            return animationClone;
        } 

        #endregion
    }
}