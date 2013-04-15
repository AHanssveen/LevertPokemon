using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.BattleSystem.Pokemon.Experience;
using PokemonTheRemakez.BattleSystem.Pokemon.Moves;
using PokemonTheRemakez.BattleSystem.Pokemon.Moves.Combat;
using PokemonTheRemakez.BattleSystem.Trainers;
// Defines the instance of an pokemon. Varies for each trainer etc.
namespace PokemonTheRemakez.BattleSystem.Pokemon
{
    public enum Stats
    {
        Health,
        Attack,
        Defense,
        SpecialAttack,
        SpecialDefense,
        Speed,
        Accuracy,
        Evasion
    }

    public enum StatStage
    {
        Neutral,
        Negative1,
        Negative2,
        Negative3,
        Negative4,
        Negative5,
        Negative6,
        Positive1,
        Positive2,
        Positive3,
        Positive4,
        Positive5,
        Positive6
    }

    /// <summary>
    /// Status ailments that persist until a PokemonInstance is healed at a PokemonInstance Center, after a specific curative item is used, or after a certain amount of turns during battle.
    /// 
    /// Can only have one.
    /// </summary>
    public enum NonVolatileStatus { None, Burn, Freeze, Paralysis, Poison, BadPoison, Sleep }

    /// <summary>
    /// Status ailments that wear off after a PokemonInstance is taken out of battle, the battle ends, or after a certain amount of turns during battle
    /// 
    /// Can have many.
    /// </summary>
    public enum VolatileStatus {
        AquaRing, 
        Bracing, 
        CenterOfAttention, 
        Confusion, 
        Curse, 
        DefenseCurl, 
        Embargo, 
        Encore, 
        Flinch, 
        FocusEnergy, 
        Glowing, 
        HealBlock, 
        Identification, 
        Infatuation, 
        MagicCoat, 
        MagneticLevitation, 
        Minimize, 
        Nightmare, 
        PartiallyTrapped, 
        PerishSong, 
        Protection, 
        Recharging, 
        Rooting, 
        Seeding, 
        SemiInvulnerable, 
        Substitute, 
        TakingAim, 
        TakingInSunlight, 
        Taunt,
        TelekineticLevitation, 
        Torment, 
        Trapped, 
        Withdrawing, 
        WhippingUpAWhirlwind 
    }

    public enum AttackAnimationState
    {
        Ready,
        Starting,
        Ending,
        Ended
    }

    /// <summary>
    /// Contains all information about a single instance of a PokemonInstance archetype, e.g. the PokemonInstance of type "Bulbasaur" named "Godzilla"
    /// </summary>
    public class PokemonInstance : DrawableBattleComponent
    {
        public long UniqueId; 
        public PokemonArchetype Archetype;
        public string Nickname;
        public CombatMoveSet Moves = new CombatMoveSet();
        public NonVolatileStatus Status = NonVolatileStatus.None;
        public List<VolatileStatus> VolatileStatuses = new List<VolatileStatus>();      

        public Gender Gender;
        public bool IsWild;
        public Trainer OriginalTrainer;

        public int Happiness;

        // AnimationState
        public AttackAnimationState AnimationState = AttackAnimationState.Ready;
        public int FrameCounter;
        public int FramesToApex = 10;

        public bool Hit;
        public bool HitInvisible;
        public int FlickerInterval = 15;
        public int FlickerDuration = 6;
        public int FlickerCounter;

        // Rendering
        public Vector2 AllyStartPosition = new Vector2(32, 52);
        public Vector2 EnemyStartPosition = new Vector2(140, 10);
        public RenderingPosition RenderingPosition = RenderingPosition.Ally;

        // Stats
        public int CurrentHealth;
        public Dictionary<Stats, int> CurrentStats = new Dictionary<Stats, int>();

        public PokemonCombatStats CombatStats;

        // Levelling
        public int Level;
        public int Experience;
        public int ExperienceRequiredForNextLevel;

        public PokemonInstance(Game game, long uniqueId, PokemonArchetype archetype, string nickname,
            Gender gender, bool isWild, Trainer originalTrainer, int experience) : base(game)
        {
            UniqueId = uniqueId;
            Archetype = archetype;
            Nickname = nickname;
            Gender = gender;
            IsWild = isWild;
            OriginalTrainer = originalTrainer;
            Experience = experience;

            CalculateLevel();
        }

        private void CalculateLevel()
        {
            while (Experience > ExperienceRequiredForNextLevel && ExperienceRequiredForNextLevel != -1)
            {
                LevelUp();
            }   
    
            CombatStats = new PokemonCombatStats(CurrentStats);
        }

        private void LevelUp()
        {
            Experience -= ExperienceRequiredForNextLevel; 
            Level++;
            if (Level < 100)
                ExperienceRequiredForNextLevel = ExperienceCatalogue.ExperienceTables[Archetype.ExperienceGroup][Level - 1];
            else
                ExperienceRequiredForNextLevel = -1;
            CurrentStats[Stats.Health] = (int)Math.Ceiling(Archetype.MaxStats[Stats.Health] * (Level / 100f));
            CurrentStats[Stats.Attack] = (int)Math.Ceiling(Archetype.MaxStats[Stats.Attack] * (Level / 100f));
            CurrentStats[Stats.Defense] = (int)Math.Ceiling(Archetype.MaxStats[Stats.Defense] * (Level / 100f));
            CurrentStats[Stats.SpecialAttack] = (int)Math.Ceiling(Archetype.MaxStats[Stats.SpecialAttack] * (Level / 100f));
            CurrentStats[Stats.SpecialDefense] = (int)Math.Ceiling(Archetype.MaxStats[Stats.SpecialDefense] * (Level / 100f));
            CurrentStats[Stats.Speed] = (int)Math.Ceiling(Archetype.MaxStats[Stats.Speed] * (Level / 100f));
            FullRestore();

            MoveArchetype newMove;
            Archetype.LevelledMoves.TryGetValue(Level, out newMove);



            if (newMove != null) 
            {
                if (Moves.Count == Moves.MaximumSize) 
                {
                    CombatMove oldestMove = null;

                    foreach (var move in Moves.Where(move => oldestMove == null || oldestMove.GainedAtLevel < move.GainedAtLevel)) 
                    {
                        oldestMove = move;
                    }

                    Moves.Remove(oldestMove);    
                }

                Moves.Add(new CombatMove(newMove, Level));                
            }
        }

        private void HealPokemon()
        {
            CurrentHealth = CurrentStats[Stats.Health];
        }

        private void RestoreAllPP()
        {
            Moves.RestorePP();
        }

        public void FullRestore()
        {
            HealPokemon();
            RestoreAllPP();
        }

        public void PrepareForCombat(RenderingPosition renderingPosition)
        {
            CombatStats = new PokemonCombatStats(CurrentStats);

            // All enemies always start at max health
            if (RenderingPosition == RenderingPosition.Enemy)
                CurrentHealth = CurrentStats[Stats.Health];

            RenderingPosition = renderingPosition;
            switch (RenderingPosition)
            {
                case RenderingPosition.Ally:
                    Position = AllyStartPosition;
                    break;
                case RenderingPosition.Enemy:
                    Position = EnemyStartPosition;
                    break;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!HitInvisible)
            {
                switch (RenderingPosition)
                {
                    case RenderingPosition.Ally:
                        spriteBatch.Draw(Archetype.Texture, Position, Archetype.SourceRectangleBack, Color.White);
                        break;
                    case RenderingPosition.Enemy:
                        spriteBatch.Draw(Archetype.Texture, Position, Archetype.SourceRectangleFront, Color.White);
                        break;
                }  
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Attack animation
            if (AnimationState == AttackAnimationState.Starting)
            {
                if (RenderingPosition == RenderingPosition.Ally)
                    Position = new Vector2(Position.X + 3, Position.Y);
                else
                    Position = new Vector2(Position.X - 3, Position.Y);

                FrameCounter++;

                if (FrameCounter == FramesToApex)
                    AnimationState = AttackAnimationState.Ending;
            }
            else if (AnimationState == AttackAnimationState.Ending)
            {
                if (RenderingPosition == RenderingPosition.Ally)
                    Position = new Vector2(Position.X - 3, Position.Y);
                else
                    Position = new Vector2(Position.X + 3, Position.Y);

                FrameCounter--;

                if (FrameCounter == 0)
                    AnimationState = AttackAnimationState.Ended;
            }

            // Hit animation
            if (Hit)
            {
                if (FrameCounter == 0)
                    HitInvisible = !HitInvisible;
                FrameCounter++;
                if (FrameCounter == FlickerInterval)
                {
                    FrameCounter = 0;
                    FlickerCounter++;
                }
                if (FlickerCounter == FlickerDuration)
                {
                    FlickerCounter = 0;
                    Hit = false;
                    HitInvisible = false;
                }
            }
        }

        public void ApplyDamage(int appliedDamage) {
            Hit = true;
            CurrentHealth -= appliedDamage;
        }

        public void AwardExperience(int awardedExperience)
        {
            Experience += awardedExperience;
            CalculateLevel();
        }
    }
}
