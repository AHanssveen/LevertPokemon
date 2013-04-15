using System;
using Microsoft.Xna.Framework;
using PokemonTheRemakez.BattleSystem.Combat;
using PokemonTheRemakez.BattleSystem.Trainers;
using PokemonTheRemakez.World.Audio;
using PokemonTheRemakez.World.Screens;
using PokemonTheRemakez.World.State;

namespace PokemonTheRemakez.BattleSystem.Screens
{
    public class BattleScreen : BaseGameState
    {
        private Battle _battle;
        private bool _battleFlaggedForRemoval;

        public BattleScreen(Game1 game, GameStateManager manager) : base(game, manager)
        {      
        }

        /// <summary>
        /// Initialization of variables used in battles
        /// </summary>
        /// <param name="alliedTrainer">instance of an allied trainer</param>
        /// <param name="trainer">instance of an enemy trainer</param>
        /// <returns>returns instance of game</returns>
        public BattleScreen InitializeBattle(Trainer alliedTrainer, Trainer trainer)
        {
            _battle = new Battle(GameRef, alliedTrainer, trainer, true);
            _battle.Exit += EndBattle;
            ChildComponents.Add(_battle);
            StateManager.PushState(this);
            AudioController.PauseAll();
            AudioController.RequestTrack("battle").Play();

            return this;
        }

        /// <summary>
        /// Ends the battle
        /// </summary>
        /// <param name="sender">instance of an object</param>
        /// <param name="e">instance of an EventArgs</param>
        private void EndBattle(object sender, EventArgs e)
        {
            _battleFlaggedForRemoval = true;
        }

        /// <summary>
        /// updates variables used for the drawing logic
        /// </summary>
        /// <param name="gameTime">provides a snapshot of the gametime</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_battleFlaggedForRemoval)
            {
                StateManager.PopState();
                ChildComponents.Remove(_battle);
                AudioController.RequestTrack("victory").Stop();
                AudioController.RestartAllPaused();
                _battle = null;
                _battleFlaggedForRemoval = false;
            }
        }
    }
}
