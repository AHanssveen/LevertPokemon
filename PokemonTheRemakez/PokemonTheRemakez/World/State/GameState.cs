using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace PokemonTheRemakez.World.State
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public abstract class GameState : DrawableGameComponent
    {
        private readonly List<GameComponent> _childComponents;
        private GameState _tag;
        protected GameStateManager StateManager;

        protected GameState(Game game, GameStateManager manager)
            : base(game)
        {
            StateManager = manager;
            _childComponents = new List<GameComponent>();
            _tag = this;
        }

        public List<GameComponent> ChildComponents
        {
            get { return _childComponents; }
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
// ReSharper restore RedundantOverridenMember

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (var component in _childComponents.Where(component => component.Enabled))
            {
                component.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var component in _childComponents.OfType<DrawableGameComponent>().Where(component => component.Visible))
            {
                component.Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        // GameState methods
        // Handles event
        internal protected virtual void StateChange(object sender, EventArgs e)
        {
            if (StateManager.CurrentState == _tag) Show();
            else Hide();
        }

        protected virtual void Show()
        {
            Visible = true;
            Enabled = true;
            foreach (var component in _childComponents)
            {
                component.Enabled = true;
                if (component is DrawableGameComponent) ((DrawableGameComponent) component).Visible = true;
            }
        }

        protected virtual void Hide()
        {
            Visible = false;
            Enabled = false;
            foreach (var component in _childComponents)
            {
                component.Enabled = false;
                if (component is DrawableGameComponent) ((DrawableGameComponent)component).Visible = false;
            }
        }
    }
}
