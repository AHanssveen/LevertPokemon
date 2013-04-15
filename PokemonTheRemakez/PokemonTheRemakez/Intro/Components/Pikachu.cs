using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.BattleSystem;

namespace PokemonTheRemakez.Intro.Components
{
    public class Pikachu : DrawableBattleComponent
    {
        private Texture2D _pika1;
        private Texture2D _pika2;
        public bool PikachuSprite1;
        private float _picahuAnimationTimer;

        public Pikachu(Game game) : base(game)
        {
            _pika1 = game.Content.Load<Texture2D>("pica_1");
            _pika2 = game.Content.Load<Texture2D>("pica_2");
        }

        /// <summary>
        /// updates the logic used to draw
        /// </summary>
        /// <param name="gameTime">snapshot of gametime</param>
        public override void Update(GameTime gameTime)
        {
            _picahuAnimationTimer += gameTime.ElapsedGameTime.Milliseconds;

            if (_picahuAnimationTimer >= 1000)
                _picahuAnimationTimer = 0;

            else if (_picahuAnimationTimer <= 500)
                PikachuSprite1 = true;
            else
                PikachuSprite1 = false;

            base.Update(gameTime);
        }

        /// <summary>
        /// draws the stuff you need
        /// </summary>
        /// <param name="spriteBatch">instance of spritebatch</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if(PikachuSprite1)
                spriteBatch.Draw(_pika1, new Rectangle(20, 70, 40, 40), Color.White);
            else
                spriteBatch.Draw(_pika2, new Rectangle(20, 70, 40, 40), Color.White);
        }
    }
}
