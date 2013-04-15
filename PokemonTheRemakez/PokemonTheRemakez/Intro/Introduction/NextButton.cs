using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.BattleSystem;

namespace PokemonTheRemakez.Intro.Introduction
{
    public class NextButton : DrawableBattleComponent
    {
        private bool draw;
        private int _showNextButtonTimer;

        /// <summary>
        /// Loads the textures used
        /// </summary>
        /// <param name="game">provides an instance of game</param>
        public NextButton(Game game) : base(game)
        {
            Texture = game.Content.Load<Texture2D>("next_button");
        }

        /// <summary>
        /// updates the logic used to draw
        /// </summary>
        /// <param name="gameTime">provides a snapshot of the gametime</param>
        public override void Update(GameTime gameTime)
        {
            _showNextButtonTimer += gameTime.ElapsedGameTime.Milliseconds;

            if (_showNextButtonTimer >= 500)
                draw = true;

            else if (_showNextButtonTimer >= 1000)
                _showNextButtonTimer = 0;

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the textures at given locations
        /// </summary>
        /// <param name="spriteBatch">provides an instance of spritebatch</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (draw)
                spriteBatch.Draw(Texture, new Rectangle(420, 360, 60, 30), Color.White);
        }
    }
}
