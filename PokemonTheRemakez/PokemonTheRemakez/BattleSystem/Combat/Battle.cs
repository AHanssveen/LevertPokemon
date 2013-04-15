using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PokemonTheRemakez.BattleSystem.Pokemon;
using PokemonTheRemakez.BattleSystem.Pokemon.Moves;
using PokemonTheRemakez.BattleSystem.Pokemon.Moves.Combat;
using PokemonTheRemakez.BattleSystem.Screens.Elements;
using PokemonTheRemakez.BattleSystem.Trainers;
using PokemonTheRemakez.World.Input;
using PokemonTheRemakez.World.Audio;
using PokemonTheRemakez.World.Screens;
using PokemonTheRemakez.World.TileEngine;

namespace PokemonTheRemakez.BattleSystem.Combat
{
    public enum RewardState
    {
        ExperienceToBeAwarded,
        ExperienceAwarded
    }

    public enum ChangingPokemon
    {
        None,
        Ally,
        Enemy
    }

    public enum AttackEffectiveness {
        Undecided,
        Normal,
        NotVeryEffective,
        SuperEffective
    }

    public enum BattleConclusion
    {
        Undecided,
        PlayerWon,
        PlayerLost,
        PlayerRan,
        WildPokemonRan
    }

    public enum EndStage
    {
        DisplayResult,
        DisplayExerienceGain,
        DisplayExit
    }

    public enum BattlePhase
    {
        NotStarted,
        Starting,
        StartingPokemon,
        AllyTurn,
        EnemyTurn,
        DetermineFirstToPerformAction,
        AllyPerformsActionFirst,
        AllyPerformsActionSecond,
        EnemyPerformsActionFirst,
        EnemyPerformsActionSecond,
        PlayerRunning,
        WildPokemonRunning,
        ChangePokemon,
        Ending
    }

    public enum PlayerTurnPhase
    {
        ChoosingAction,
        ChoosingAttack,
        ChoosingPokemon,
        ChoosingItem,
        Running,
        Result
    }

    public enum AttackState
    {
        Undecided,
        Hit,
        Miss
    }

    public class Battle : DrawableGameComponent {
        public Texture2D Frame;

        public Camera Camera;

        public event EventHandler Exit;

        protected virtual void OnExit()
        {
            EventHandler handler = Exit;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public Random Random = new Random();

        public Trainer AlliedTrainer;
        public Trainer EnemyTrainer;

        public PokemonWrapper ActiveAlliedPokemon;
        public PokemonWrapper ActiveEnemyPokemon;

        // State

        public ChangingPokemon ChangingPokemonState = ChangingPokemon.None;
        public BattleConclusion BattleConclusion;
        public EndStage EndStage;
        public BattlePhase CurrentPhase;
        public PlayerTurnPhase CurrentPlayerTurnPhase;
        public RewardState RewardState;
       
        // Timers
        
        public int Timer;
        public int AttackTimer;
        public int AttackTimerLength = 800;

        // Components

        public List<DrawableBattleComponent> DrawableComponents = new List<DrawableBattleComponent>();

        // Choices

        public AttackChoiceArrowPosition PreviousPlayerAttack;
        public AttackChoiceArrowPosition PlayerChosenAttack;
        public int EnemyChosenAttackIndex;

        // Combat calculations

        public AttackState AttackState = AttackState.Undecided;

        // UI Components

        private Background _background;

        private TextPanel _textPanel;
        private PokeballsAlly _pokeballsAlly;
        private PokeballsEnemy _pokeballsEnemy;
       
        private AttackMenu _attackMenu;
        private AttackChoiceArrow _attackChoiceArrow;
        
        private ActionMenu _actionMenu;       
        private ActionChoiceArrow _actionChoiceArrow;

        private AllyPokemonPanel _allyPokemonPanel;
        private EnemyPokemonPanel _enemyPokemonPanel;

        // END

        private bool _trainerEnemyWithdrawing,
                     _withDrawingStarted,
                     _trainerBattle,
                     _trainerHeroWithdrawing,
                     _trainersInPosition;


        public Battle(Game1 game, Trainer alliedTrainer, Trainer enemyTrainer, bool trainerBattle)
            : base(game)
        {
            Camera = new Camera(game.ScreenRectangle) {Zoom = 4f};
            Camera.LockToCenter(game.ScreenRectangle);

            _trainerBattle = trainerBattle;
            CurrentPhase = BattlePhase.Starting;

            AlliedTrainer = alliedTrainer;
            AlliedTrainer.PrepareForCombat(RenderingPosition.Ally);

            EnemyTrainer = enemyTrainer;
            EnemyTrainer.PrepareForCombat(RenderingPosition.Enemy);

            foreach (var pokemon in EnemyTrainer.PokemonSet.Where(pokemon => pokemon.CurrentHealth > 0))
            {
                ActiveEnemyPokemon = new PokemonWrapper(game, pokemon);
                break;
            }
            

            foreach (var pokemon in AlliedTrainer.PokemonSet.Where(pokemon => pokemon.CurrentHealth > 0))
            {
                ActiveAlliedPokemon = new PokemonWrapper(game, pokemon);   
                break;
            }        

            ActiveAlliedPokemon.PokemonInstance.PrepareForCombat(RenderingPosition.Ally);
            ActiveEnemyPokemon.PokemonInstance.PrepareForCombat(RenderingPosition.Enemy);

            // Initiate state
            BattleConclusion = BattleConclusion.Undecided;
            EndStage = EndStage.DisplayResult;
            CurrentPlayerTurnPhase = PlayerTurnPhase.ChoosingAction;
            RewardState = RewardState.ExperienceToBeAwarded;
            
            LoadContent();
        }

        public new void LoadContent() {
            Frame = Game.Content.Load<Texture2D>(@"BattleFrame");

            _background = new Background(Game, BackgroundType.Grass);
            DrawableComponents.Add(_background);

            DrawableComponents.Add(EnemyTrainer);
            DrawableComponents.Add(AlliedTrainer);

            ActiveAlliedPokemon.Visible = false;
            ActiveEnemyPokemon.Visible = false;
            DrawableComponents.Add(ActiveAlliedPokemon);
            DrawableComponents.Add(ActiveEnemyPokemon);

            _textPanel = new TextPanel(Game, 0, 110, false);
            DrawableComponents.Add(_textPanel);

            _allyPokemonPanel = new AllyPokemonPanel(Game) { Visible = false };
            DrawableComponents.Add(_allyPokemonPanel);

            _enemyPokemonPanel = new EnemyPokemonPanel(Game) { Visible = false };
            DrawableComponents.Add(_enemyPokemonPanel);

            _pokeballsAlly = new PokeballsAlly(Game, AlliedTrainer) { Visible = false };
            DrawableComponents.Add(_pokeballsAlly);

            _pokeballsEnemy = new PokeballsEnemy(Game, EnemyTrainer) { Visible = false };
            DrawableComponents.Add(_pokeballsEnemy);

            _attackChoiceArrow = new AttackChoiceArrow(Game) { Visible = false };
            _attackMenu = new AttackMenu(Game, _attackChoiceArrow, ActiveAlliedPokemon.PokemonInstance) { Visible = false };
            DrawableComponents.Add(_attackMenu);
            DrawableComponents.Add(_attackChoiceArrow);

            _actionChoiceArrow = new ActionChoiceArrow(Game) { Visible = false };
            _actionMenu = new ActionMenu(Game) { Visible = false };
            DrawableComponents.Add(_actionMenu);
            DrawableComponents.Add(_actionChoiceArrow);
            
            DrawableComponents.Add(_textPanel.BattleText);
        }

        public override void Update(GameTime gameTime)
        {
            // Conclude battle if any active pokemon reaches 0 HP
            if (BattleConclusion == BattleConclusion.Undecided &&
                (ActiveAlliedPokemon.PokemonInstance.CurrentHealth == 0
                 || ActiveEnemyPokemon.PokemonInstance.CurrentHealth == 0))
            {
               CurrentPhase = BattlePhase.ChangePokemon; 
            }                   

            switch (CurrentPhase)
            {
                case BattlePhase.Starting:
                    StartBattle(gameTime);
                    break;
                case BattlePhase.AllyTurn:
                    AllyTurn(gameTime);
                    break;
                case BattlePhase.EnemyTurn:
                    EnemyTurn(gameTime);
                    break;
                case BattlePhase.DetermineFirstToPerformAction:
                    DetermineFirstToPerformAction();
                    break;
                case BattlePhase.AllyPerformsActionFirst:
                case BattlePhase.AllyPerformsActionSecond:
                    AllyPerformsAttack(gameTime);
                    break;
                case BattlePhase.EnemyPerformsActionFirst:
                case BattlePhase.EnemyPerformsActionSecond:
                    EnemyPerformsAttack(gameTime);
                    break;
                case BattlePhase.PlayerRunning:
                    PlayerRunning(gameTime);
                    break;
                case BattlePhase.ChangePokemon:
                    ChangePokemon(gameTime);
                    break;
                case BattlePhase.Ending:
                    Ending(gameTime);
                    break;
            }

            // Update all UI components
            foreach (var component in DrawableComponents.Where(component => component.Visible))
            {
                component.Update(gameTime);
            }
        }

        private void ChangePokemon(GameTime gameTime)
        {
            ChangingPokemonState = ChangingPokemon.None;

            _textPanel.TextPromptArrow.WaitingForTextToAppear(gameTime);



            if (ActiveAlliedPokemon.PokemonInstance.CurrentHealth == 0)
            {
                foreach (var pokemon in AlliedTrainer.PokemonSet.Where(pokemon => pokemon.CurrentHealth > 0))
                {
                    ActiveAlliedPokemon.PokemonInstance = pokemon;
                    ActiveAlliedPokemon.PokemonInstance.PrepareForCombat(RenderingPosition.Ally);
                    _attackMenu.SetPokemon(ActiveAlliedPokemon.PokemonInstance);
                    _allyPokemonPanel.SetPokemon(ActiveAlliedPokemon.PokemonInstance);

                    ChangingPokemonState = ChangingPokemon.Ally;
                    CurrentPhase = BattlePhase.AllyTurn;
                    AttackState = AttackState.Undecided;
                    break;
                }       

                if (ChangingPokemonState == ChangingPokemon.None)
                {
                    BattleConclusion = BattleConclusion.PlayerLost;
                    Game1.StateManager.ChangeState(((Game1) Game).GameOverScreen);
                }
            }
            else if (ActiveEnemyPokemon.PokemonInstance.CurrentHealth == 0)
            {
                foreach (var pokemon in EnemyTrainer.PokemonSet.Where(pokemon => pokemon.CurrentHealth > 0))
                {
                    ActiveEnemyPokemon.PokemonInstance = pokemon;
                    ActiveEnemyPokemon.PokemonInstance.PrepareForCombat(RenderingPosition.Enemy);
                    _enemyPokemonPanel.SetPokemon(ActiveEnemyPokemon.PokemonInstance);

                    ChangingPokemonState = ChangingPokemon.Enemy;
                    CurrentPhase = BattlePhase.AllyTurn;
                    AttackState = AttackState.Undecided;
                    break;
                }

                if (ChangingPokemonState == ChangingPokemon.None)
                {
                    if (EnemyTrainer.Name.Equals("Lehmann"))
                        Game1.StateManager.ChangeState(((Game1)Game).VictoryScreen);
                }
            }

            if (ChangingPokemonState == ChangingPokemon.None)
            {
                CurrentPhase = BattlePhase.Ending;
                if (ActiveEnemyPokemon.PokemonInstance.CurrentHealth == 0)
                {
                    AudioController.RequestTrack("battle").Stop();
                    AudioController.RequestTrack("victory").Play();
                    BattleConclusion = BattleConclusion.PlayerWon;
                }
                else
                {
                    BattleConclusion = BattleConclusion.PlayerLost;
                }
            }
                


        }

        private void PlayerRunning(GameTime gameTime)
        {
            _textPanel.BattleText.FirstLine = "Got away safely.";
            _textPanel.BattleText.SecondLine = "";

            _textPanel.TextPromptArrow.WaitingForTextToAppear(gameTime);

            if (_textPanel.TextPromptArrow.State == TextArrowState.Clicked)
            {
                CurrentPhase = BattlePhase.Ending;
                BattleConclusion = BattleConclusion.PlayerRan;
            }
        }

        private void DetermineFirstToPerformAction()
        {
            float alliedPokemonSpeed = ActiveAlliedPokemon.PokemonInstance.CombatStats[Stats.Speed].CurrentStatValue;
            float enemyPokemonSpeed = ActiveEnemyPokemon.PokemonInstance.CombatStats[Stats.Speed].CurrentStatValue;

            if (alliedPokemonSpeed > enemyPokemonSpeed)
                CurrentPhase = BattlePhase.AllyPerformsActionFirst;
            else if (alliedPokemonSpeed > enemyPokemonSpeed)
                CurrentPhase = BattlePhase.EnemyPerformsActionFirst;
            else
                CurrentPhase = Random.Next(0,2) == 1 ? BattlePhase.AllyPerformsActionFirst : BattlePhase.EnemyPerformsActionFirst;
        }


        /// <summary>
        /// Starts the battle and handles the update logic for that part
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the gametime</param>
        public void StartBattle(GameTime gameTime)
        {

            //Moves trainers into position
            if (AlliedTrainer.Position.X > 50 && EnemyTrainer.Position.X < 200 && !_withDrawingStarted)
            {
                EnemyTrainer.Position = new Vector2(EnemyTrainer.Position.X + 2, EnemyTrainer.Position.Y);
                AlliedTrainer.Position = new Vector2(AlliedTrainer.Position.X - 2, AlliedTrainer.Position.Y);
            }

            //Tests if the trainers are in position for the battleText to appear
            if (EnemyTrainer.Position.X >= 50 && EnemyTrainer.Position.X >= 150 && !_withDrawingStarted)
            {
                _textPanel.BattleText.FirstLine = EnemyTrainer.Name + " wants to fight!";

                _trainersInPosition = true;
                _textPanel.TextPromptArrow.WaitingForTextToAppear(gameTime);

                if (_textPanel.TextPromptArrow.State == TextArrowState.Clicked)
                {
                    _trainerEnemyWithdrawing = true;
                    _trainersInPosition = false;
                }
            }

            //EnemyTrainer enemy starts it's withdrawal
            if (_trainerEnemyWithdrawing)
            {
                _withDrawingStarted = true;
                EnemyTrainer.Position = new Vector2(EnemyTrainer.Position.X +2, EnemyTrainer.Position.Y);

                if (EnemyTrainer.Position.X >= 250)
                    _textPanel.BattleText.FirstLine = EnemyTrainer.Name + " sent out " +
                                                        ActiveEnemyPokemon.PokemonInstance.Archetype.Name + "!";


                if (EnemyTrainer.Position.X >= 400)
                {
                    EnemyTrainer.Position = new Vector2(EnemyTrainer.Position.X - 2, EnemyTrainer.Position.Y);

                    _textPanel.TextPromptArrow.WaitingForTextToAppear(gameTime);

                    if (_textPanel.TextPromptArrow.State == TextArrowState.Clicked)
                    {
                        _textPanel.BattleText.FirstLine = "";
                        Timer = 0;
                        _trainerEnemyWithdrawing = false;
                        _trainerHeroWithdrawing = true;
                    }
                }
            }
            

            //EnemyTrainer hero starts it's withdrawal
            if (_trainerHeroWithdrawing)
            {
                Timer += gameTime.ElapsedGameTime.Milliseconds;
                if (Timer >= 100)
                    AlliedTrainer.Position = new Vector2(AlliedTrainer.Position.X - 2, AlliedTrainer.Position.Y);

                if (Math.Abs(AlliedTrainer.Position.X - 30) < 1)
                    _textPanel.BattleText.FirstLine = "Go, " + ActiveAlliedPokemon.PokemonInstance.Archetype.Name + "!";
            }

            //Changes the battlephase after you have thrown out a pokemon
            if (Math.Abs(AlliedTrainer.Position.X + 300) < 1)
            {
                EnemyTrainer.Visible = false;
                AlliedTrainer.Visible = false;
                CurrentPhase = BattlePhase.AllyTurn;
                Timer = 0;
            }

            //Draws the hero and the basic control-panel background
            EnemyTrainer.Visible = true;

            //Both trainers are in position to withdraw and throw out a pokeball
            if (_trainersInPosition)
            {
                _pokeballsAlly.Visible = true;

                if (EnemyTrainer.Position.X < 250)
                    _pokeballsEnemy.Visible = true;
            }

            //The enemy withdraws to throw out a pokemon
            if (EnemyTrainer.Position.X >= 160)
                _pokeballsEnemy.Visible = false;

            //The enemy withdraws to throw out a pokemon
            if (EnemyTrainer.Position.X >= 300)
            {
                if (_enemyPokemonPanel.Visible == false)
                {
                    AudioController.RequestTrack("deployPokeBall").Play();
                    // AudioController.RequestTrack("name").Play();

                }

                ActiveEnemyPokemon.Visible = true;
                _enemyPokemonPanel.Visible = true;

                if (_enemyPokemonPanel.Pokemon.PokemonInstance != ActiveEnemyPokemon.PokemonInstance)
                    _enemyPokemonPanel.SetPokemon(ActiveEnemyPokemon.PokemonInstance);
            }

            if (AlliedTrainer.Position.X <= 40)
                _pokeballsAlly.Visible = false;

            if (AlliedTrainer.Position.X <= -100)
            {
                if (ActiveAlliedPokemon.Visible == false)
                {
                    AudioController.RequestTrack("deployPokeBall").Play();

                    AudioController.RequestTrack(ActiveAlliedPokemon.PokemonInstance.Archetype.Name).Play();
                }

                ActiveAlliedPokemon.Visible = true;
                _allyPokemonPanel.Visible = true;


                if (_allyPokemonPanel.Pokemon.PokemonInstance != ActiveAlliedPokemon.PokemonInstance)
                    _allyPokemonPanel.SetPokemon(ActiveAlliedPokemon.PokemonInstance);
            }
        }

        public void ChoosingAction(GameTime gameTime)
        {
            _actionMenu.Visible = true;
            _actionChoiceArrow.Visible = true;

            _textPanel.BattleText.FirstLine = "What will";
            _textPanel.BattleText.SecondLine = ActiveAlliedPokemon.PokemonInstance.Archetype.Name + " do?";

            if (InputHandler.ActionKeyPressed(ActionKey.ConfirmAndInteract, PlayerIndex.One))
            {
                switch (_actionChoiceArrow.ActionChoiceArrowPosition)
                {
                    case ActionChoiceArrowPosition.Fight:
                        CurrentPlayerTurnPhase = PlayerTurnPhase.ChoosingAttack;
                        _attackChoiceArrow.SetPosition(PreviousPlayerAttack);
                        break;
                    case ActionChoiceArrowPosition.Pokemon:
                        CurrentPlayerTurnPhase = PlayerTurnPhase.ChoosingPokemon;
                        break;
                    case ActionChoiceArrowPosition.Bag:
                        CurrentPlayerTurnPhase = PlayerTurnPhase.ChoosingItem;
                        break;
                    case ActionChoiceArrowPosition.Run:
                        CurrentPlayerTurnPhase = PlayerTurnPhase.Running;
                        break;
                }

                _actionMenu.Visible = false;
                _actionChoiceArrow.Visible = false;
            }
        }

        public void ChoosingAttack(GameTime gameTime)
        {
            _textPanel.BattleText.FirstLine = "";
            _textPanel.BattleText.SecondLine = "";

            _attackMenu.SetPokemon(ActiveAlliedPokemon.PokemonInstance);
            _attackMenu.Visible = true;
            _attackChoiceArrow.Visible = true;

            if (InputHandler.ActionKeyPressed(ActionKey.Back, PlayerIndex.One))
            {
                // Return to action menu
                CurrentPlayerTurnPhase = PlayerTurnPhase.ChoosingAction;
                _attackMenu.Visible = false;
                _attackChoiceArrow.Visible = false;
            }
            else if (InputHandler.ActionKeyPressed(ActionKey.ConfirmAndInteract, PlayerIndex.One))
            {
                bool validMoveChosen = false;

                switch (_attackChoiceArrow.AttackChoiceArrowPosition)
                {
                    case AttackChoiceArrowPosition.Move1:
                        if (ActiveAlliedPokemon.PokemonInstance.Moves[0].RemainingPP > 0)
                        {
                            PlayerChosenAttack = AttackChoiceArrowPosition.Move1;
                            validMoveChosen = true;
                        }
                        break;
                    case AttackChoiceArrowPosition.Move2:
                        if (ActiveAlliedPokemon.PokemonInstance.Moves[1].RemainingPP > 0)
                        {
                            PlayerChosenAttack = AttackChoiceArrowPosition.Move2;
                            validMoveChosen = true;
                        }
                        break;
                    case AttackChoiceArrowPosition.Move3:
                        if (ActiveAlliedPokemon.PokemonInstance.Moves[2].RemainingPP > 0)
                        {
                            PlayerChosenAttack = AttackChoiceArrowPosition.Move3;
                            validMoveChosen = true;
                        }
                        break;
                    case AttackChoiceArrowPosition.Move4:
                        if (ActiveAlliedPokemon.PokemonInstance.Moves[3].RemainingPP > 0)
                        {
                            PlayerChosenAttack = AttackChoiceArrowPosition.Move4;
                            validMoveChosen = true;
                        }
                        break;
                }

                if (validMoveChosen)
                {
                    CurrentPhase = BattlePhase.EnemyTurn;
                    CurrentPlayerTurnPhase = PlayerTurnPhase.ChoosingAction;
                    PreviousPlayerAttack = PlayerChosenAttack;

                    _attackMenu.Visible = false;
                    _attackChoiceArrow.Visible = false;
                }
            }
        }


        /// <summary>
        /// Handles the update logic for the ally EnemyTrainer turn
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the gametime</param>
        public void AllyTurn(GameTime gameTime)
        {
            switch (CurrentPlayerTurnPhase)
            {
                case PlayerTurnPhase.ChoosingAction:
                    ChoosingAction(gameTime);
                    break;
                case PlayerTurnPhase.ChoosingAttack:
                    ChoosingAttack(gameTime);
                    break;  
                case PlayerTurnPhase.ChoosingPokemon:
                    ChoosingPokemon(gameTime);
                    break;
                case PlayerTurnPhase.ChoosingItem:
                    ChoosingItem(gameTime);
                    break;
                case PlayerTurnPhase.Running:
                    TryToRun(gameTime);
                    break;
            }
        }

        private void ChoosingItem(GameTime gameTime)
        {
            _textPanel.BattleText.FirstLine = "You have no items in your bag!";
            _textPanel.BattleText.SecondLine = "";

            _textPanel.TextPromptArrow.WaitingForTextToAppear(gameTime);

            if (_textPanel.TextPromptArrow.State == TextArrowState.Clicked)
            {
                CurrentPlayerTurnPhase = PlayerTurnPhase.ChoosingAction;
            }
        }

        private void ChoosingPokemon(GameTime gameTime)
        {
            _textPanel.BattleText.FirstLine = "You have forgotten how to change";
            _textPanel.BattleText.SecondLine = "your active Pokemon!";

            _textPanel.TextPromptArrow.WaitingForTextToAppear(gameTime);

            if (_textPanel.TextPromptArrow.State == TextArrowState.Clicked)
            {
                CurrentPlayerTurnPhase = PlayerTurnPhase.ChoosingAction;
            }
        }

        private void TryToRun(GameTime gameTime)
        {
            if(!EnemyTrainer.Name.Equals("Tall Grass"))
            {
                _textPanel.BattleText.FirstLine = "You can't run from an enemy trainer!";
                _textPanel.BattleText.SecondLine = "";
                _textPanel.TextPromptArrow.WaitingForTextToAppear(gameTime);

                if (_textPanel.TextPromptArrow.State == TextArrowState.Clicked)
                    CurrentPlayerTurnPhase = PlayerTurnPhase.ChoosingAction;
            }
            else
            {
                CurrentPhase = BattlePhase.Ending;
                BattleConclusion = BattleConclusion.PlayerRan;
            }
        }


        /// <summary>
        /// Handles the update logic for the enemy EnemyTrainer turn
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the gametime</param>
        public void EnemyTurn(GameTime gameTime)
        {
            EnemyChosenAttackIndex = Random.Next(0, ActiveEnemyPokemon.PokemonInstance.Moves.Count);

            CurrentPhase = BattlePhase.DetermineFirstToPerformAction;
        }


        /// <summary>
        /// Lets Enemy pokemon perform it's action
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the gametime</param>
        public void EnemyPerformsAttack(GameTime gameTime)
        {
            if (AttackState == AttackState.Undecided)
            {
                ActiveEnemyPokemon.PokemonInstance.AnimationState = AttackAnimationState.Ready;
                _textPanel.BattleText.FirstLine = "Enemy " + ActiveEnemyPokemon.PokemonInstance.Archetype.Name + " used " + ActiveEnemyPokemon.PokemonInstance.Moves[EnemyChosenAttackIndex].Archetype.Name + "!";
                CalculateHitOrMiss(ActiveEnemyPokemon.PokemonInstance.Moves[EnemyChosenAttackIndex], ActiveEnemyPokemon.PokemonInstance);
            }

            AttackTimer += gameTime.ElapsedGameTime.Milliseconds;

            if (AttackTimer >= AttackTimerLength)
            {
                if (AttackState == AttackState.Hit)
                {
                    if (ActiveEnemyPokemon.PokemonInstance.AnimationState == AttackAnimationState.Ready)
                        ActiveEnemyPokemon.PokemonInstance.AnimationState = AttackAnimationState.Starting;

                    if (ActiveEnemyPokemon.PokemonInstance.FrameCounter == ActiveEnemyPokemon.PokemonInstance.FramesToApex)
                    {
                        ProcessHit(ActiveEnemyPokemon.PokemonInstance.Moves[EnemyChosenAttackIndex], ActiveEnemyPokemon.PokemonInstance);
                        AudioController.RequestTrack("hit").Play();
                    }

                    _textPanel.TextPromptArrow.WaitingForTextToAppear(gameTime, 1000);
                    if (_textPanel.TextPromptArrow.State == TextArrowState.Clicked)
                    {
                        CurrentPhase = CurrentPhase == BattlePhase.EnemyPerformsActionFirst ? BattlePhase.AllyPerformsActionSecond : BattlePhase.AllyTurn;

                        AttackTimer = 0;
                        ActiveEnemyPokemon.PokemonInstance.AnimationState = AttackAnimationState.Ready;
                        AttackState = AttackState.Undecided;
                    }
                }
                else
                {
                    _textPanel.BattleText.FirstLine = "But it failed!";

                    _textPanel.TextPromptArrow.WaitingForTextToAppear(gameTime, 100);   

                    if (_textPanel.TextPromptArrow.State == TextArrowState.Clicked)
                    {

                        CurrentPhase = CurrentPhase == BattlePhase.EnemyPerformsActionFirst ? BattlePhase.AllyPerformsActionSecond : BattlePhase.AllyTurn;
                        AttackTimer = 0;
                        AttackState = AttackState.Undecided;
                    }
                }
            }  
        }

        private void ProcessHit(CombatMove combatMove, PokemonInstance origin) {
            PokemonInstance target = null;

            switch (combatMove.Archetype.MoveTarget) {
                case MoveTarget.ActiveEnemy:
                    target = origin == ActiveEnemyPokemon.PokemonInstance ? ActiveAlliedPokemon.PokemonInstance : ActiveEnemyPokemon.PokemonInstance;
                    break;
                case MoveTarget.Self:
                    target = origin;
                    break;
            }

            if (target != null && combatMove.Archetype.BattleEffectType == BattleEffectType.DamageOnly) {
                var moveElement = combatMove.Archetype.ElementType;
                var targetPrimaryElement = target.Archetype.PrimaryType;
                var targetSecondaryElement = target.Archetype.SecondaryType;

                // Weaknesses and resistances
                var damageModifier = DataManager.GetEffectiveness(moveElement, targetPrimaryElement) * DataManager.GetEffectiveness(moveElement, targetSecondaryElement);

                // Random modifier
                damageModifier *= (Random.Next(85, 101) + 0f)/100; 

                // Same Type Attack Bonus (STAB)
                if (moveElement == origin.Archetype.PrimaryType || moveElement == origin.Archetype.SecondaryType)
                    damageModifier *= 1.5f;

                // Critical hit
                if (Random.Next(0, 10001) <= 625)
                    damageModifier *= 2;

                var attack = combatMove.Archetype.MoveType == MoveType.Physical
                                 ? origin.CombatStats[Stats.Attack].CurrentStatValue
                                 : origin.CombatStats[Stats.SpecialAttack].CurrentStatValue;

                var defense = combatMove.Archetype.MoveType == MoveType.Physical
                                  ? target.CombatStats[Stats.Defense].CurrentStatValue
                                  : target.CombatStats[Stats.SpecialDefense].CurrentStatValue;

                var appliedDamage = (int) Math.Ceiling((((2*origin.Level + 10)/(float)250)*(attack/defense)*combatMove.Archetype.Power + 2)*
                                                       damageModifier);

                if (target.CurrentHealth < appliedDamage)
                    appliedDamage = target.CurrentHealth;

                //Console.WriteLine(appliedDamage);

                target.ApplyDamage(appliedDamage);

                //Console.WriteLine(ActiveEnemyPokemon.CurrentHealth + "," + ActiveAlliedPokemon.CurrentHealth);
            }    
        }

        private void CalculateHitOrMiss(CombatMove combatMove, PokemonInstance origin)
        {
            combatMove.RemainingPP--;

            PokemonInstance target = null;

            switch (combatMove.Archetype.MoveTarget)
            {
                case MoveTarget.ActiveEnemy:
                    target = origin == ActiveEnemyPokemon.PokemonInstance ? ActiveAlliedPokemon.PokemonInstance : ActiveEnemyPokemon.PokemonInstance;
                    break;
                case MoveTarget.Self:
                    target = origin;
                    break;
            }

            if (target != null && combatMove.Archetype.BattleEffectType == BattleEffectType.DamageOnly)
            {
                var hitThreshold = Math.Ceiling(target.CombatStats[Stats.Evasion].CurrentStatValue/100f * origin.CombatStats[Stats.Accuracy].CurrentStatValue/100f) * combatMove.Archetype.Accuracy;

                AttackState = Random.Next(0,101) > hitThreshold ? AttackState.Miss : AttackState.Hit;
            }
        }


        /// <summary>
        /// Lets allied pokemon perform it's action
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the gametime</param>
        public void AllyPerformsAttack(GameTime gameTime)
        {

            if (AttackState == AttackState.Undecided)
            {
                ActiveAlliedPokemon.PokemonInstance.AnimationState = AttackAnimationState.Ready;
                _textPanel.BattleText.FirstLine = ActiveAlliedPokemon.PokemonInstance.Archetype.Name + " used " + ActiveAlliedPokemon.PokemonInstance.Moves[(int)PlayerChosenAttack].Archetype.Name + "!";
                CalculateHitOrMiss(ActiveAlliedPokemon.PokemonInstance.Moves[(int)PlayerChosenAttack], ActiveAlliedPokemon.PokemonInstance);
            }

            AttackTimer += gameTime.ElapsedGameTime.Milliseconds;

            if (AttackTimer >= AttackTimerLength)
            {
                if (AttackState == AttackState.Hit)
                {
                    if (ActiveAlliedPokemon.PokemonInstance.AnimationState == AttackAnimationState.Ready)
                        ActiveAlliedPokemon.PokemonInstance.AnimationState = AttackAnimationState.Starting;

                    if (ActiveAlliedPokemon.PokemonInstance.FrameCounter == ActiveAlliedPokemon.PokemonInstance.FramesToApex)
                    {
                        ProcessHit(ActiveAlliedPokemon.PokemonInstance.Moves[(int)PlayerChosenAttack], ActiveAlliedPokemon.PokemonInstance);
                        AudioController.RequestTrack("hit").Play();
                    }

                    _textPanel.TextPromptArrow.WaitingForTextToAppear(gameTime, 1000);
                    if (_textPanel.TextPromptArrow.State == TextArrowState.Clicked)
                    {

                        CurrentPhase = CurrentPhase == BattlePhase.AllyPerformsActionFirst ? BattlePhase.EnemyPerformsActionSecond : BattlePhase.AllyTurn;

                        AttackTimer = 0;
                        ActiveAlliedPokemon.PokemonInstance.AnimationState = AttackAnimationState.Ready;
                        AttackState = AttackState.Undecided;
                    }
                }
                else
                {
                    _textPanel.BattleText.FirstLine = "But it failed!";

                    _textPanel.TextPromptArrow.WaitingForTextToAppear(gameTime, 100);

                    if (_textPanel.TextPromptArrow.State == TextArrowState.Clicked)
                    {
                        CurrentPhase = CurrentPhase == BattlePhase.AllyPerformsActionFirst ? BattlePhase.EnemyPerformsActionSecond : BattlePhase.AllyTurn;
                        AttackTimer = 0;
                        AttackState = AttackState.Undecided;
                    }
                }
            }     
        }


        public override void Draw(GameTime gameTime)
        {
            var game = (Game1)Game;

            // DEBUG

            //Console.Clear();
            //Console.WriteLine(CurrentPhase);
            //Console.WriteLine(CurrentPlayerTurnPhase);
            //Console.WriteLine(BattleConclusion);
            //Console.WriteLine(EndStage);

            // END


            // Draw all UI components
            game.SpriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null,
                null,
                Camera.Transformation);

            foreach (var component in DrawableComponents.Where(component => component.Visible))
            {
                component.Draw(game.SpriteBatch);
            }

            //game.SpriteBatch.Draw(Frame, new Vector2(-64,-64), Color.White);

            game.SpriteBatch.End();
        }

        // Clean up after the battle
        public void Ending(GameTime gameTime)
        {
            switch (EndStage)
            {
                case EndStage.DisplayResult:
                    DisplayResult(gameTime);
                    break;    
                case EndStage.DisplayExerienceGain:
                    DisplayExperienceGain(gameTime);
                    break;
                case EndStage.DisplayExit:
                    DisplayExit(gameTime);
                    break;
            }
        }

        private void DisplayExperienceGain(GameTime gameTime)
        {
            if (RewardState == RewardState.ExperienceToBeAwarded)
            {
                int experienceGained = AwardExperience();
                _textPanel.BattleText.FirstLine = "Gained " + experienceGained + " experience!";
                RewardState = RewardState.ExperienceAwarded;
            }

            _textPanel.TextPromptArrow.WaitingForTextToAppear(gameTime, 2000);

            if (_textPanel.TextPromptArrow.State == TextArrowState.Clicked)
            {
                EndStage = EndStage.DisplayExit;
            }
        }

        private void DisplayExit(GameTime gameTime)
        {

            _textPanel.BattleText.FirstLine = "Returning to world map...";
            _textPanel.BattleText.SecondLine = "";

            _textPanel.TextPromptArrow.WaitingForTextToAppear(gameTime, 2000);

            if (_textPanel.TextPromptArrow.State == TextArrowState.Clicked)
            {
                OnExit();
            }
        }

        public void DisplayResult(GameTime gameTime)
        {
            switch (BattleConclusion)
            {
                case BattleConclusion.PlayerRan:
                    _textPanel.BattleText.FirstLine = "Run, Forrest, run!";
                    _textPanel.BattleText.SecondLine = "";
                    break;
                case BattleConclusion.PlayerWon:
                    _textPanel.BattleText.FirstLine = "You won!";
                    _textPanel.BattleText.SecondLine = "";
                    break;
                case BattleConclusion.PlayerLost:
                    _textPanel.BattleText.FirstLine = "You lost!";
                    _textPanel.BattleText.SecondLine = "";
                    break;
            }

            _textPanel.TextPromptArrow.WaitingForTextToAppear(gameTime, 2000);

            if (_textPanel.TextPromptArrow.State == TextArrowState.Clicked)
            {               
                if (BattleConclusion == BattleConclusion.PlayerWon)
                    EndStage = EndStage.DisplayExerienceGain;
                else
                    EndStage = EndStage.DisplayExit;
            }
        }

        public int AwardExperience()
        {
            var trainerOwnedPokemon = _trainerBattle ? 1.5f : 1f;
            var tradedPokemon = ActiveAlliedPokemon.PokemonInstance.OriginalTrainer == AlliedTrainer ? 1f : 1.5f;
            var baseExperienceGain = 100f;
            var faintedPokemonLevel = ActiveEnemyPokemon.PokemonInstance.Level;

            var awardedExperience = (int) Math.Ceiling((trainerOwnedPokemon*tradedPokemon*baseExperienceGain*faintedPokemonLevel)/7);

            ActiveAlliedPokemon.PokemonInstance.AwardExperience(awardedExperience);

            return awardedExperience;
        }
    }  
}
