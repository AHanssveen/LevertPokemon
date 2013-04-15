using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PokemonTheRemakez.BattleSystem;
using PokemonTheRemakez.World.Input;

namespace PokemonTheRemakez.Intro.Introduction
{
    public class InfoScreen : DrawableBattleComponent
    {
        private Texture2D[] _introTextTextures = new Texture2D[6];
        private double _timer;

        public int IntroTextIndex;
        public bool DisplayNextArrow;

        /// <summary>
        /// loads the textures used
        /// </summary>
        /// <param name="game">provides instance of game</param>
        public InfoScreen(Game game) : base(game)
        {
            _introTextTextures[0] = game.Content.Load<Texture2D>("Resources/IntroText/controlls");
            _introTextTextures[1] = game.Content.Load<Texture2D>("Resources/IntroText/Intro1");
            _introTextTextures[2] = game.Content.Load<Texture2D>("Resources/IntroText/Intro2");
            _introTextTextures[3] = game.Content.Load<Texture2D>("Resources/IntroText/Intro3");
            _introTextTextures[4] = game.Content.Load<Texture2D>("Resources/IntroText/Intro4");
            _introTextTextures[5] = game.Content.Load<Texture2D>("Resources/IntroText/Intro4");
        }

        /// <summary>
        /// updates the logic used to draw
        /// </summary>
        /// <param name="gameTime">snapshot of gametime</param>
        public override void Update(GameTime gameTime)
        {
            WaitingForTextToAppear(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// MAkes you wait for text to appear
        /// </summary>
        /// <param name="gameTime">snapshot of gametime</param>
        private void WaitingForTextToAppear(GameTime gameTime)
        {
            _timer += gameTime.ElapsedGameTime.Milliseconds;
            if (_timer >= 600)
            {
                DisplayNextArrow = true;

                if (InputHandler.ActionKeyPressed(ActionKey.ConfirmAndInteract, PlayerIndex.One))
                {
                    IntroTextIndex++;
                    DisplayNextArrow = false;
                    _timer = 0;
                }
            }
        }

        /// <summary>
        /// Draws what should be drawn
        /// </summary>
        /// <param name="spriteBatch">instance of spritebatch</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_introTextTextures[IntroTextIndex], new Rectangle(10, 10, 512, 426), Color.White);
        }
    }
}
