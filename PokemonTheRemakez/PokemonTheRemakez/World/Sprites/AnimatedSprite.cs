using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.World.TileEngine;

namespace PokemonTheRemakez.World.Sprites {
    /// <summary>
    /// Contains all the graphic data and controls necessary to animate a single sprite
    /// </summary>
    public class AnimatedSprite {
        public Dictionary<AnimationKey, Animation> Animations;
        private AnimationKey _currentAnimation;
        private bool _isAnimating;

        protected Texture2D Texture;
        private Vector2 _position;
        private Vector2 _velocity;
        private float _speed = 16f;

        public AnimatedSprite(Texture2D sprite, Dictionary<AnimationKey, Animation> animation) {
            Texture = sprite;
            Animations = new Dictionary<AnimationKey, Animation>();

            foreach (AnimationKey key in animation.Keys)
                Animations.Add(key, (Animation)animation[key].Clone());
        }

        public AnimatedSprite()
        {            
        }

        public AnimationKey CurrentAnimation {
            get { return _currentAnimation; }
            set { _currentAnimation = value; }
        }

        public bool IsAnimating {
            get { return _isAnimating; }
            set { _isAnimating = value; }
        }

        public int Width {
            get { return Animations[_currentAnimation].FrameWidth; }
        }

        public int Height {
            get { return Animations[_currentAnimation].FrameHeight; }
        }

        public float Speed {
            get { return _speed; }
            set { _speed = MathHelper.Clamp(value, 1.0f, 16.0f); }
        }

        public Vector2 Position {
            get { return _position; }
            set { _position = value; }
        }

        public Vector2 Velocity {
            get { return _velocity; }
            set {
                _velocity = value;
                if (_velocity != Vector2.Zero)
                    _velocity.Normalize();
            }
        }

        public void Update(GameTime gameTime) {
            if (IsAnimating)
                Animations[_currentAnimation].Update(gameTime);
        }

        public void Update()
        {
            if (IsAnimating)
                Animations[_currentAnimation].Update();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Draw(
                Texture,
                _position,
                Animations[_currentAnimation].CurrentFrameRect,
                Color.White);
        }
    }
}
