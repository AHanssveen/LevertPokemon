using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace PokemonTheRemakez.World.State
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class GameStateManager : Microsoft.Xna.Framework.GameComponent
    {
        public event EventHandler OnStateChange;

        private Stack<GameState> _gameStates = new Stack<GameState>();

        private const int StartDrawOrder = 5000;
        private const int DrawOrderInc = 100;
        private int _draworder;

        public GameStateManager(Game game)
            : base(game)
        {
            _draworder = StartDrawOrder;
        }

        public GameState CurrentState
        {
            get { return _gameStates.Peek(); }
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        /// <summary>
        /// Remove current state
        /// </summary>
        public void PopState()
        {
            if (_gameStates.Count > 0)
            {
                RemoveState();
                _draworder -= DrawOrderInc;

                if (OnStateChange != null)
                    OnStateChange(this, null);
            }    
        }
        
        /// <summary>
        /// Helper for PopState().
        /// </summary>
        private void RemoveState()
        {
            GameState state = CurrentState;

            // Removes subscription of the state to OnStateChange
            OnStateChange -= state.StateChange;

            Game.Components.Remove(state);
            _gameStates.Pop();
        }

        /// <summary>
        /// Add a new state
        /// </summary>
        /// <param name="newState"></param>
        public void PushState(GameState newState)
        {
            _draworder += DrawOrderInc;
            newState.DrawOrder = _draworder;

            AddState(newState);

            if (OnStateChange != null)
                OnStateChange(this, null);
        }

        /// <summary>
        /// Helper for PushState().
        /// </summary>
        /// <param name="newState"></param>
        private void AddState(GameState newState)
        {
            _gameStates.Push(newState);

            Game.Components.Add(newState);

            // Subscribe the state to OnStateChange.
            OnStateChange += newState.StateChange;
        }

        /// <summary>
        /// Remove all other states and set a new state
        /// </summary>
        /// <param name="newState"></param>
        public void ChangeState(GameState newState)
        {
            while (_gameStates.Count > 0)
                RemoveState();

            newState.DrawOrder = StartDrawOrder;
            _draworder = StartDrawOrder;

            AddState(newState);

            if (OnStateChange != null)
                OnStateChange(this, null);
        }
    }
}
