using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.World.Input;

namespace PokemonTheRemakez.BattleSystem.Screens.Elements
{
    public class Pause : DrawableBattleComponent
    {
        private readonly SpriteFont _font;

        public Pause(Game game, int x, int y) : base(game)
        {
            _font = Game.Content.Load<SpriteFont>("Size10");
            Position = new Vector2(x, y);
            Offset = new Vector2(0, -100);
            Visible = false;
        }

        /// <summary>
        /// Draws the elements to the screen
        /// </summary>
        /// <param name="spriteBatch">instance of spritebatch</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, "Game paused", Position, Color.White);
        }

        /// <summary>
        /// Draws the elements to the screen
        /// </summary>
        /// <param name="spriteBatch">instance of spritebatch</param>
        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            spriteBatch.DrawString(_font, "Game paused", Position + offset, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
        }
    }
}

