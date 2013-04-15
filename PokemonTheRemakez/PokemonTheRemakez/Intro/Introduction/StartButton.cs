using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.BattleSystem;

namespace PokemonTheRemakez.Intro.Introduction
{
    public class StartButton : DrawableBattleComponent
    {
        private double Timer;
        private bool draw;

        /// <summary>
        /// Loads the textures needed
        /// </summary>
        /// <param name="game">provides an instance of game</param>
        public StartButton(Game game) : base(game)
        {
            Texture = game.Content.Load<Texture2D>("Resources/StartScreen/Press_start");
        }

        /// <summary>
        /// Updates the logic for drawing
        /// </summary>
        /// <param name="spriteBatch">provides a snapshot of the gametime</param>
        public override void Update(GameTime gameTime)
        {
            Timer += gameTime.ElapsedGameTime.Milliseconds;

            if (Timer >= 800)
                Timer = 0;

            else if (Timer >= 500)
                draw = true;
            else
                draw = false;
            
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the textures at given locations
        /// </summary>
        /// <param name="spriteBatch">provides an instance of spritebatch</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if(draw)
                spriteBatch.Draw(Texture, new Rectangle(130, 250, 100, 20), Color.White);
        }
    }
}
