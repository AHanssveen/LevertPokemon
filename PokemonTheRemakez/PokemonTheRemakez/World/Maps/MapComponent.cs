using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.World.Audio;

namespace PokemonTheRemakez.World.Maps {
    public class MapComponent
    {
        public String Name;
        public Texture2D Texture;
        public Texture2D CollisionTexture;
        public Rectangle Destination;
        public string TrackName;

        public MapComponent(String name, Texture2D texture, Texture2D collisionTexture, Rectangle destination, string trackName, Game1 game)
        {
            Name = name;
            Texture = texture;
            CollisionTexture = collisionTexture;
            Destination = destination;
            TrackName = trackName;                
        }

        /// <summary>
        /// Stops the running music track
        /// </summary>
        public void StopTrack()
        {
            var track = AudioController.RequestTrack(TrackName);
            if (track != null)
            {
                track.Stop();
            }
        }

        /// <summary>
        /// starts the running music track
        /// </summary>
        public void StartTrack()
        {
            var track = AudioController.RequestTrack(TrackName);
            if (track.State == SoundState.Playing)
                track.IsLooped = true;
            track.Play();
        }
    }
}
