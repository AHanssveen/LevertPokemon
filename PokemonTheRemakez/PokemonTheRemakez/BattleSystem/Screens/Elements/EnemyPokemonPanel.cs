using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.BattleSystem.Pokemon;

namespace PokemonTheRemakez.BattleSystem.Screens.Elements
{
    public class EnemyPokemonPanel : DrawableBattleComponent
    {
        public PokemonWrapper Pokemon;
        private SpriteFont _levelFont;
        private SpriteFont _nameFont;

        private Texture2D _elementTexture;
        private Rectangle _genderSource;

        private Vector2 _healthBarLocation = new Vector2(51,29);

        public EnemyPokemonPanel(Game game) : base(game)
        {
            Pokemon = new PokemonWrapper(game);
            Position = new Vector2(10, 10);
            Texture = game.Content.Load<Texture2D>("Resources/Battlesystem/lvlPokemonTop");
            _elementTexture = game.Content.Load<Texture2D>("Resources/Battlesystem/elements");
            _levelFont = game.Content.Load<SpriteFont>("Size7");
            _nameFont = game.Content.Load<SpriteFont>("Size8");
        }

        /// <summary>
        /// Draws the elements to the screen
        /// </summary>
        /// <param name="spriteBatch">instance of spritebatch</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            // Panel
            base.Draw(spriteBatch);

            // Level
            spriteBatch.DrawString(_levelFont, "" + Pokemon.PokemonInstance.Level, new Vector2(93, 15), Color.Black);

            // Name
            spriteBatch.DrawString(_nameFont, "" + Pokemon.PokemonInstance.Nickname, new Vector2(18, 13), Color.Black);

            // Gender
            spriteBatch.Draw(_elementTexture, new Vector2(16 + _nameFont.MeasureString("" + Pokemon.PokemonInstance.Nickname).X, 16), _genderSource, Color.White);

            // HP bar
            // Black background
            for (int i = 0; i < 48; i++)
                spriteBatch.Draw(_elementTexture, _healthBarLocation + new Vector2(i, 0), new Rectangle(117, 21, 1, 3), Color.White);

            var hpRectangle = new Rectangle(117, 9, 1, 3);

            if ((Pokemon.PokemonInstance.CurrentHealth + 0f) / Pokemon.PokemonInstance.CurrentStats[Stats.Health] < 0.5f)
                hpRectangle = new Rectangle(117, 13, 1, 3);
            if ((Pokemon.PokemonInstance.CurrentHealth + 0f) / Pokemon.PokemonInstance.CurrentStats[Stats.Health] < 0.25f)
                hpRectangle = new Rectangle(117, 17, 1, 3);

            var hpBarLength = (int)Math.Ceiling(48 * ((Pokemon.PokemonInstance.CurrentHealth + 0.0) / Pokemon.PokemonInstance.CurrentStats[Stats.Health]));

            for (int i = 0; i < hpBarLength; i++)
                spriteBatch.Draw(_elementTexture, _healthBarLocation + new Vector2(i, 0), hpRectangle, Color.White);    
        }

        /// <summary>
        /// Sets a rectangle with different size over female/male
        /// </summary>
        /// <param name="pokemon">instance of a pokemon</param>
        public void SetPokemon(PokemonInstance pokemon)
        {
            Pokemon.PokemonInstance = pokemon;
            if (Pokemon.PokemonInstance.Gender == Gender.Female)
                _genderSource = new Rectangle(282, 166, 8, 12);
            else
                _genderSource = new Rectangle(269, 165, 7, 10);
        }
    }
}
