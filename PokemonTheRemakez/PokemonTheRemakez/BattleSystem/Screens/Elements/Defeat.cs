
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.World.Input;

namespace PokemonTheRemakez.BattleSystem.Screens.Elements
{
    class Defeat : DrawableBattleComponent
    {
        private bool _draw;
        public Defeat(Game game)
            : base(game)
        {
            Texture = game.Content.Load<Texture2D>("Defeat");
        }

        public override void Update(GameTime gameTime)
        {
            if (InputHandler.ActionKeyPressed(ActionKey.ConfirmAndInteract, PlayerIndex.One))
            {
                _draw = false;
                //some restart logic
            }

            if (InputHandler.ActionKeyPressed(ActionKey.Exit, PlayerIndex.One))
            {
                _draw = false;
                Game.Exit();
            }
                    
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Rectangle(0, 0, 960, 632), Color.White);
        }
    }
}
