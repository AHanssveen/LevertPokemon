using System.Collections.Generic;

namespace PokemonTheRemakez.BattleSystem.Pokemon
{
    public class CombatStat
    {
        public Stats Stat;
        public Dictionary<StatStage, float> StatsStages = new Dictionary<StatStage, float>();
        public StatStage CurrentStage;
        // Handles the stats in combat suchs as evasion and accuracy
        public CombatStat(Stats stat, int value)
        {
            CurrentStage = StatStage.Neutral;
            Stat = stat;

            switch (stat)
            {
                case Stats.Evasion:
                    StatsStages.Add(StatStage.Positive6, value * 3f);
                    StatsStages.Add(StatStage.Positive5, value * 2.66f);
                    StatsStages.Add(StatStage.Positive4, value * 2.33f);
                    StatsStages.Add(StatStage.Positive3, value * 2f);
                    StatsStages.Add(StatStage.Positive2, value * 1.66f);
                    StatsStages.Add(StatStage.Positive1, value * 1.33f);
                    StatsStages.Add(StatStage.Neutral, value);
                    StatsStages.Add(StatStage.Negative1, value * 0.75f);
                    StatsStages.Add(StatStage.Negative2, value * 0.6f);
                    StatsStages.Add(StatStage.Negative3, value * 0.5f);
                    StatsStages.Add(StatStage.Negative4, value * 0.428f);
                    StatsStages.Add(StatStage.Negative5, value * 0.375f);
                    StatsStages.Add(StatStage.Negative6, value * 0.33f);
                    break;

                case Stats.Accuracy:
                    StatsStages.Add(StatStage.Positive6, value * 0.33f);
                    StatsStages.Add(StatStage.Positive5, value * 0.375f);
                    StatsStages.Add(StatStage.Positive4, value * 0.428f);
                    StatsStages.Add(StatStage.Positive3, value * 0.5f);
                    StatsStages.Add(StatStage.Positive2, value * 0.6f);
                    StatsStages.Add(StatStage.Positive1, value * 0.75f);
                    StatsStages.Add(StatStage.Neutral, value);
                    StatsStages.Add(StatStage.Negative1, value * 1.33f);
                    StatsStages.Add(StatStage.Negative2, value * 1.66f);
                    StatsStages.Add(StatStage.Negative3, value * 2f);
                    StatsStages.Add(StatStage.Negative4, value * 2.33f);
                    StatsStages.Add(StatStage.Negative5, value * 2.66f);
                    StatsStages.Add(StatStage.Negative6, value * 3f);
                    break;

                default:
                    StatsStages.Add(StatStage.Positive6, value * 0.25f);
                    StatsStages.Add(StatStage.Positive5, value * 0.285f);
                    StatsStages.Add(StatStage.Positive4, value * 0.33f);
                    StatsStages.Add(StatStage.Positive3, value * 0.4f);
                    StatsStages.Add(StatStage.Positive2, value * 0.5f);
                    StatsStages.Add(StatStage.Positive1, value * 0.66f);
                    StatsStages.Add(StatStage.Neutral, value);
                    StatsStages.Add(StatStage.Negative1, value * 1.5f);
                    StatsStages.Add(StatStage.Negative2, value * 2f);
                    StatsStages.Add(StatStage.Negative3, value * 2.5f);
                    StatsStages.Add(StatStage.Negative4, value * 3f);
                    StatsStages.Add(StatStage.Negative5, value * 3.5f);
                    StatsStages.Add(StatStage.Negative6, value * 4f);
                    break;
            }
        }

        public float CurrentStatValue
        {
            get { return StatsStages[CurrentStage]; }
        }
    }
}
