using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.BattleSystem;

namespace PokemonTheRemakez.Intro.Components
{
    public class LogoFlash : DrawableBattleComponent
    {
        private Texture2D _pokeLogo;
        private Texture2D[] LogoFlashArt = new Texture2D[5];
        
        private float _shinyLogoGametime;
        private int _delayForFlash;
        private int _flashArrayIndex = 0;
        private bool _drawLogoFlash;

        /// <summary>
        /// Loads the stuff you need to draw
        /// </summary>
        /// <param name="game">intance of game</param>
        public LogoFlash(Game game): base(game)
        {
            _pokeLogo = game.Content.Load<Texture2D>("Resources/StartScreen/Pokemon_logo");

            LogoFlashArt[0] = game.Content.Load<Texture2D>("Resources/StartScreen/logo_flash_1");
            LogoFlashArt[1] = game.Content.Load<Texture2D>("Resources/StartScreen/logo_flash_2");
            LogoFlashArt[2] = game.Content.Load<Texture2D>("Resources/StartScreen/logo_flash_3");
            LogoFlashArt[3] = game.Content.Load<Texture2D>("Resources/StartScreen/logo_flash_4");
            LogoFlashArt[4] = game.Content.Load<Texture2D>("Resources/StartScreen/logo_flash_5");
        }

        /// <summary>
        /// updates logic used to draw
        /// </summary>
        /// <param name="gameTime">snapshot of gametime</param>
        public override void Update(GameTime gameTime)
        {
            _drawLogoFlash = false;

            _shinyLogoGametime += gameTime.ElapsedGameTime.Milliseconds;

            if (_shinyLogoGametime >= 2000)
            {
                _drawLogoFlash = true;
                _delayForFlash += gameTime.ElapsedGameTime.Milliseconds;

                if (_delayForFlash >= 60)
                {
                    _flashArrayIndex++;
                    _delayForFlash = 0;

                    if (_flashArrayIndex == 5)
                    {
                        _flashArrayIndex = 0;
                        _drawLogoFlash = false;
                    }
                }

                if (_shinyLogoGametime >= 2300)
                    _shinyLogoGametime = 0;
            }
        }

        /// <summary>
        /// draws stuff to the screen
        /// </summary>
        /// <param name="spriteBatch">instance of spritebatch</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_pokeLogo, new Rectangle(60, 120, 220, 100), Color.White);
            if(_drawLogoFlash)
                spriteBatch.Draw(LogoFlashArt[_flashArrayIndex], new Rectangle(60, 120, 220, 100), Color.White);
        }
    }  
}
