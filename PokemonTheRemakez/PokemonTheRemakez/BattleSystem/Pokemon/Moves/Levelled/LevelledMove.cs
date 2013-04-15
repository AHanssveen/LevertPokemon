namespace PokemonTheRemakez.BattleSystem.Pokemon.Moves.Levelled
{
    /// <summary>
    /// Container class for a move and the required level to learn it
    /// </summary>
    public class LevelledMove
    {
        public int LevelRequired;
        public MoveArchetype Archetype;

        public LevelledMove(int levelRequired, MoveArchetype archetype)
        {
            LevelRequired = levelRequired;
            Archetype = archetype;
        }
    }
}
