using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.BattleSystem.Pokemon.Experience;
using PokemonTheRemakez.BattleSystem.Pokemon.Moves;
using PokemonTheRemakez.BattleSystem.Pokemon.Moves.Levelled;

namespace PokemonTheRemakez.BattleSystem.Pokemon
{
    public enum Gender { None, Male, Female }

    /// <summary>
    /// Contains all information for all Pokemon of one type, e.g. all Pokemon of type "Bulbasaur"
    /// </summary>
    public class PokemonArchetype
    {
        public string Name;
        public int PokemonNumber;
        public string PokedexText;
        public ElementType PrimaryType;
        public ElementType SecondaryType;

        // Evolution
        public bool Evolves;
        public int EvolvesAtLevel;
        public int NextEvolutionPokemonNumber;

        // Moves
        public Dictionary<int, MoveArchetype> LevelledMoves = new Dictionary<int, MoveArchetype>();
        //public List<MoveArchetype> LearnableMoves = new List<MoveArchetype>();

        // Base Stats
        public Dictionary<Stats, int> BaseStats = new Dictionary<Stats, int>();
        public Dictionary<Stats, int> MaxStats = new Dictionary<Stats, int>();

        // Gender restrictions
        public Gender RestrictedGender;

        // Rendering       
        public readonly Texture2D Texture;
        public readonly Rectangle SourceRectangleBack;
        public readonly Rectangle SourceRectangleFront;
        public readonly Rectangle SourceRectangleInventory1;
        public readonly Rectangle SourceRectangleInventory2;

        // Other
        public ExperienceGroup ExperienceGroup;

        public PokemonArchetype(string name, int pokemonNumber, ElementType primaryType, ElementType secondaryType, string pokedexText, bool evolves, 
            int nextEvolutionPokemonNumber, int baseHealth, int baseAttack, int baseDefense,
            int baseSpecialAttack, int baseSpecialDefense, int baseSpeed, int maxHealth, int maxAttack, int maxDefense,
            int maxSpecialAttack, int maxSpecialDefense, int maxSpeed, Texture2D texture, Rectangle sourceRectangleBack,
            Rectangle sourceRectangleFront, Rectangle sourceRectangleInventory1, Rectangle sourceRectangleInventory2,
            ExperienceGroup experienceGroup, Dictionary<int, MoveArchetype> levelledMoves, Gender restrictedGender)
        {
            Name = name;
            PokemonNumber = pokemonNumber;
            PrimaryType = primaryType;
            SecondaryType = secondaryType;
            PokedexText = pokedexText;
            Evolves = evolves;
            NextEvolutionPokemonNumber = nextEvolutionPokemonNumber;
            BaseStats.Add(Stats.Health, baseHealth);
            BaseStats.Add(Stats.Defense, baseDefense);
            BaseStats.Add(Stats.Attack, baseAttack);
            BaseStats.Add(Stats.SpecialAttack, baseSpecialAttack);
            BaseStats.Add(Stats.SpecialDefense, baseSpecialDefense);
            BaseStats.Add(Stats.Speed, baseSpeed);
            MaxStats.Add(Stats.Health, maxHealth);
            MaxStats.Add(Stats.Defense, maxDefense);
            MaxStats.Add(Stats.Attack, maxAttack);
            MaxStats.Add(Stats.SpecialAttack, maxSpecialAttack);
            MaxStats.Add(Stats.SpecialDefense, maxSpecialDefense);
            MaxStats.Add(Stats.Speed, maxSpeed);
            Texture = texture;
            SourceRectangleBack = sourceRectangleBack;
            SourceRectangleFront = sourceRectangleFront;
            SourceRectangleInventory1 = sourceRectangleInventory1;
            SourceRectangleInventory2 = sourceRectangleInventory2;
            ExperienceGroup = experienceGroup;
            LevelledMoves = levelledMoves;
            RestrictedGender = restrictedGender;
        }
    }
}
