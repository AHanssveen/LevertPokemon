using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.BattleSystem.Trainers;

namespace PokemonTheRemakez.BattleSystem.Screens.Elements
{
    public class PokeballsEnemy : DrawableBattleComponent
    {
        private readonly Texture2D _elementsTexture;
        private readonly Trainer _trainer;

        private readonly Vector2 _firstBallOffset = new Vector2(76, 0);
        private readonly Vector2 _ballOffset = new Vector2(-10, 0);

        public PokeballsEnemy(Game game, Trainer trainer) : base(game)
        {
            _trainer = trainer;
            Position = new Vector2(0, 24);
            _elementsTexture = game.Content.Load<Texture2D>("Resources/Battlesystem/elements");
        }

        /// <summary>
        /// Draws the elements to the screen
        /// </summary>
        /// <param name="spriteBatch">instance of spritebatch</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_elementsTexture, Position, new Rectangle(178, 66, 104, 12), Color.White);

            for (int i = 0; i < _trainer.PokemonSet.Count; i++)
                spriteBatch.Draw(_elementsTexture, Position + _firstBallOffset + i * _ballOffset, new Rectangle(133, 65, 7, 7), Color.White);    

        }
    }
}
