using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace PokemonTheRemakez.World.Audio
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class AudioController : GameComponent
    {
        // All available tracks and sounds
        private static readonly Dictionary<string, SoundEffect> SoundEffects = new Dictionary<string, SoundEffect>();

        // All active tracks and sounds
        private static readonly Dictionary<string, SoundEffectInstance> SoundEffectInstances = new Dictionary<string, SoundEffectInstance>();

        /// <summary>
        /// loads sounds needed
        /// </summary>
        /// <param name="game">instance of game</param>
        public AudioController(Game game)
            : base(game)
        {
            SoundEffects.Add("pallet", game.Content.Load<SoundEffect>(@"Audio\pallet"));
            SoundEffects.Add("route1", game.Content.Load<SoundEffect>(@"Audio\route1"));
            SoundEffects.Add("pewter", game.Content.Load<SoundEffect>(@"Audio\pewter"));
            SoundEffects.Add("trainerAppears", game.Content.Load<SoundEffect>(@"Audio\trainerAppears"));
            SoundEffects.Add("battle", game.Content.Load<SoundEffect>(@"Audio\battle"));
            SoundEffects.Add("victory", game.Content.Load<SoundEffect>(@"Audio\victory"));
            SoundEffects.Add("intro", game.Content.Load<SoundEffect>(@"Audio\lugiaSong"));

            // SoundEffects
            SoundEffects.Add("hit", game.Content.Load<SoundEffect>(@"Audio\SoundEffects\hit"));
            SoundEffects.Add("deployPokeBall", game.Content.Load<SoundEffect>(@"Audio\SoundEffects\deployPokeBall"));

            // Pokemon Cries
            SoundEffects.Add("Alakazam", game.Content.Load<SoundEffect>(@"Audio\PokemonCries\Alakazam"));
            SoundEffects.Add("Arcanine", game.Content.Load<SoundEffect>(@"Audio\PokemonCries\Arcanine"));
            SoundEffects.Add("Gyarados", game.Content.Load<SoundEffect>(@"Audio\PokemonCries\Gyarados"));
            SoundEffects.Add("Onix", game.Content.Load<SoundEffect>(@"Audio\PokemonCries\Onix"));
            SoundEffects.Add("Pidgeot", game.Content.Load<SoundEffect>(@"Audio\PokemonCries\Pidgeot"));
            SoundEffects.Add("Pidgey", game.Content.Load<SoundEffect>(@"Audio\PokemonCries\Pidgey"));
            SoundEffects.Add("Rapidash", game.Content.Load<SoundEffect>(@"Audio\PokemonCries\Rapidash"));
            SoundEffects.Add("Rattata", game.Content.Load<SoundEffect>(@"Audio\PokemonCries\Rattata"));
            SoundEffects.Add("Scyther", game.Content.Load<SoundEffect>(@"Audio\PokemonCries\Scyther"));

        }

        /// <summary>
        /// LEts you request a track you want to be played
        /// </summary>
        /// <param name="trackName">name of the track</param>
        /// <returns>the track you wanted</returns>
        public static SoundEffectInstance RequestTrack(string trackName)
        {
            SoundEffectInstance track = null;
            if (!SoundEffectInstances.ContainsKey(trackName))
            {
                if (SoundEffects.ContainsKey(trackName))
                {
                    track = SoundEffects[trackName].CreateInstance();
                    SoundEffectInstances.Add(trackName, track);                   
                }
            }
            else
            {
                track = SoundEffectInstances[trackName];
            }
            return track;
        }

        /// <summary>
        /// Pauses the current playing track
        /// </summary>
        public static void PauseAll()
        {
            foreach (var soundEffectInstance in SoundEffectInstances.Where(soundEffectInstance => soundEffectInstance.Value.State == SoundState.Playing))
            {
                soundEffectInstance.Value.Pause();
            }
        }

        /// <summary>
        /// Resumes all paused tracks
        /// </summary>
        public static void ResumeAllPaused()
        {
            foreach (var soundEffectInstance in SoundEffectInstances.Where(soundEffectInstance => soundEffectInstance.Value.State == SoundState.Paused))
            {
                soundEffectInstance.Value.Play();
            }
        }

        /// <summary>
        /// Restarts all paused
        /// </summary>
        public static void RestartAllPaused()
        {
            foreach (var soundEffectInstance in SoundEffectInstances.Where(soundEffectInstance => soundEffectInstance.Value.State == SoundState.Paused))
            {
                soundEffectInstance.Value.Stop();
               // Console.Write(soundEffectInstance.Value.State);
                soundEffectInstance.Value.Play();
               // Console.WriteLine(":" + soundEffectInstance.Value.State);
            }
        }
    }
}
