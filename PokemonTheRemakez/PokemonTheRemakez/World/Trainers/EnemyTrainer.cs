using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.BattleSystem;
using PokemonTheRemakez.BattleSystem.Screens.Elements;
using PokemonTheRemakez.World.Audio;
using PokemonTheRemakez.World.Elements;
using PokemonTheRemakez.World.Input;
using PokemonTheRemakez.World.Players;
using PokemonTheRemakez.World.Screens;
using PokemonTheRemakez.World.Sprites;

namespace PokemonTheRemakez.World.Trainers
{
    public enum EnemyTrainerState
    {
        PlayerNotDiscovered,
        PlayerDiscovered,
        TalkingToPlayer,
        StartingBattle,
        BattleFinished
    }
    /// <summary>
    /// Enemys trainer contructor
    /// </summary>
    public class EnemyTrainer : DrawableBattleComponent
    {
        public Dictionary<AnimationKey, Rectangle> Frames = new Dictionary<AnimationKey, Rectangle>(); 
        public AnimationKey CurrentFrame;

        public readonly String Name, TrainerEnemySayingL1, TrainerEnemySayingL2;

        private Texture2D _enemyTrainerTextureSmall;
        private int _timer;

        public EnemyTrainerState CurrentState;
        
        private BlackFlash _blackFlash;
        private ExclamationBubble _exclamationBubble;

        public Rectangle CollisionRectangle;
        /// <summary>
        /// Deals with enemy trainer logic
        /// </summary>
        /// <param name="game">Game state</param>
        /// <param name="name">Takes trainer name</param>
        /// <param name="trainerEnemySayingL1">Takes what the trainer says</param>
        /// <param name="trainerEnemySayingL2">Takes what the trainer says</param>
        /// <param name="x">Trainer posistion X</param>
        /// <param name="y">Trainer posistion Y</param>
        /// <param name="key"> Takes which animation to run</param>
        public EnemyTrainer(Game game, String name, String trainerEnemySayingL1, String trainerEnemySayingL2, int x, int y, AnimationKey key) : base(game)
        {
            Position = new Vector2(x,y);
            Name = name;
            TrainerEnemySayingL1 = trainerEnemySayingL1;
            TrainerEnemySayingL2 = trainerEnemySayingL2;

            Frames.Add(AnimationKey.Down, new Rectangle(0, 0, 16, 32));

            Frames.Add(AnimationKey.Left, new Rectangle(16, 0, 16, 32));

            Frames.Add(AnimationKey.Up, new Rectangle(32, 0, 16, 32));

            if (key == AnimationKey.Left)
                CurrentFrame = AnimationKey.Left;
            else if (key == AnimationKey.Down)
                CurrentFrame = AnimationKey.Down;

            CollisionRectangle = new Rectangle(x, y + 16, 16, 16);

            game.Components.Add(this);
        }
        /// <summary>
        /// Loads Enemy trainers content
        /// </summary>
        protected override void LoadContent()
        {
            // Large texture of the enemy trainer: 
            //_enemyTrainerTextureLarge = Game.Content.Load<Texture2D>("Resources/Trainers/" + _enemyTrainerName + "Large");
            _enemyTrainerTextureSmall = Game.Content.Load<Texture2D>("Resources/Trainers/" + Name);

            CurrentState = EnemyTrainerState.PlayerNotDiscovered;

            _exclamationBubble = new ExclamationBubble(Game, (int) Position.X, (int) Position.Y) { Visible = false};
            _blackFlash = new BlackFlash(Game) { Visible = false };

            Components.Add(_exclamationBubble);
            Components.Add(_blackFlash);

            Position = Position;

            base.LoadContent();
        }
        /// <summary>
        /// Enemys trainers update function
        /// </summary>
        /// <param name="gameTime">Snapshot of the gametime</param>
        public override void Update(GameTime gameTime)
        {
            foreach (var drawableBattleComponent in Components.Where(drawableBattleComponent => drawableBattleComponent.Enabled)) {
                drawableBattleComponent.Update(gameTime);
            }

            switch (CurrentState)
            {
                case EnemyTrainerState.PlayerDiscovered:
                    _blackFlash.AnimationState = BlackFlashState.Inactive;
                    PlayerSeen();
                    break;
                case EnemyTrainerState.TalkingToPlayer:
                    TextAppearing(gameTime);
                    break;
                case EnemyTrainerState.StartingBattle:
                    StartBattle();
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// The player has been seen and an exclamation bubble apears over the npc
        /// </summary>
        public void PlayerSeen()
        {
            AudioController.PauseAll();
            AudioController.RequestTrack("trainerAppears").Play();
            _timer++;
            _exclamationBubble.Visible = true;

            if (_timer >= 60)
            {
                CurrentState = EnemyTrainerState.TalkingToPlayer;
                _exclamationBubble.Visible = false;
                _timer = 0;
            }
        }

        /// <summary>
        /// What the trainer says before the battle starts
        /// </summary>
        public void TextAppearing(GameTime gameTime)
        {
            GamePlayScreen.Player.TextPanel.Visible = true;
            GamePlayScreen.Player.TextPanel.TextPromptArrow.Visible = false;
            GamePlayScreen.Player.TextPanel.BattleText.FirstLine = TrainerEnemySayingL1;
            GamePlayScreen.Player.TextPanel.BattleText.SecondLine = TrainerEnemySayingL2;
            WaitingForTextToAppear(gameTime);

            if (GamePlayScreen.Player.TextPanel.TextPromptArrow.State == TextArrowState.Clicked)
            {
                AudioController.RequestTrack("trainerAppears").Stop();
                GamePlayScreen.Player.TextPanel.TextPromptArrow.State = TextArrowState.Inactive;
                CurrentState = EnemyTrainerState.StartingBattle;
                GamePlayScreen.Player.TextPanel.Visible = false;

            }
        }

        /// <summary>
        /// Flashes black, starts the battle song and gives control to the battle class
        /// </summary>
        public void StartBattle()
        {
            AudioController.PauseAll();
            AudioController.RequestTrack("battle").Play();
            if (_blackFlash.AnimationState == BlackFlashState.Inactive)
                _blackFlash.AnimationState = BlackFlashState.Active;

            if (_blackFlash.AnimationState == BlackFlashState.Finished) {
                ((Game1)Game).BattleScreen.InitializeBattle(GamePlayScreen.Player.PlayerTrainer,
                                                            DataManager.Trainers[Name]);
                CurrentState = EnemyTrainerState.BattleFinished;

            }
        }

        /// <summary>
        ///Makes you wait for reading the text and tests if you press enter and want to move on
        /// </summary>
        /// <param name="gameTime">Snapshot of the gametime</param>
        public void WaitingForTextToAppear(GameTime gameTime)
        {
            GamePlayScreen.Player.TextPanel.TextPromptArrow.State = TextArrowState.AwaitingClick;
            _timer += gameTime.ElapsedGameTime.Milliseconds;
            if (_timer >= 600)
            {
                GamePlayScreen.Player.TextPanel.TextPromptArrow.Visible = true;
                if (InputHandler.ActionKeyPressed(ActionKey.ConfirmAndInteract, PlayerIndex.One))
                {
                    GamePlayScreen.Player.TextPanel.TextPromptArrow.State = TextArrowState.Clicked;
                    GamePlayScreen.Player.TextPanel.TextPromptArrow.Visible = false;
                    _timer = 0;
                }
            }
            else
                GamePlayScreen.Player.TextPanel.TextPromptArrow.Visible = false;
        }
        /// <summary>
        /// Draws the enemy trainer
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(_enemyTrainerTextureSmall, Position, Frames[CurrentFrame], Color.White);

            foreach (var drawableBattleComponent in Components.Where(drawableBattleComponent => drawableBattleComponent.Visible)) {
                drawableBattleComponent.Draw(spriteBatch, new Vector2(-100, 50));
            }
        }
        /// <summary>
        /// What happens when you interact with enemy trainer
        /// </summary>
        /// <param name="currentAnimation"></param>
        public void TriggerTrainer(AnimationKey currentAnimation) {
            if (CurrentState == EnemyTrainerState.BattleFinished) return;
            GamePlayScreen.Player.MoveState = MoveState.Frozen;

            switch (currentAnimation) {
                case AnimationKey.Up:
                    CurrentFrame = AnimationKey.Down;
                    break;
            }

            CurrentState = EnemyTrainerState.PlayerDiscovered; 
        }
    }
}
