using System.Collections.Generic;
using System.Linq;

namespace PokemonTheRemakez.BattleSystem.Pokemon.Moves.Combat
{
    /// <summary>
    /// Contains the set of moves a PokemonInstance has currently learned and have available during combat
    /// </summary>
    public class CombatMoveSet : List<CombatMove>
    {
        public int MaximumSize = 4;

        public new bool Add(CombatMove element)
        {
            if (Count == MaximumSize)
                return false;

            base.Add(element);
            return true;
        }

        public void RestorePP()
        {
            foreach (var move in this)
            {
                move.RestorePP();
            }
        }

        public bool Contains(string moveName) {
            return this.Any(move => move.Archetype.Name.Equals(moveName));
        }
    }
}
