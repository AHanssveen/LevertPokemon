namespace PokemonTheRemakez.BattleSystem.Pokemon.Moves.Combat
{
    //Handles combat states such as PP, xp and moves
    public class CombatMove {
        public int GainedAtLevel;
        public int RemainingPP;
        public MoveArchetype Archetype;

        public CombatMove(MoveArchetype archetype, int gainedAtLevel) {
            GainedAtLevel = gainedAtLevel;
            RemainingPP = archetype.MaxPP;
            Archetype = archetype;
        }
        
        public void RestorePP()
        {
            RemainingPP = Archetype.MaxPP;
        }
    }
}
