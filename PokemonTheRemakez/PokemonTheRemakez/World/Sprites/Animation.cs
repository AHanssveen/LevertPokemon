using System;
using Microsoft.Xna.Framework;

namespace PokemonTheRemakez.World.Sprites {
    public enum AnimationKey { Down, Left, Right, Up }

    public class Animation : ICloneable {
        protected readonly Rectangle[] Frames;
        private int _framesPerSecond;
        private TimeSpan _frameLength;
        private TimeSpan _frameTimer;
        private int _currentFrame;
        public int FrameWidth;
        public int FrameHeight;

        private const int DefaultFramesPerSecond = 6;

        public Animation(int frameCount, int frameWidth, int frameHeight, int xOffset, int yOffset) {
            Frames = new Rectangle[frameCount];
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;

            for (int i = 0; i < frameCount; i++) {
                Frames[i] = new Rectangle(
                    xOffset + (frameWidth * i),
                    yOffset,
                    frameWidth,
                    frameHeight);
            }

            FramesPerSecond = DefaultFramesPerSecond;
            Reset();
        }

        public Animation(Animation animation) {
            Frames = animation.Frames;
            FrameWidth = Frames[0].Width;
            FrameHeight = Frames[0].Height;
            FramesPerSecond = DefaultFramesPerSecond;
            Reset();
        }

        public Animation(Rectangle[] frames)
        {
            Frames = frames;
            FrameWidth = frames[0].Width;
            FrameHeight = frames[0].Height;
            FramesPerSecond = DefaultFramesPerSecond;
            Reset();
        }

        public Animation() {}

        /// <summary>
        /// Number of frames per second
        /// </summary>
        public int FramesPerSecond {
            get { return _framesPerSecond; }
            set {
                if (value < 1)
                    _framesPerSecond = 1;
                else if (value > 60)
                    _framesPerSecond = 60;
                else
                    _framesPerSecond = value;

                _frameLength = TimeSpan.FromSeconds(1 / (double)_framesPerSecond);
            }
        }

        public Rectangle CurrentFrameRect {
            get { return Frames[_currentFrame]; }
        }

        public int CurrentFrame {
            get { return _currentFrame; }
            set { _currentFrame = (int)MathHelper.Clamp(value, 0, Frames.Length - 1); }

        }

        /// <summary>
        /// Clones the current animation
        /// </summary>
        /// <returns>clone of the animation</returns>
        public virtual object Clone() {
            var animationClone = new Animation(this);

            animationClone.FrameWidth = FrameWidth;
            animationClone.FrameHeight = FrameHeight;
            animationClone.Reset();

            return animationClone;
        }

        /// <summary>
        /// updates the logic used to draw
        /// </summary>
        /// <param name="gameTime">provides a snapshot of the gametime</param>
        public void Update(GameTime gameTime) {
            _frameTimer += gameTime.ElapsedGameTime;

            if (_frameTimer >= _frameLength) {
                _frameTimer = TimeSpan.Zero;
                _currentFrame = (_currentFrame + 1) % Frames.Length;
            }
        }

        public void Update()
        {
            _currentFrame = (_currentFrame + 1)%Frames.Length;

            // DEBUG
            //Console.WriteLine(CurrentFrame);
        }

        /// <summary>
        /// reserts the current animation frame
        /// </summary>
        public void Reset() {
            _currentFrame = 1;
            _frameTimer = TimeSpan.Zero;
        }
    }
}
