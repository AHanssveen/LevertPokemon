using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.World.Trainers;

namespace PokemonTheRemakez.World.Collisions
{
    public enum CollisionType
    {
        Unwalkable,
        Walkable,
        HealingHerb,
        Bush,
        TrainerTriggerBush,
        TrainerTrigger
    }

    public class CollisionDetecter
    {
        private static readonly Color HealingHerb = new Color(255, 0, 0);
        private static readonly Color WalkableTile = new Color(155, 0, 155);
        private static readonly Color BushTile = new Color(0, 40, 255);
        private static readonly Color TrainerTriggerBushTile = new Color(255, 255, 0);
        private static readonly Color TrainerTriggerTile = new Color(255, 254, 0);

        private static readonly Color[] MovementAllowedColors = new[]
            {
                WalkableTile,
                BushTile
            };

        /// <summary>
        /// Tests for colors collision using a bitmap
        /// </summary>
        /// <returns>boolean</returns>
        public bool TestCollision(Texture2D collisionTexture, Point checkPoint, Color color)
        {
            var collisionColors = new Color[collisionTexture.Height * collisionTexture.Width];
            collisionTexture.GetData(collisionColors);

            var checkIndex = checkPoint.X + checkPoint.Y*collisionTexture.Width;

            if (checkIndex > collisionColors.Length - 1) return false;

            var detectedColor = collisionColors[checkIndex];
            
            //Console.WriteLine(detectedColor);

            return detectedColor == color;
        }

        /// <summary>
        /// Checks for collision in the bitmap
        /// </summary>
        /// <param name="collisionTexture">texture you want collision against</param>
        /// <param name="checkPoint">point you want to check for collision</param>
        /// <returns>collision type</returns>
        public CollisionType CheckForCollisions(Texture2D collisionTexture, Point checkPoint)
        {
            var collisionColors = new Color[collisionTexture.Height * collisionTexture.Width];
            collisionTexture.GetData(collisionColors);

            var checkIndex = checkPoint.X + checkPoint.Y * collisionTexture.Width;

            // Index out of bounds
            if (checkIndex > collisionColors.Length - 1)
                return CollisionType.Unwalkable;

            var detectedColor = collisionColors[checkIndex];
            
            if (detectedColor == BushTile)
                return CollisionType.Bush;

            if ( detectedColor == HealingHerb )
                return CollisionType.HealingHerb;

            if (detectedColor == WalkableTile)
                return CollisionType.Walkable;

            if (detectedColor == TrainerTriggerBushTile)
                return CollisionType.TrainerTriggerBush;

            if (detectedColor == TrainerTriggerTile)
                return CollisionType.TrainerTrigger;

            return CollisionType.Unwalkable;
        }
    }
}