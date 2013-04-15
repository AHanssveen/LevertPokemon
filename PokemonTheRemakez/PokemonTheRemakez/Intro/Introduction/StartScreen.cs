using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.BattleSystem;

namespace PokemonTheRemakez.Intro.Introduction
{
    public class StartScreen : DrawableBattleComponent
    {
        private Texture2D _startBackground;
        private Texture2D _charizardArt;

        /// <summary>
        /// Loads the textures needed
        /// </summary>
        /// <param name="game">provides an instance of game</param>
        public StartScreen(Game game) : base(game)
        {
            _startBackground = game.Content.Load<Texture2D>("Resources/StartScreen/Background");
            _charizardArt = game.Content.Load<Texture2D>("Resources/StartScreen/Charizard");
        }

        /// <summary>
        /// Draws the textures at given locations
        /// </summary>
        /// <param name="spriteBatch">provides an instance of spritebatch</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_startBackground, new Rectangle(10, 10, 512, 426), Color.White);
            spriteBatch.Draw(_charizardArt, new Rectangle(270, 190, 250, 150), Color.White);
        }
    }          
}
