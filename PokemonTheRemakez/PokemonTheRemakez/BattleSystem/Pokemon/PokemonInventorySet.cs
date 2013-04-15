using System.Collections.Generic;

namespace PokemonTheRemakez.BattleSystem.Pokemon
{
    // Defines which pokemon equipped
    public class PokemonInventorySet : List<PokemonInstance>
    {
        public int MaximumSize = 6;

        public new bool Add(PokemonInstance element)
        {
            if (Count == MaximumSize)
                return false;

            base.Add(element);
            return true;
        }
    }
}
