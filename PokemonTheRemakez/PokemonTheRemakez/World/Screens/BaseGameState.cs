using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.World.State;

namespace PokemonTheRemakez.World.Screens {
    public abstract class BaseGameState : GameState {
        protected Game1 GameRef;
        protected PlayerIndex PlayerIndexInControl;

        protected BaseGameState(Game1 game, GameStateManager manager)
            : base(game, manager) {
            GameRef = game;
            PlayerIndexInControl = PlayerIndex.One;
        }

        protected override void LoadContent() {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            base.Draw(gameTime);
        }
    }
}
