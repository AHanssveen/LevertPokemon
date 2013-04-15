using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.BattleSystem.Pokemon;
// logic for ally pokemon in combat
namespace PokemonTheRemakez.BattleSystem.Screens.Elements
{
    public class AllyPokemonPanel : DrawableBattleComponent
    {
        public PokemonWrapper Pokemon;
        private SpriteFont _levelFont;
        private SpriteFont _nameFont;

        private Texture2D _elementTexture;
        private Rectangle _genderSource;

        private Vector2 _healthOffset = new Vector2(-5,0);
        private Vector2 _experienceBarLocation = new Vector2(160, 105);
        private Vector2 _healthBarLocation = new Vector2(176, 89);

        public AllyPokemonPanel(Game game) : base(game)
        {
            Pokemon = new PokemonWrapper(game);
            Position = new Vector2(125, 64);
            Texture = game.Content.Load<Texture2D>("Resources/Battlesystem/lvlPokemonBottom");
            _elementTexture = game.Content.Load<Texture2D>("Resources/Battlesystem/elements");
            _levelFont = game.Content.Load<SpriteFont>("Size7");
            _nameFont = game.Content.Load<SpriteFont>("Size8");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Panel
            base.Draw(spriteBatch);

            // Level
            spriteBatch.DrawString(_levelFont, "" + Pokemon.PokemonInstance.Level, new Vector2(218, 75), Color.Black);

            // Name
            spriteBatch.DrawString(_nameFont, "" + Pokemon.PokemonInstance.Nickname, new Vector2(143, 74), Color.Black);

            // Gender
            spriteBatch.Draw(_elementTexture, new Vector2(141 + _nameFont.MeasureString("" + Pokemon.PokemonInstance.Nickname).X, 77), _genderSource, Color.White);

            // HP numbers
            var currentHealthPosition = new Vector2(200, 93);
            var maxHealthPosition = new Vector2(218, 93);

            if (Pokemon.PokemonInstance.CurrentHealth >= 10)
                currentHealthPosition += _healthOffset;
            if (Pokemon.PokemonInstance.CurrentHealth >= 100)
                currentHealthPosition += _healthOffset;

            spriteBatch.DrawString(_levelFont, "" + Pokemon.PokemonInstance.CurrentHealth, currentHealthPosition, Color.Black);
            spriteBatch.DrawString(_levelFont, "/", new Vector2(205, 93), Color.Black);

            if (Pokemon.PokemonInstance.CurrentStats[Stats.Health] >= 10)
                maxHealthPosition += _healthOffset;
            if (Pokemon.PokemonInstance.CurrentStats[Stats.Health] >= 100)
                maxHealthPosition += _healthOffset;

            spriteBatch.DrawString(_levelFont, "" + Pokemon.PokemonInstance.CurrentStats[Stats.Health], maxHealthPosition, Color.Black);

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

            // Experience bar
            var experienceBarLength =
                (int)Math.Ceiling(64 * ((Pokemon.PokemonInstance.Experience + 0.0f) / Pokemon.PokemonInstance.ExperienceRequiredForNextLevel));

            for (int i = 0; i < experienceBarLength; i++)
            {
                spriteBatch.Draw(_elementTexture, _experienceBarLocation + new Vector2(i, 0), new Rectangle(129, 9, 1, 2), Color.White);                
            }

        }
        /// <summary>
        /// Sets which pokemon thats active
        /// </summary>
        /// <param name="activeAlliedPokemon"></param>
        public void SetPokemon(PokemonInstance activeAlliedPokemon)
        {
            Pokemon.PokemonInstance = activeAlliedPokemon;
            if (Pokemon.PokemonInstance.Gender == Gender.Female)
                _genderSource = new Rectangle(282, 166, 8, 12);
            else
                _genderSource = new Rectangle(269, 165, 7, 10);
        }
    }
}
