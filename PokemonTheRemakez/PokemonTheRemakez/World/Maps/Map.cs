using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using PokemonTheRemakez.World.Collisions;
using PokemonTheRemakez.World.Players;
using PokemonTheRemakez.World.Trainers;

namespace PokemonTheRemakez.World.Maps
{
    public class Map : List<MapComponent>
    {
        public Dictionary<string, EnemyTrainer> Trainers; 

        public CollisionDetecter CollisionDetecter = new CollisionDetecter();
        public MapComponent CurrentMapComponent;

        /// <summary>
        /// Finds the point where the maps intersects
        /// </summary>
        /// <param name="point">point where the maps intersect</param>
        /// <returns>where the maps intersect</returns>
        public MapComponent FindIntersectingMapComponent(Point point)
        {
            var checkRectangle = new Rectangle(point.X, point.Y, 1, 1);

            return this.FirstOrDefault(c => c.Destination.Intersects(checkRectangle));
        }

        /// <summary>
        /// Checks for collision
        /// </summary>
        /// <param name="position">current position</param>
        /// <param name="enemyTrainer">position of enemy trainer</param>
        /// <returns>if you can or can not walk there</returns>
        public CollisionType CheckForCollisions(Vector2 position, ref EnemyTrainer enemyTrainer)
        {
            var mapComponent = FindIntersectingMapComponent(new Point((int)position.X, (int)position.Y));
            
            // Tried to move outside the map
            if (mapComponent == null)
                return CollisionType.Unwalkable;

            if (CurrentMapComponent != mapComponent)
            {
                if (CurrentMapComponent == null)
                    CurrentMapComponent = mapComponent;
                else
                {
                    CurrentMapComponent.StopTrack();
                    CurrentMapComponent = mapComponent;
                    CurrentMapComponent.StartTrack(); 
                }                    
            }

            var checkRectangle = new Rectangle((int) position.X, (int) position.Y, 1, 1);

            enemyTrainer = null;

            foreach (var trainer in Trainers.Where(trainer => trainer.Value.CollisionRectangle.Intersects(checkRectangle))) {
                enemyTrainer = trainer.Value;
            }

            var checkPointByMapComponent = new Point((int)(position.X - mapComponent.Destination.X),
                                                    (int)(position.Y - mapComponent.Destination.Y)); 
            return CollisionDetecter.CheckForCollisions(mapComponent.CollisionTexture, checkPointByMapComponent);
        } 
    }
}
