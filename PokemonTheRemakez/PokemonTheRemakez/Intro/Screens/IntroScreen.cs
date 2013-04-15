using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using PokemonTheRemakez.Intro.Introduction;
using PokemonTheRemakez.World.Audio;
using PokemonTheRemakez.World.Screens;
using PokemonTheRemakez.World.State;

namespace PokemonTheRemakez.Intro.Screens
{
    public class IntroScreen : BaseGameState
    {
        private Game _game;
        private IntroController _intro;
        private bool _introFlaggedForRemoval;

        public IntroScreen(Game1 game, GameStateManager manager) : base(game, manager)
        {
            _game = game;
        }

        /// <summary>
        /// Initializes the intro by making it ready to run and runs it
        /// </summary>
        /// <returns>returns an instance of game</returns>
        public IntroScreen InitializeIntro()
        {
            _intro = new IntroController(_game);
            _intro.Exit += EndIntro;
            ChildComponents.Add(_intro);
            StateManager.PushState(this);
            AudioController.PauseAll();
            AudioController.RequestTrack("intro");

            return this;
        }

        /// <summary>
        /// Ends the introduction part
        /// </summary>
        /// <param name="sender">Takes an object argument</param>
        /// <param name="e">Takes an EventArgs argument</param>
        private void EndIntro(object sender, EventArgs e)
        {
            _introFlaggedForRemoval = true;
        }

        /// <summary>
        /// Updates the game logic
        /// </summary>
        /// <param name="gameTime">provides a snapshot of the game logic</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(_introFlaggedForRemoval)
            {
                StateManager.PopState();
                ChildComponents.Remove(_intro);
                AudioController.RequestTrack("intro").Stop();
                if (AudioController.RequestTrack("pallet").State == SoundState.Playing)
                    AudioController.RequestTrack("pallet").IsLooped = true;
                AudioController.RequestTrack("pallet").Play(); 
                _intro = null;
                _introFlaggedForRemoval = false;
            }
        }
    }
}
