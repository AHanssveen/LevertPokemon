using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.BattleSystem;

namespace PokemonTheRemakez.World.Elements
{
    public class ExclamationBubble : DrawableBattleComponent
    {
        /// <summary>
        /// Loads textures and position of exclamation bubble
        /// </summary>
        /// <param name="game">instance of game</param>
        /// <param name="x">position x</param>
        /// <param name="y">position y</param>
        public ExclamationBubble(Game game, int x, int y)
            : base(game)
        {
            Position = new Vector2(x, y);
            Texture = game.Content.Load<Texture2D>("Resources/Trainers/ChatBubble");
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset) {
            base.Draw(spriteBatch);
        }
    }
}