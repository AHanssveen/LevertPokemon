using System.Collections.Generic;

namespace PokemonTheRemakez.BattleSystem.Pokemon
{
    // Handles the stats of pokemon in combat, health, attack, defense etc.
    public class PokemonCombatStats : Dictionary<Stats, CombatStat>
    {
        public PokemonCombatStats(IDictionary<Stats, int> stats)
        {
            var stat = Stats.Health;
            Add(stat, new CombatStat(stat, stats[stat]));
            
            stat = Stats.Attack;
            Add(stat, new CombatStat(stat, stats[stat]));

            stat = Stats.Defense;
            Add(stat, new CombatStat(stat, stats[stat]));
            
            stat = Stats.SpecialAttack;
            Add(stat, new CombatStat(stat, stats[stat]));
            
            stat = Stats.SpecialDefense;
            Add(stat, new CombatStat(stat, stats[stat]));

            stat = Stats.Speed;
            Add(stat, new CombatStat(stat, stats[stat]));

            stat = Stats.Accuracy;
            Add(stat, new CombatStat(stat, 100));

            stat = Stats.Evasion;
            Add(stat, new CombatStat(stat, 100));
        }

        public void Reset()
        {
            foreach (var value in Values)
                value.CurrentStage = StatStage.Neutral;  
        }
    }
}
