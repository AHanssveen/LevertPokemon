using Microsoft.Xna.Framework;

namespace PokemonTheRemakez.World.Sprites
{
    public enum FrameKey { Idle, RightFoot, LeftFoot }

    public class PlayerAnimation : Animation
    {
        public FrameKey CurrentFrameKey = FrameKey.Idle;

        public int RightFoot = 0;
        public int Idle = 1;
        public int LeftFoot = 2;

        public PlayerAnimation(Rectangle[] frames) : base(frames)
        {            
        }

        /// <summary>
        /// Sets the animation frame
        /// </summary>
        /// <param name="frameKey">sets which frame should start</param>
        public void SetAnimationFrame(FrameKey frameKey)
        {
            switch (frameKey)
            {
                case FrameKey.Idle:
                    CurrentFrame = Idle;
                    break;
                case FrameKey.RightFoot:
                    CurrentFrame = RightFoot;
                    break;
                default:
                    CurrentFrame = LeftFoot;
                    break;
            }

            CurrentFrameKey = frameKey;
        }

        /// <summary>
        /// Clones an animation
        /// </summary>
        /// <returns>the cloned animation</returns>
        public override object Clone()
        {
            var animationClone = new PlayerAnimation(Frames);

            animationClone.FrameWidth = FrameWidth;
            animationClone.FrameHeight = FrameHeight;
            animationClone.Reset();

            return animationClone;
        }
    }
}
