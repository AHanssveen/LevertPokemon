using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.BattleSystem.Pokemon;
using PokemonTheRemakez.BattleSystem.Pokemon.Moves.Combat;
// Handles which abillity the player can use.
namespace PokemonTheRemakez.BattleSystem.Screens.Elements
{
    public class AttackMenu : DrawableBattleComponent
    {
        private readonly AttackChoiceArrow _attackChoiceArrow;

        private readonly SpriteFont _moveFont;
        private readonly SpriteFont _PPFont; 
        
        private PokemonWrapper _pokemon;       
        private string _move1;
        private string _move2;
        private string _move3;
        private string _move4;

        private string _remainingPP;
        private string _maxPP;
        private string _type;

        private Vector2 _movesOffset = Vector2.Zero;
        private Vector2 _remainingPPOffset = Vector2.Zero;

        public AttackMenu(Game game, AttackChoiceArrow attackChoiceArrow, PokemonInstance pokemonInstance) : base(game)
        {
            _pokemon = new PokemonWrapper(game, pokemonInstance);

            _attackChoiceArrow = attackChoiceArrow;
            Position = new Vector2(0, 110);
            Texture = game.Content.Load<Texture2D>("Resources/Battlesystem/attackMenu");
            _moveFont = game.Content.Load<SpriteFont>("Size8");
            _PPFont = game.Content.Load<SpriteFont>("Size10");

        }
        /// <summary>
        /// Draw the name of the abillities.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            // Panel
            base.Draw(spriteBatch);

            // Move names
            if (_move1 != null)
                spriteBatch.DrawString(_moveFont, _move1, new Vector2(20, 120) + _movesOffset, Color.Black);
            if ( _move2 != null )
                spriteBatch.DrawString(_moveFont, _move2, new Vector2(90, 120) + _movesOffset, Color.Black);
            if ( _move3 != null )
                spriteBatch.DrawString(_moveFont, _move3, new Vector2(20, 135) + _movesOffset, Color.Black);
            if ( _move4 != null )
                spriteBatch.DrawString(_moveFont, _move4, new Vector2(90, 135) + _movesOffset, Color.Black);

            // PP
            spriteBatch.DrawString(_PPFont, _remainingPP, new Vector2(199, 118) + _remainingPPOffset, Color.Black);
            spriteBatch.DrawString(_PPFont, _maxPP, new Vector2(220, 118), Color.Black);

            // Type
            spriteBatch.DrawString(_moveFont, _type, new Vector2(192, 136), Color.Black);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            CombatMove combatMove = null;

            switch (_attackChoiceArrow.AttackChoiceArrowPosition)
            {
                case AttackChoiceArrowPosition.Move1:
                    combatMove = _pokemon.PokemonInstance.Moves[0];
                    break;
                case AttackChoiceArrowPosition.Move2:
                    combatMove = _pokemon.PokemonInstance.Moves[1];
                    break;
                case AttackChoiceArrowPosition.Move3:
                    combatMove = _pokemon.PokemonInstance.Moves[2];
                    break;
                case AttackChoiceArrowPosition.Move4:
                    combatMove = _pokemon.PokemonInstance.Moves[3];
                    break;
            }

            if (combatMove != null)
            {
                _maxPP = combatMove.Archetype.MaxPP + "";
                _remainingPP = combatMove.RemainingPP + "";
                if (_remainingPP.Length == 1)
                    _remainingPPOffset = new Vector2(7, 0);
                else
                    _remainingPPOffset = Vector2.Zero;
                _type = combatMove.Archetype.ElementType.ToString();
            }
        }
        /// <summary>
        /// Sets the abilities to the pokemon
        /// </summary>
        /// <param name="activeAlliedPokemon"></param>
        public void SetPokemon(PokemonInstance activeAlliedPokemon)
        {
            _pokemon.PokemonInstance = activeAlliedPokemon;
            _attackChoiceArrow.MoveCount = _pokemon.PokemonInstance.Moves.Count;

            _move1 = "";
            _move2 = "";
            _move3 = "";
            _move4 = "";

            if (_pokemon.PokemonInstance.Moves.Count > 0)
            {
                _move1 = activeAlliedPokemon.Moves[0].Archetype.Name;

                if (_pokemon.PokemonInstance.Moves.Count > 1)
                {
                    _move2 = activeAlliedPokemon.Moves[1].Archetype.Name;

                    if (_pokemon.PokemonInstance.Moves.Count > 2)
                    {
                        _move3 = activeAlliedPokemon.Moves[2].Archetype.Name;

                        if (_pokemon.PokemonInstance.Moves.Count > 3)
                            _move4 = activeAlliedPokemon.Moves[3].Archetype.Name; 
                    }
                }
            }
        }
    }
}
