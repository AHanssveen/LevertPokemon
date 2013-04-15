using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.BattleSystem.Pokemon;
using PokemonTheRemakez.BattleSystem.Pokemon.Experience;
using PokemonTheRemakez.BattleSystem.Pokemon.Moves;
using PokemonTheRemakez.BattleSystem.Pokemon.Moves.Combat;
using PokemonTheRemakez.BattleSystem.Pokemon.Moves.Levelled;
using PokemonTheRemakez.BattleSystem.Trainers;

namespace PokemonTheRemakez.BattleSystem
{
    public class DataManager
    {
        private static Texture2D _pokemonTexture;

        public static Dictionary<string, MoveArchetype> MoveArchetypes = new Dictionary<string, MoveArchetype>();
        public static Dictionary<string, PokemonArchetype> PokemonArchetypes = new Dictionary<string, PokemonArchetype>();
        public static Dictionary<long, PokemonInstance> PokemonInstances = new Dictionary<long, PokemonInstance>();
        public static Dictionary<string, Trainer> Trainers = new Dictionary<string, Trainer>();
        public static Dictionary<string, float> Effectiveness = new Dictionary<string, float>();  


        public static float GetEffectiveness(ElementType moveElement, ElementType targetElement)
        {
            var key = moveElement + "," + targetElement;
            return Effectiveness.ContainsKey(key) ? Effectiveness[key] : 1f;
        }

        public static void AddEffectiveness(ElementType moveElement, ElementType targetElement, float effectiveness)
        {
            var key = moveElement + "," + targetElement;
            Effectiveness.Add(key, effectiveness);
        }


        public DataManager(Game game)
        {
            var experienceCatalogue = new ExperienceCatalogue();

            _pokemonTexture = game.Content.Load<Texture2D>(@"Resources\Pokemon\pokemon");

            var name = "Trond";
            Trainers.Add(name, new Trainer(game, name, "trainers2"));

            name = "Sabrina";
            Trainers.Add(name, new Trainer(game, name, "trainers2"));


            //Moves------------------------------------------------

            //Normal moves
            name = "Tackle";
            MoveArchetypes.Add(name, new MoveArchetype(
                name, 
                MoveType.Physical, 
                ElementType.Normal, 
                35, 
                50, 
                100,
                DurationType.SingleFire, 
                0,
                BattleEffectStageEffect.None,
                EffectType.None,
                EffectStart.Immediate, 
                MoveTarget.ActiveEnemy, 
                BattleEffectType.DamageOnly,
                ""
                ));

            name = "Scratch";
            MoveArchetypes.Add(name, new MoveArchetype(
                name,
                MoveType.Physical,
                ElementType.Normal,
                35,
                50,
                100,
                DurationType.SingleFire,
                0,
                BattleEffectStageEffect.None,
                EffectType.None,
                EffectStart.Immediate,
                MoveTarget.ActiveEnemy,
                BattleEffectType.DamageOnly,
                ""
                ));

            name = "Slam";
            MoveArchetypes.Add(name, new MoveArchetype(
                name,
                MoveType.Physical,
                ElementType.Normal,
                20,
                80,
                75,
                DurationType.SingleFire,
                0,
                BattleEffectStageEffect.None,
                EffectType.None,
                EffectStart.Immediate,
                MoveTarget.ActiveEnemy,
                BattleEffectType.DamageOnly,
                ""
                ));


            //Dark moves
            name = "Bite";
            MoveArchetypes.Add(name, new MoveArchetype(
                name,
                MoveType.Physical,
                ElementType.Dark,
                25,
                60,
                100,
                DurationType.SingleFire,
                0,
                BattleEffectStageEffect.None,
                EffectType.None,
                EffectStart.Immediate,
                MoveTarget.ActiveEnemy,
                BattleEffectType.DamageOnly,
                ""
                ));


            //Water moves
            name = "Aqua Tail";
            MoveArchetypes.Add(name, new MoveArchetype(
                name,
                MoveType.Physical,
                ElementType.Water,
                10,
                90,
                90,
                DurationType.SingleFire,
                0,
                BattleEffectStageEffect.None,
                EffectType.None,
                EffectStart.Immediate,
                MoveTarget.ActiveEnemy,
                BattleEffectType.DamageOnly,
                ""
                ));


            //Ice moves
            name = "Ice Fang";
            MoveArchetypes.Add(name, new MoveArchetype(
                name,
                MoveType.Physical,
                ElementType.Ice,
                15,
                65,
                95,
                DurationType.SingleFire,
                0,
                BattleEffectStageEffect.None,
                EffectType.None,
                EffectStart.Immediate,
                MoveTarget.ActiveEnemy,
                BattleEffectType.DamageOnly,
                ""
                ));


            //Flying moves
            name = "Gust";
            MoveArchetypes.Add(name, new MoveArchetype(
                name,
                MoveType.Special,
                ElementType.Flying,
                35,
                40,
                100,
                DurationType.SingleFire,
                0,
                BattleEffectStageEffect.None,
                EffectType.None,
                EffectStart.Immediate,
                MoveTarget.ActiveEnemy,
                BattleEffectType.DamageOnly,
                ""
                ));

            name = "Wing Attack";
            MoveArchetypes.Add(name, new MoveArchetype(
                name,
                MoveType.Physical,
                ElementType.Flying,
                35,
                60,
                100,
                DurationType.SingleFire,
                0,
                BattleEffectStageEffect.None,
                EffectType.None,
                EffectStart.Immediate,
                MoveTarget.ActiveEnemy,
                BattleEffectType.DamageOnly,
                ""
                ));

            name = "Peck";
            MoveArchetypes.Add(name, new MoveArchetype(
                name,
                MoveType.Physical,
                ElementType.Flying,
                35,
                35,
                100,
                DurationType.SingleFire,
                0,
                BattleEffectStageEffect.None,
                EffectType.None,
                EffectStart.Immediate,
                MoveTarget.ActiveEnemy,
                BattleEffectType.DamageOnly,
                ""
                ));


            //Fire moves
            name = "Fire Blast";
            MoveArchetypes.Add(name, new MoveArchetype(
                name,
                MoveType.Special,
                ElementType.Fire,
                5,
                120,
                85,
                DurationType.SingleFire,
                0,
                BattleEffectStageEffect.None,
                EffectType.None,
                EffectStart.Immediate,
                MoveTarget.ActiveEnemy,
                BattleEffectType.DamageOnly,
                ""
                ));


            //Psycic moves
            name = "Confusion";
            MoveArchetypes.Add(name, new MoveArchetype(
                name,
                MoveType.Special,
                ElementType.Psychic,
                25,
                50,
                100,
                DurationType.SingleFire,
                0,
                BattleEffectStageEffect.None,
                EffectType.None,
                EffectStart.Immediate,
                MoveTarget.ActiveEnemy,
                BattleEffectType.DamageOnly,
                ""
                ));

            name = "Psybeam";
            MoveArchetypes.Add(name, new MoveArchetype(
                name,
                MoveType.Special,
                ElementType.Psychic,
                20,
                65,
                100,
                DurationType.SingleFire,
                0,
                BattleEffectStageEffect.None,
                EffectType.None,
                EffectStart.Immediate,
                MoveTarget.ActiveEnemy,
                BattleEffectType.DamageOnly,
                ""
                ));

            name = "Psychic";
            MoveArchetypes.Add(name, new MoveArchetype(
                name,
                MoveType.Special,
                ElementType.Psychic,
                10,
                90,
                100,
                DurationType.SingleFire,
                0,
                BattleEffectStageEffect.None,
                EffectType.None,
                EffectStart.Immediate,
                MoveTarget.ActiveEnemy,
                BattleEffectType.DamageOnly,
                ""
                ));


            //Ground moves
            name = "Rock Throw";
            MoveArchetypes.Add(name, new MoveArchetype(
                name,
                MoveType.Physical,
                ElementType.Rock,
                15,
                50,
                90,
                DurationType.SingleFire,
                0,
                BattleEffectStageEffect.None,
                EffectType.None,
                EffectStart.Immediate,
                MoveTarget.ActiveEnemy,
                BattleEffectType.DamageOnly,
                ""
                ));

            name = "Rock Slide";
            MoveArchetypes.Add(name, new MoveArchetype(
                name,
                MoveType.Physical,
                ElementType.Rock,
                10,
                75,
                90,
                DurationType.SingleFire,
                0,
                BattleEffectStageEffect.None,
                EffectType.None,
                EffectStart.Immediate,
                MoveTarget.ActiveEnemy,
                BattleEffectType.DamageOnly,
                ""
                ));


            //MoveSet--------------------------------------------------

            var levelledList = new List<LevelledMove>
                {
                    new LevelledMove(0, MoveArchetypes["Tackle"]),
                    new LevelledMove(0, MoveArchetypes["Scratch"])
                };

            // Standard
            var standardMoveSet = new CombatMoveSet
                {
                    new CombatMove(MoveArchetypes["Tackle"]),
                    new CombatMove(MoveArchetypes["Scratch"])
                };



            var alakazamMoveSet = new CombatMoveSet
                {
                    new CombatMove(MoveArchetypes["Tackle"]),
                    new CombatMove(MoveArchetypes["Confusion"]),
                    new CombatMove(MoveArchetypes["Psybeam"]),
                    new CombatMove(MoveArchetypes["Psychic"])
                };

            var pidgeotMoveSet = new CombatMoveSet
                {
                    new CombatMove(MoveArchetypes["Tackle"]),
                    new CombatMove(MoveArchetypes["Wing Attack"]),
                    new CombatMove(MoveArchetypes["Gust"]),
                    new CombatMove(MoveArchetypes["Peck"])
                };

            var onixMoveSet = new CombatMoveSet
                {
                    new CombatMove(MoveArchetypes["Tackle"]),
                    new CombatMove(MoveArchetypes["Slam"]),
                    new CombatMove(MoveArchetypes["Rock Throw"]),
                    new CombatMove(MoveArchetypes["Rock Slide"])
                };

            var arcanineMoveSet = new CombatMoveSet
                {
                    new CombatMove(MoveArchetypes["Tackle"]),
                    new CombatMove(MoveArchetypes["Fire Blast"])
                };

            var gyaradosMoveSet = new CombatMoveSet
                {
                    new CombatMove(MoveArchetypes["Slam"]),
                    new CombatMove(MoveArchetypes["Bite"]),
                    new CombatMove(MoveArchetypes["Aqua Tail"]),
                    new CombatMove(MoveArchetypes["Ice Fang"])
                };
            

            // Pokemon-----------------------------------------------------

            name = "Alakazam";
            PokemonArchetypes.Add(name, new PokemonArchetype(
                name,
                65,
                ElementType.Psychic,
                ElementType.None,
                "Alakazam's brain continually grows, infinitely multiplying brain cells. " +
                "This amazing brain gives this Pokémon an astoundingly high IQ of 5,000. It " +
                "has a thorough memory of everything that has occurred in the world.",
                false,
                0,
                13,
                6,
                6,
                8,
                7,
                8,
                314,
                199,
                189,
                369,
                269,
                339,
                _pokemonTexture,
                new Rectangle(226, 854, 64, 64),
                new Rectangle(162, 854, 64, 64),
                new Rectangle(290, 854, 32, 32),
                new Rectangle(290, 886, 32, 32),
                ExperienceGroup.MediumSlow, 
                levelledList,
                Gender.None
                ));

            int uniqueID = 1;
            PokemonInstances.Add(uniqueID, new PokemonInstance(
                game,
                uniqueID,
                PokemonArchetypes[name],
                name,
                alakazamMoveSet,
                Gender.Male,
                false,
                Trainers["Sabrina"],
                90000
                ));

            name = "Pidgeot";
            PokemonArchetypes.Add(name, new PokemonArchetype(
                name,
                18,
                ElementType.Normal,
                ElementType.Flying,
                "When hunting, it skims the surface of water at high " +
                "speed to pick off unwary prey such as Magikarp.",
                false,
                0,
                13,
                7,
                7,
                7,
                7,
                7,
                370,
                259,
                249,
                239,
                239,
                281,
                _pokemonTexture,
                new Rectangle(64, 334, 64, 64),
                new Rectangle(1, 330, 64, 64),
                new Rectangle(290, 854, 32, 32),
                new Rectangle(290, 886, 32, 32),
                ExperienceGroup.MediumSlow,
                levelledList,
                Gender.None
                ));

            uniqueID = 2;
            PokemonInstances.Add(uniqueID, new PokemonInstance(
                game,
                uniqueID,
                PokemonArchetypes[name],
                name,
                pidgeotMoveSet,
                Gender.Male,
                false,
                Trainers["Trond"],
                90000
                ));


            name = "Onix";
            PokemonArchetypes.Add(name, new PokemonArchetype(
                name,
                95,
                ElementType.Rock,
                ElementType.Ground,
                "As it grows, the stone portions of its body harden to become similar " +
                "to a diamond, but colored black.",
                false,
                0,
                12,
                6,
                9,
                6,
                6,
                7,
                274,
                189,
                419,
                159,
                189,
                239,
                _pokemonTexture,
                new Rectangle(1030, 265, 64, 64),
                new Rectangle(970, 265, 64, 64),
                new Rectangle(1095, 270, 32, 32),
                new Rectangle(1095, 300, 32, 32),
                ExperienceGroup.MediumFast,
                levelledList,
                Gender.None
                ));

            uniqueID = 3;
            PokemonInstances.Add(uniqueID, new PokemonInstance(
                game,
                uniqueID,
                PokemonArchetypes[name],
                name,
                onixMoveSet,
                Gender.Male,
                false,
                Trainers["Trond"],
                90000
                ));


            name = "Arcanine";
            PokemonArchetypes.Add(name, new PokemonArchetype(
                name,
                59,
                ElementType.Fire, 
                ElementType.None,
                "Arcanine is known for its high speed. It is said to be " +
                "capable of running over 6,200 miles in a single day and " +
                "night. The fire that blazes wildly within this Pokémon's " +
                "body is its source of power.",
                false,
                0,
                13,
                8,
                7,
                7,
                8,
                9,
                384,
                319,
                259,
                299,
                259,
                289,
                _pokemonTexture,
                new Rectangle(709, 657, 64, 64),
                new Rectangle(645, 657, 64, 64),
                new Rectangle(773, 657, 32, 32),
                new Rectangle(773, 689, 100, 32),
                ExperienceGroup.MediumSlow,
                levelledList,
                Gender.None
                ));

            uniqueID = 4;
            PokemonInstances.Add(uniqueID, new PokemonInstance(
                game,
                uniqueID,
                PokemonArchetypes[name],
                name,
                arcanineMoveSet,
                Gender.Female,
                false,
                Trainers["Trond"],
                100
                ));

            name = "Gyarados";
            PokemonArchetypes.Add(name, new PokemonArchetype(
                name,
                130,
                ElementType.Water,
                ElementType.Flying,
                "Brutally vicious and enormously destructive. " +
                "Known for totally destroying cities in ancient times.",
                false,
                0,
                13,
                8,
                7,
                7,
                7,
                7,
                394,
                349,
                257,
                219,
                299,
                261,
                _pokemonTexture,
                new Rectangle(1353, 592, 64, 64),
                new Rectangle(1289, 592, 64, 64),
                new Rectangle(290, 854, 32, 32),
                new Rectangle(290, 886, 32, 32),
                ExperienceGroup.Slow,
                levelledList,
                Gender.None
                ));

            uniqueID = 5;
            PokemonInstances.Add(uniqueID, new PokemonInstance(
                game,
                uniqueID,
                PokemonArchetypes[name],
                name,
                gyaradosMoveSet,
                Gender.Male,
                false,
                Trainers["Trond"],
                90000
                ));

            name = "Scyther";
            PokemonArchetypes.Add(name, new PokemonArchetype(
                name,
                123,
                ElementType.Bug,
                ElementType.Flying,
                "Brutally vicious and enormously destructive. " +
                "Known for totally destroying cities in ancient times.",
                false,
                0,
                13,
                8,
                7,
                7,
                7,
                7,
                394,
                349,
                257,
                219,
                299,
                261,
                _pokemonTexture,
                new Rectangle(1353, 592, 64, 64),
                new Rectangle(1289, 592, 64, 64),
                new Rectangle(290, 854, 32, 32),
                new Rectangle(290, 886, 32, 32),
                ExperienceGroup.Slow,
                levelledList,
                Gender.None
                ));

            uniqueID = 5;
            PokemonInstances.Add(uniqueID, new PokemonInstance(
                game,
                uniqueID,
                PokemonArchetypes[name],
                name,
                gyaradosMoveSet,
                Gender.Male,
                false,
                Trainers["Trond"],
                90000
                ));


 
            Trainers["Trond"].PokemonSet.Add(PokemonInstances[4]);
            Trainers["Sabrina"].PokemonSet.Add(PokemonInstances[4]);
            



            AddEffectiveness(ElementType.Normal, ElementType.Rock, 0.5f);
            AddEffectiveness(ElementType.Normal, ElementType.Steel, 0.5f);
            AddEffectiveness(ElementType.Fire, ElementType.Fire, 0.5f);
            AddEffectiveness(ElementType.Fire, ElementType.Water, 0.5f);
            AddEffectiveness(ElementType.Fire, ElementType.Grass, 2f);
            AddEffectiveness(ElementType.Fire, ElementType.Ice, 2f);
            AddEffectiveness(ElementType.Fire, ElementType.Bug, 2f);
            AddEffectiveness(ElementType.Fire, ElementType.Rock, 0.5f);
            AddEffectiveness(ElementType.Fire, ElementType.Dragon, 0.5f);
            AddEffectiveness(ElementType.Fire, ElementType.Steel, 2f);
            AddEffectiveness(ElementType.Water, ElementType.Fire, 2f);
            AddEffectiveness(ElementType.Water, ElementType.Grass, 0.5f);
            AddEffectiveness(ElementType.Water, ElementType.Ground, 2f);
            AddEffectiveness(ElementType.Water, ElementType.Rock, 2f);
            AddEffectiveness(ElementType.Water, ElementType.Dragon, 0.5f);
            AddEffectiveness(ElementType.Electric, ElementType.Water, 2f);
            AddEffectiveness(ElementType.Electric, ElementType.Electric, 0.5f);
            AddEffectiveness(ElementType.Electric, ElementType.Grass, 0.5f);
            AddEffectiveness(ElementType.Electric, ElementType.Ground, 0f);
            AddEffectiveness(ElementType.Electric, ElementType.Flying, 2f);
            AddEffectiveness(ElementType.Electric, ElementType.Dragon, 0f);
            AddEffectiveness(ElementType.Grass, ElementType.Fire, 0.5f);
            AddEffectiveness(ElementType.Grass, ElementType.Water, 2f);
            AddEffectiveness(ElementType.Grass, ElementType.Grass, 0.5f);
            AddEffectiveness(ElementType.Grass, ElementType.Poision, 0.5f);
            AddEffectiveness(ElementType.Grass, ElementType.Ground, 2f);
            AddEffectiveness(ElementType.Grass, ElementType.Flying, 0.5f);
            AddEffectiveness(ElementType.Grass, ElementType.Bug, 0.5f);
            AddEffectiveness(ElementType.Grass, ElementType.Dragon, 0.5f);
            AddEffectiveness(ElementType.Grass, ElementType.Steel, 0.5f);
            AddEffectiveness(ElementType.Ice, ElementType.Fire, 0.5f);
            AddEffectiveness(ElementType.Ice, ElementType.Water, 0.5f);
            AddEffectiveness(ElementType.Ice, ElementType.Grass, 2f);
            AddEffectiveness(ElementType.Ice, ElementType.Ice, 0.5f);
            AddEffectiveness(ElementType.Ice, ElementType.Ground, 2f);
            AddEffectiveness(ElementType.Ice, ElementType.Flying, 2f);
            AddEffectiveness(ElementType.Ice, ElementType.Dragon, 2f);
            AddEffectiveness(ElementType.Ice, ElementType.Steel, 0.5f);
            AddEffectiveness(ElementType.Fight, ElementType.Normal, 2f);
            AddEffectiveness(ElementType.Fight, ElementType.Ice, 2f);
            AddEffectiveness(ElementType.Fight, ElementType.Poision, 0.5f);
            AddEffectiveness(ElementType.Fight, ElementType.Flying, 0.5f);
            AddEffectiveness(ElementType.Fight, ElementType.Psychic, 0.5f);
            AddEffectiveness(ElementType.Fight, ElementType.Bug, 0.5f);
            AddEffectiveness(ElementType.Fight, ElementType.Rock, 2f);
            AddEffectiveness(ElementType.Fight, ElementType.Ghost, 0f);
            AddEffectiveness(ElementType.Fight, ElementType.Dark, 2f);
            AddEffectiveness(ElementType.Fight, ElementType.Steel, 2f);
            AddEffectiveness(ElementType.Poision, ElementType.Grass, 2f);
            AddEffectiveness(ElementType.Poision, ElementType.Poision, 0.5f);
            AddEffectiveness(ElementType.Poision, ElementType.Ground, 0.5f);
            AddEffectiveness(ElementType.Poision, ElementType.Rock, 0.5f);
            AddEffectiveness(ElementType.Poision, ElementType.Ghost, 0.5f);
            AddEffectiveness(ElementType.Poision, ElementType.Steel, 0f);
            AddEffectiveness(ElementType.Ground, ElementType.Fire, 2f);
            AddEffectiveness(ElementType.Ground, ElementType.Electric, 2f);
            AddEffectiveness(ElementType.Ground, ElementType.Grass, 0.5f);
            AddEffectiveness(ElementType.Ground, ElementType.Poision, 2f);
            AddEffectiveness(ElementType.Ground, ElementType.Flying, 0f);
            AddEffectiveness(ElementType.Ground, ElementType.Bug, 2f);
            AddEffectiveness(ElementType.Ground, ElementType.Rock, 0.5f);
            AddEffectiveness(ElementType.Ground, ElementType.Steel, 0.5f);
            AddEffectiveness(ElementType.Flying, ElementType.Electric, 0.5f);
            AddEffectiveness(ElementType.Flying, ElementType.Grass, 2f);
            AddEffectiveness(ElementType.Flying, ElementType.Fight, 2f);
            AddEffectiveness(ElementType.Flying, ElementType.Bug, 2f);
            AddEffectiveness(ElementType.Flying, ElementType.Rock, 0.5f);
            AddEffectiveness(ElementType.Flying, ElementType.Steel, 0.5f);
            AddEffectiveness(ElementType.Psychic, ElementType.Fight, 2f);
            AddEffectiveness(ElementType.Psychic, ElementType.Poision, 2f);
            AddEffectiveness(ElementType.Psychic, ElementType.Psychic, 0.5f);
            AddEffectiveness(ElementType.Psychic, ElementType.Dark, 0f);
            AddEffectiveness(ElementType.Psychic, ElementType.Steel, 0.5f);
            AddEffectiveness(ElementType.Bug, ElementType.Fire, 0.5f);
            AddEffectiveness(ElementType.Bug, ElementType.Grass, 2f);
            AddEffectiveness(ElementType.Bug, ElementType.Fight, 0.5f);
            AddEffectiveness(ElementType.Bug, ElementType.Flying, 0.5f);
            AddEffectiveness(ElementType.Bug, ElementType.Psychic, 2f);
            AddEffectiveness(ElementType.Bug, ElementType.Ghost, 0.5f);
            AddEffectiveness(ElementType.Bug, ElementType.Dark, 2f);
            AddEffectiveness(ElementType.Bug, ElementType.Steel, 0.5f);
            AddEffectiveness(ElementType.Rock, ElementType.Fire, 2f);
            AddEffectiveness(ElementType.Rock, ElementType.Ice, 2f);
            AddEffectiveness(ElementType.Rock, ElementType.Fight, 0.5f);
            AddEffectiveness(ElementType.Rock, ElementType.Ground, 0.5f);
            AddEffectiveness(ElementType.Rock, ElementType.Flying, 2f);
            AddEffectiveness(ElementType.Rock, ElementType.Bug, 2f);
            AddEffectiveness(ElementType.Rock, ElementType.Steel, 2f);
            AddEffectiveness(ElementType.Ghost, ElementType.Normal, 0f);
            AddEffectiveness(ElementType.Ghost, ElementType.Psychic, 2f);
            AddEffectiveness(ElementType.Ghost, ElementType.Ghost, 2f);
            AddEffectiveness(ElementType.Ghost, ElementType.Dark, 0.5f);
            AddEffectiveness(ElementType.Ghost, ElementType.Steel, 0.5f);
            AddEffectiveness(ElementType.Dragon, ElementType.Dragon, 2f);
            AddEffectiveness(ElementType.Dragon, ElementType.Steel, 0.5f);
            AddEffectiveness(ElementType.Dark, ElementType.Fight, 0.5f);
            AddEffectiveness(ElementType.Dark, ElementType.Psychic, 2f);
            AddEffectiveness(ElementType.Dark, ElementType.Ghost, 2f);
            AddEffectiveness(ElementType.Dark, ElementType.Dark, 0.5f);
            AddEffectiveness(ElementType.Dark, ElementType.Steel, 0.5f);
            AddEffectiveness(ElementType.Steel, ElementType.Fire, 0.5f);
            AddEffectiveness(ElementType.Steel, ElementType.Water, 0.5f);
            AddEffectiveness(ElementType.Steel, ElementType.Electric, 0.5f);
            AddEffectiveness(ElementType.Steel, ElementType.Ice, 2f);
            AddEffectiveness(ElementType.Steel, ElementType.Rock, 2f);
            AddEffectiveness(ElementType.Steel, ElementType.Steel, 0.5f);
        }
    }
}
