using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.BattleSystem.Pokemon;
using PokemonTheRemakez.BattleSystem.Pokemon.Experience;
using PokemonTheRemakez.BattleSystem.Pokemon.Moves;
using PokemonTheRemakez.BattleSystem.Trainers;

namespace PokemonTheRemakez.BattleSystem
{
    public class DataManager
    {
        private static Game Game;
        private static long _idcounter = 10000;

        private static Texture2D _pokemonTexture;

        public static Dictionary<string, MoveArchetype> MoveArchetypes;
        public static Dictionary<string, PokemonArchetype> PokemonArchetypes;
        public static Dictionary<long, PokemonInstance> PokemonInstances;
        public static Dictionary<string, Trainer> Trainers;
        public static Dictionary<string, float> Effectiveness;
        public static Random Random = new Random();
        
        /// <summary>
        /// Gets the effectiveness of an attack
        /// </summary>
        /// <param name="moveElement">the element of the move</param>
        /// <param name="targetElement">the target element of the move</param>
        /// <returns>returns the effectiveness</returns>
        public static float GetEffectiveness(ElementType moveElement, ElementType targetElement)
        {
            var key = moveElement + "," + targetElement;
            return Effectiveness.ContainsKey(key) ? Effectiveness[key] : 1f;
        }

        /// <summary>
        /// Adds effectiveness to an attack
        /// </summary>
        /// <param name="moveElement">the element of the move</param>
        /// <param name="targetElement">the target element of the move</param>
        /// <param name="effectiveness">how effective the move is</param>
        public static void AddEffectiveness(ElementType moveElement, ElementType targetElement, float effectiveness)
        {
            var key = moveElement + "," + targetElement;
            Effectiveness.Add(key, effectiveness);
        }

        /// <summary>
        /// Manages the data for trainers
        /// </summary>
        /// <param name="game">instance of game</param>
        public DataManager(Game game)
        {
            MoveArchetypes = new Dictionary<string, MoveArchetype>();
            PokemonArchetypes = new Dictionary<string, PokemonArchetype>();
            PokemonInstances = new Dictionary<long, PokemonInstance>();
            Trainers = new Dictionary<string, Trainer>();
            Effectiveness = new Dictionary<string, float>();

            Game = game;

            var experienceCatalogue = new ExperienceCatalogue();

            _pokemonTexture = game.Content.Load<Texture2D>(@"Resources\Pokemon\pokemon");

            var name = "Trond";
            Trainers.Add(name, new Trainer(game, name, "trainers2"));

            name = "Lehmann";
            Trainers.Add(name, new Trainer(game, name, "LehmannLarge"));

            name = "Giovanni";
            Trainers.Add(name, new Trainer(game, name, "GiovanniLarge"));

            name = "Sabrina";
            Trainers.Add(name, new Trainer(game, name, "SabrinaLarge"));

            name = "Tall Grass";
            Trainers.Add(name, new Trainer(game, name, "TallGrass"));


            //List of pokemon moves
            #region pokemonMoves 

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

            name = "Slash";
            MoveArchetypes.Add(name, new MoveArchetype(
                name,
                MoveType.Physical,
                ElementType.Normal,
                20,
                70,
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

            // Bug moves
            name = "X-scissor";
            MoveArchetypes.Add(name, new MoveArchetype(
                name,
                MoveType.Physical,
                ElementType.Normal,
                15,
                80,
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

            // Dark moves
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

            name = "Ember";
            MoveArchetypes.Add(name, new MoveArchetype(
                name,
                MoveType.Special,
                ElementType.Fire,
                25,
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

            name = "Flame Wheel";
            MoveArchetypes.Add(name, new MoveArchetype(
                name,
                MoveType.Physical,
                ElementType.Fire,
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


            #endregion

            //another list of pokemon moves
            #region moveLists

            // Standard
            var standardMoveSet = new Dictionary<int, MoveArchetype>();
            standardMoveSet.Add(1, MoveArchetypes["Tackle"]);
            standardMoveSet.Add(10, MoveArchetypes["Scratch"]);
            standardMoveSet.Add(13, MoveArchetypes["Slash"]);
            standardMoveSet.Add(15, MoveArchetypes["Slam"]);

            var psycicMoveSet = new Dictionary<int, MoveArchetype>();
            psycicMoveSet.Add(1, MoveArchetypes["Tackle"]);
            psycicMoveSet.Add(10, MoveArchetypes["Confusion"]);
            psycicMoveSet.Add(13, MoveArchetypes["Psybeam"]);
            psycicMoveSet.Add(15, MoveArchetypes["Psychic"]);

            var flyingMoveSet = new Dictionary<int, MoveArchetype>();
            flyingMoveSet.Add(1, MoveArchetypes["Tackle"]);
            flyingMoveSet.Add(10, MoveArchetypes["Peck"]);
            flyingMoveSet.Add(13, MoveArchetypes["Gust"]);
            flyingMoveSet.Add(15, MoveArchetypes["Wing Attack"]);

            var groundMoveSet = new Dictionary<int, MoveArchetype>();
            groundMoveSet.Add(1, MoveArchetypes["Tackle"]);
            groundMoveSet.Add(10, MoveArchetypes["Slam"]);
            groundMoveSet.Add(13, MoveArchetypes["Rock Slide"]);
            groundMoveSet.Add(15, MoveArchetypes["Rock Throw"]);

            var archanineMoveSet = new Dictionary<int, MoveArchetype>();
            archanineMoveSet.Add(1, MoveArchetypes["Tackle"]);
            archanineMoveSet.Add(10, MoveArchetypes["Ember"]);
            archanineMoveSet.Add(13, MoveArchetypes["Bite"]);
            archanineMoveSet.Add(15, MoveArchetypes["Fire Blast"]);

            var fireMoveSet = new Dictionary<int, MoveArchetype>();
            fireMoveSet.Add(1, MoveArchetypes["Tackle"]);
            fireMoveSet.Add(10, MoveArchetypes["Ember"]);
            fireMoveSet.Add(13, MoveArchetypes["Bite"]);
            fireMoveSet.Add(15, MoveArchetypes["Fire Blast"]);

            var gyaradosMoveSet = new Dictionary<int, MoveArchetype>();
            gyaradosMoveSet.Add(1, MoveArchetypes["Slam"]);
            gyaradosMoveSet.Add(10, MoveArchetypes["Bite"]);
            gyaradosMoveSet.Add(13, MoveArchetypes["Aqua Tail"]);
            gyaradosMoveSet.Add(15, MoveArchetypes["Ice Fang"]);

            var grassMoveSet = new Dictionary<int, MoveArchetype>();
            grassMoveSet.Add(1, MoveArchetypes["Tackle"]);
            grassMoveSet.Add(10, MoveArchetypes["Bite"]);
            grassMoveSet.Add(13, MoveArchetypes["Slash"]);
            grassMoveSet.Add(15, MoveArchetypes["X-scissor"]);

            #endregion

            //archetypes of the pokemon
            #region pokemonArchetype

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
                psycicMoveSet,
                Gender.None
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
                flyingMoveSet,
                Gender.None
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
                new Rectangle(970, 262, 64, 64),
                new Rectangle(1095, 270, 32, 32),
                new Rectangle(1095, 300, 32, 32),
                ExperienceGroup.MediumFast,
                groundMoveSet,
                Gender.None
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
                archanineMoveSet,
                Gender.None
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
                gyaradosMoveSet,
                Gender.None
                ));

            name = "Scyther";
            PokemonArchetypes.Add(name, new PokemonArchetype(
                name,
                123,
                ElementType.Bug,
                ElementType.Flying,
                "With ninja-like agility and speed, it can create " +
                "the illusion that there is more than one.",
                false,
                0,
                13,
                8,
                7,
                7,
                7,
                8,
                344,
                319,
                259,
                209,
                259,
                309,
                _pokemonTexture,
                new Rectangle(869, 725, 64, 64),
                new Rectangle(806, 722, 64, 64),
                new Rectangle(290, 854, 32, 32),
                new Rectangle(290, 886, 32, 32),
                ExperienceGroup.MediumFast,
                grassMoveSet,
                Gender.None
                ));

            name = "Rapidash";
            PokemonArchetypes.Add(name, new PokemonArchetype(
                name,
                78,
                ElementType.Fire,
                ElementType.None,
                "Very competitive, this Pokémon will chase anything " +
                "that moves fast in the hopes of racing it.",
                false,
                0,
                13,
                7,
                7,
                7,
                7,
                8,
                334,
                299,
                239,
                259,
                259,
                309,
                _pokemonTexture,
                new Rectangle(871, 135, 64, 64),
                new Rectangle(806, 131, 64, 64),
                new Rectangle(290, 854, 32, 32),
                new Rectangle(290, 886, 32, 32),
                ExperienceGroup.MediumFast,
                fireMoveSet,
                Gender.None
                ));

            name = "Rattata";
            PokemonArchetypes.Add(name, new PokemonArchetype(
                name,
                19,
                ElementType.Normal,
                ElementType.None,
                "Bites anything when it attacks. Small and very quick, " +
                "it is a common sight in many places.",
                true,
                19,
                12,
                7,
                6,
                6,
                6,
                7,
                264,
                211,
                169,
                149,
                169,
                243,
                _pokemonTexture,
                new Rectangle(231, 202, 64, 64),
                new Rectangle(168, 198, 64, 64),
                new Rectangle(290, 854, 32, 32),
                new Rectangle(290, 886, 32, 32),
                ExperienceGroup.MediumFast,
                standardMoveSet,
                Gender.None
                ));

            name = "Pidgey";
            PokemonArchetypes.Add(name, new PokemonArchetype(
                name,
                16,
                ElementType.Flying,
                ElementType.None,
                "A common sight in forests and woods. It flaps its " +
                "wings at ground level to kick up blinding sand.",
                true,
                17,
                12,
                6,
                6,
                6,
                6,
                7,
                284,
                189,
                179,
                169,
                169,
                221,
                _pokemonTexture,
                new Rectangle(71, 201, 64, 64),
                new Rectangle(5, 198, 64, 64),
                new Rectangle(290, 854, 32, 32),
                new Rectangle(290, 886, 32, 32),
                ExperienceGroup.MediumFast,
                flyingMoveSet,
                Gender.None
                ));
            #endregion

            //instances of pokemon
            #region pokemonInstance

            int uniqueID = 1;
            name = "Alakazam";
            PokemonInstances.Add(uniqueID, new PokemonInstance(
                game,
                uniqueID,
                PokemonArchetypes[name],
                name,
                Gender.Male,
                false,
                Trainers["Sabrina"],
                1100
                ));

            uniqueID = 2;
            name = "Pidgeot";
            PokemonInstances.Add(uniqueID, new PokemonInstance(
                game,
                uniqueID,
                PokemonArchetypes[name],
                name,
                Gender.Male,
                false,
                Trainers["Trond"],
                1900
                ));
            
            uniqueID = 3;
            name = "Onix";
            PokemonInstances.Add(uniqueID, new PokemonInstance(
                game,
                uniqueID,
                PokemonArchetypes[name],
                name,
                Gender.Male,
                false,
                Trainers["Trond"],
                1100
                ));

            
            uniqueID = 4;
            name = "Arcanine";
            PokemonInstances.Add(uniqueID, new PokemonInstance(
                game,
                uniqueID,
                PokemonArchetypes[name],
                name,
                Gender.Female,
                false,
                Trainers["Trond"],
                1200
                ));

            uniqueID = 5;
            name = "Gyarados";
            PokemonInstances.Add(uniqueID, new PokemonInstance(
                game,
                uniqueID,
                PokemonArchetypes[name],
                name,
                Gender.Male,
                false,
                Trainers["Trond"],
                1400
                ));

            uniqueID = 6;
            name = "Scyther";
            PokemonInstances.Add(uniqueID, new PokemonInstance(
                game,
                uniqueID,
                PokemonArchetypes[name],
                name,
                Gender.Male,
                false,
                Trainers["Trond"],
                1900
                ));

            uniqueID = 7;
            name = "Rapidash";
            PokemonInstances.Add(uniqueID, new PokemonInstance(
                game,
                uniqueID,
                PokemonArchetypes[name],
                name,
                Gender.Female,
                false,
                Trainers["Trond"],
                1500
                ));

            uniqueID = 8;
            name = "Rattata";
            PokemonInstances.Add(uniqueID, new PokemonInstance(
                game,
                uniqueID,
                PokemonArchetypes[name],
                name,
                Gender.Female,
                false,
                Trainers["Trond"],
                1600
                ));

            uniqueID = 9;
            name = "Pidgey";
            PokemonInstances.Add(uniqueID, new PokemonInstance(
                game,
                uniqueID,
                PokemonArchetypes[name],
                name,
                Gender.Female,
                false,
                Trainers["Trond"],
                1400               
                ));

            #endregion

            RandomWildPokemon();

            //The current trainers in existence
            Trainers["Trond"].PokemonSet.Add(PokemonInstances[5]);
            Trainers["Trond"].PokemonSet.Add(PokemonInstances[4]);

            Trainers["Lehmann"].PokemonSet.Add(PokemonInstances[2]);
            Trainers["Lehmann"].PokemonSet.Add(PokemonInstances[6]);
            Trainers["Lehmann"].PokemonSet.Add(PokemonInstances[1]);

            Trainers["Sabrina"].PokemonSet.Add(PokemonInstances[9]);
            Trainers["Sabrina"].PokemonSet.Add(PokemonInstances[7]);

            Trainers["Giovanni"].PokemonSet.Add(PokemonInstances[3]);
            Trainers["Giovanni"].PokemonSet.Add(PokemonInstances[8]);

            #region effectiveness
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
            #endregion
        }

        /// <summary>
        /// a random wild pokemon
        /// </summary>
        /// <returns>the tall grass trainer with a pokemon</returns>
        public static Trainer RandomWildPokemon()
        {
            var randomPokemonList = PokemonArchetypes.Keys.ToList();

            var index = Random.Next(0, randomPokemonList.Count());


            Console.WriteLine(index);
            string name = randomPokemonList[index];
            PokemonInstances.Add(++_idcounter, new PokemonInstance(
                Game,
                _idcounter,
                PokemonArchetypes[name],
                name,
                Gender.Female,
                true,
                Trainers["Tall Grass"],
                Random.Next(140, 340)
                ));

            Trainers["Tall Grass"].PokemonSet.Clear();
            Trainers["Tall Grass"].PokemonSet.Add(PokemonInstances[_idcounter]);

            return Trainers["Tall Grass"];
        }
    }
}
