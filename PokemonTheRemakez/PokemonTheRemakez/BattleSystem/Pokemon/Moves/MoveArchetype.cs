namespace PokemonTheRemakez.BattleSystem.Pokemon.Moves
{
    // handles the different variables for abilities
    public enum DurationType
    {
        SingleFire,
        Persistent,
        Temporary
    }

    public enum BattleEffectStageEffect
    {
        None,
        One,
        Two,
        Three
    }

    public enum EffectType
    {
        Recoil,
        LowerStat,
        IncreaseStat,
        ResetStat,
        None
    }


    public enum EffectStart
    {
        Immediate,
        Delayed
    }

    public enum MoveTarget
    {
        ActiveEnemy,
        ActiveAlly, 
        Battlefield,
        AlliedInventory,
        EnemyInventory,
        Self
    }

    public enum BattleEffectType
    {
        Positive,
        Negative,
        DamageOnly
    }

    public enum MoveType { Physical, Special, Other }
    public enum ElementType { Bug, Dark, Dragon, Electric, Fight, Fire, Flying, Ghost, Grass, Ground, Ice, Normal, Poision, Psychic, Rock, Steel, Water, None }


    /// <summary>
    /// Contains all information about a PokemonInstance combat move
    /// </summary>
    public class MoveArchetype
    {
        public string Name;
        public MoveType MoveType;
        public ElementType ElementType;
        public int MaxPP;
        public int Power;
        public int Accuracy;
        public int TurnDuration;
        public BattleEffectStageEffect BattleEffectStageEffect;
        public EffectType EffectType;
        public EffectStart EffectStart;
        public MoveTarget MoveTarget;
        public DurationType DurationType;
        public BattleEffectType BattleEffectType;
        public string BattleEffectText;


        public MoveArchetype(string name, MoveType moveType, ElementType elementType,
            int maxPp, int power, int accuracy, DurationType durationType, int turnDuration, BattleEffectStageEffect battleEffectStageEffect, 
            EffectType effectType, EffectStart effectStart, MoveTarget moveTarget, 
            BattleEffectType battleEffectType, string battleEffectText)
        {
            Name = name;
            MoveType = moveType;
            ElementType = elementType;
            MaxPP = maxPp;
            Power = power;
            Accuracy = accuracy;
            TurnDuration = turnDuration;
            BattleEffectStageEffect = battleEffectStageEffect;
            EffectType = effectType;
            EffectStart = effectStart;
            MoveTarget = moveTarget;
            DurationType = durationType;
            BattleEffectType = battleEffectType;
            BattleEffectText = battleEffectText;
        }
    }
}
