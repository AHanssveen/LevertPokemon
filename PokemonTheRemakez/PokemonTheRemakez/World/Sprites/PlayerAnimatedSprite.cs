using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonTheRemakez.World.Sprites
{
    public class PlayerAnimatedSprite : AnimatedSprite
    {
        public PlayerAnimatedSprite(Texture2D sprite, Dictionary<AnimationKey, PlayerAnimation> animation)
        {
            Texture = sprite;
            Animations = new Dictionary<AnimationKey, Animation>();

            foreach (AnimationKey key in animation.Keys)
                Animations.Add(key, (Animation)animation[key].Clone());
        }

        /// <summary>
        /// Sets current animation frame
        /// </summary>
        /// <param name="frameKey">what frame you want to set</param>
        public void SetCurrentAnimationFrame(FrameKey frameKey)
        {
            ((PlayerAnimation)Animations[CurrentAnimation]).SetAnimationFrame(frameKey);    
        }

        /// <summary>
        /// gets current animation frame key
        /// </summary>
        /// <returns>returns the current animation key</returns>
        public FrameKey GetCurrentAnimationFrameKey()
        {
            return ((PlayerAnimation) Animations[CurrentAnimation]).CurrentFrameKey;
        }
    }
}
