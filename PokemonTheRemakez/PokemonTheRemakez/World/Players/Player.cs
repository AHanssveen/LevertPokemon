using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PokemonTheRemakez.BattleSystem;
using PokemonTheRemakez.BattleSystem.Screens.Elements;
using PokemonTheRemakez.BattleSystem.Trainers;
using PokemonTheRemakez.World.Audio;
using PokemonTheRemakez.World.Collisions;
using PokemonTheRemakez.World.Input;
using PokemonTheRemakez.World.Maps;
using PokemonTheRemakez.World.Screens;
using PokemonTheRemakez.World.Sprites;
using PokemonTheRemakez.World.TileEngine;
using PokemonTheRemakez.World.Trainers;

namespace PokemonTheRemakez.World.Players {

    public enum TextDisplayState {
        TextDisplaying,
        TextNotDisplaying
    }

    public enum MoveState {
        Still,
        Moving,
        Frozen
    }

    public enum GameState
    {
        Active,
        Paused,
        Victory,
        GameOver
    }

    /// <summary>
    /// Handles everything related to the Player, including
    /// - Input and movement
    /// - Camera control
    /// - Collision checks with the ParentState
    /// - Animation
    /// </summary>
    public class Player : DrawableBattleComponent {
        public TextPanel TextPanel;
        public Trainer PlayerTrainer;
        private EnemyTrainer _encounteredEnemyTrainer;
        private TextDisplayState _textDisplayState = TextDisplayState.TextNotDisplaying;
        private bool _justConfirmedPrompt;
        private GameState _gameState = GameState.Active;

        /// <summary>
        /// The world map the Player moves inside. Used to check for collisions.
        /// </summary>
        private readonly Map _map;
        
        /// <summary>
        /// The camera following the Player
        /// </summary>
        public Camera Camera;

        /// <summary>
        /// The instance of the running game. Provides access to members defined in Game1.  
        /// </summary>
        private readonly Game1 _gameRef;

        /// <summary>
        /// An object containing all the textures needed to fully animate the
        /// player and control the selection and flow of animations.
        /// </summary>
        public PlayerAnimatedSprite Sprite;

        // ******************
        // Movement variables
        // ******************
        public MoveState MoveState = MoveState.Still;

        private CollisionType _collision = CollisionType.Walkable;

        private FrameKey _lastFoot = FrameKey.RightFoot;

        private bool _movementJustFinished;

        private AnimationKey _previousMovementDirection = AnimationKey.Down;

        /// <summary>
        /// Stores the motion vector until the movement is finished
        /// </summary>
        private Vector2 _movementVector;

        /// <summary>
        /// Duration of movement animation
        /// </summary>
        private const int FramesPerMovement = 18;               // DEFAULT = 18
        private int _framesPerMovement = FramesPerMovement;        

        /// <summary>
        /// Counter for movement animation duration
        /// </summary>
        private int _frameCounter;

        /// <summary>
        /// Used to check if player has entered a new tile
        /// </summary>
        private bool _encounterChecked;
        private MoveState moveState;
        private MoveState PreviousMoveState;

        // UI Components
        private Pause _pauseScreen;

        public Player(Game game, Map map) : base(game)
        {
            _map = map;
            _gameRef = (Game1)game;
            Camera = new Camera(_gameRef.ScreenRectangle);
            game.Components.Add(this);
        }

        /// <summary>
        /// Loads all content used by the player
        /// </summary>
        protected override void LoadContent()
        {
            // Hero
            var spriteSheet = Game.Content.Load<Texture2D>(@"BoyHeroWalk");
            var animations = new Dictionary<AnimationKey, PlayerAnimation>();

            var animation = new PlayerAnimation(new[]
                {
                    new Rectangle(0, 0, 16, 32),
                    new Rectangle(16, 0, 16, 32),
                    new Rectangle(32, 0, 16, 32)
                });
            animations.Add(AnimationKey.Down, animation);

            animation = new PlayerAnimation(new[]
                {
                    new Rectangle(0, 32, 16, 32),
                    new Rectangle(16, 32, 16, 32),
                    new Rectangle(32, 32, 16, 32)
                });
            animations.Add(AnimationKey.Up, animation);


            animation = new PlayerAnimation(new[]
                {
                    new Rectangle(0, 64, 16, 32),
                    new Rectangle(16, 64, 16, 32),
                    new Rectangle(32, 64, 16, 32)
                });
            animations.Add(AnimationKey.Left, animation);

            animation = new PlayerAnimation(new[]
                {
                    new Rectangle(0, 96, 16, 32),
                    new Rectangle(16, 96, 16, 32),
                    new Rectangle(32, 96, 16, 32)
                });
            animations.Add(AnimationKey.Right, animation);

            Sprite = new PlayerAnimatedSprite(spriteSheet, animations) {Position = new Vector2(96, 96)};

            TextPanel = new TextPanel(Game, 0, 0, true) {Visible = false};
            Components.Add(TextPanel);

            _pauseScreen = new Pause(Game, 0, 0);
            Components.Add(_pauseScreen);

            base.LoadContent();
        }

        /// <summary>
        /// Displays text
        /// </summary>
        /// <param name="gameTime">provides a snapshot of the gametime</param>
        private void DisplayText(GameTime gameTime) {
            TextPanel.Visible = true;
            TextPanel.TextPromptArrow.Visible = false;

            TextPanel.TextPromptArrow.WaitingForTextToAppear(gameTime);

            if (GamePlayScreen.Player.TextPanel.TextPromptArrow.State == TextArrowState.Clicked) {
                GamePlayScreen.Player.TextPanel.Visible = false;
                _textDisplayState = TextDisplayState.TextNotDisplaying;
                _justConfirmedPrompt = true;
                TextPanel.BattleText.FirstLine = "";
                TextPanel.BattleText.SecondLine = "";
                MoveState = MoveState.Still;
            }
        }
        
        /// <summary>
        /// Freezes the screen
        /// </summary>
        /// <param name="gameTime">provides a snapshot of the gametime</param>
        public void PauseScreen(GameTime gameTime)
        {
            _pauseScreen.Visible = true;

            if (InputHandler.ActionKeyPressed(ActionKey.Pause, PlayerIndex.One))
            {
                _gameState = GameState.Active;
                _pauseScreen.Visible = false;
            }
            else if (InputHandler.ActionKeyPressed(ActionKey.Exit, PlayerIndex.One))
                Game.Exit();
        }

        /// <summary>
        /// Starts the game
        /// </summary>
        /// <param name="gameTime">provides a snapshot of the gametime</param>
        public void PlayGame(GameTime gameTime)
        {
            switch (_textDisplayState)
            {
                case TextDisplayState.TextDisplaying:
                    DisplayText(gameTime);
                    break;
            }

            PreviousMoveState = moveState;

            moveState = MoveState;
            if (PreviousMoveState != moveState)
                if (moveState == MoveState.Still)
                    _encounterChecked = false;

            var motion = new Vector2();

            // Get input if the Player is not already moving 
            if (MoveState == MoveState.Still)
            {
                // Pause
                if (InputHandler.ActionKeyPressed(ActionKey.Pause, PlayerIndex.One))
                    _gameState = GameState.Paused;

                // DEBUG
                // Start combat
                if (InputHandler.ActionKeyPressed(ActionKey.Back, PlayerIndex.One))
                {
                    DataManager.RandomWildPokemon();
                    _gameRef.BattleScreen.InitializeBattle(DataManager.Trainers["Trond"], DataManager.Trainers["Tall Grass"]);
                }

                //If last movement brought you onto a trigger tile
                if (_collision == CollisionType.TrainerTriggerBush)
                    GamePlayScreen.Trainers["Sabrina"].TriggerTrainer(AnimationKey.Right);
                else if (_collision == CollisionType.TrainerTrigger)
                    GamePlayScreen.Trainers["Giovanni"].TriggerTrainer(AnimationKey.Right);
                else if (_collision == CollisionType.HealingHerb && InputHandler.ActionKeyPressed(ActionKey.ConfirmAndInteract, PlayerIndex.One) && !_justConfirmedPrompt)
                {
                    foreach (var pokemonNr in PlayerTrainer.PokemonSet)
                        pokemonNr.FullRestore();

                    TextPanel.BattleText.FirstLine = "Your pokemon ate some herbs.";
                    TextPanel.BattleText.SecondLine = "They feel healthy!";
                    MoveState = MoveState.Frozen;
                    _textDisplayState = TextDisplayState.TextDisplaying;
                }
                //Decides when to encounter wild pokemon
                else if (_collision == CollisionType.Bush && !_encounterChecked)
                {
                    _encounterChecked = true;
                    var rand = new Random();
                    int tau = rand.Next(200);
                    //Console.WriteLine(tau);
                    if (tau < 18)
                    {
                        DataManager.RandomWildPokemon();
                        _gameRef.BattleScreen.InitializeBattle(DataManager.Trainers["Trond"], DataManager.Trainers["Tall Grass"]);
                        return;
                    }
                }

                _justConfirmedPrompt = false;

                // Check for sprint
                if (InputHandler.ActionKeyDown(ActionKey.Sprint, PlayerIndex.One))
                {
                    _framesPerMovement = FramesPerMovement / 2;
                }
                else
                {
                    _framesPerMovement = FramesPerMovement;
                }

                // Check for EnemyTrainer
                if (InputHandler.ActionKeyPressed(ActionKey.ConfirmAndInteract, PlayerIndex.One))
                {
                    var checkPoint = new Vector2(Sprite.Position.X + (float)Sprite.Width / 2 + _movementVector.X * Sprite.Speed,
                                                 Sprite.Position.Y + (float)Sprite.Height / 2 * 1.5f +
                                                 _movementVector.Y * Sprite.Speed);

                    // Check for EnemyTrainer
                    _map.CheckForCollisions(checkPoint, ref _encounteredEnemyTrainer);

                    if (_encounteredEnemyTrainer != null && _encounteredEnemyTrainer.CurrentState != EnemyTrainerState.BattleFinished)
                        _encounteredEnemyTrainer.TriggerTrainer(Sprite.CurrentAnimation);


                } 
                else if (InputHandler.ActionKeyDown(ActionKey.Up, PlayerIndex.One))
                {
                    Sprite.CurrentAnimation = AnimationKey.Up;
                    motion.Y = -1;
                    _movementVector = motion;
                }
                else if (InputHandler.ActionKeyDown(ActionKey.Down, PlayerIndex.One))
                {
                    Sprite.CurrentAnimation = AnimationKey.Down;
                    motion.Y = 1;
                    _movementVector = motion;
                }
                else if (InputHandler.ActionKeyDown(ActionKey.Left, PlayerIndex.One))
                {
                    Sprite.CurrentAnimation = AnimationKey.Left;
                    motion.X = -1;
                    _movementVector = motion;
                }
                else if (InputHandler.ActionKeyDown(ActionKey.Right, PlayerIndex.One))
                {
                    Sprite.CurrentAnimation = AnimationKey.Right;
                    motion.X = 1;
                    _movementVector = motion;
                }
                else if (_movementJustFinished)
                {
                    _movementJustFinished = false;
                    Sprite.SetCurrentAnimationFrame(FrameKey.Idle);
                }
            }

            // If the player moves in a new direction
            if (Sprite.CurrentAnimation != _previousMovementDirection && _collision == CollisionType.Unwalkable)
                _collision = CollisionType.Walkable;

            // If the player is not already moving AND the player has initiated movement AND didn't previously try to move onto an unwalkable tile
            if (MoveState == MoveState.Still &&
                motion != Vector2.Zero &&
                _collision != CollisionType.Unwalkable)
            {
                var checkPoint = new Vector2(Sprite.Position.X + (float)Sprite.Width / 2 + motion.X * Sprite.Speed,
                                          Sprite.Position.Y + (float)Sprite.Height / 2 * 1.5f +
                                          motion.Y * Sprite.Speed);

                // Check for collisions
                _collision = _map.CheckForCollisions(checkPoint, ref _encounteredEnemyTrainer);


                if (_collision != CollisionType.Unwalkable)
                {
                    MoveState = MoveState.Moving;
                    Sprite.IsAnimating = true;
                }
                else
                {
                    _previousMovementDirection = Sprite.CurrentAnimation;
                }
            }

            // Process movement if movement is initiated
            if (MoveState == MoveState.Moving)
            {
                // FIRST FRAME: Proceed to the next animation frame
                if (_frameCounter == 0)
                {
                    if (_lastFoot == FrameKey.RightFoot)
                    {
                        Sprite.SetCurrentAnimationFrame(FrameKey.LeftFoot);
                        _lastFoot = FrameKey.LeftFoot;
                    }
                    else
                    {
                        Sprite.SetCurrentAnimationFrame(FrameKey.RightFoot);
                        _lastFoot = FrameKey.RightFoot;
                    }

                }

                // Increment the frame counter
                _frameCounter++;

                // Update the position of the sprite
                Sprite.Position += _movementVector * Sprite.Speed / _framesPerMovement;

                // MIDDLE OF MOVEMENT: Proceed to the next animation frame
                if (_frameCounter == _framesPerMovement / 2)
                    Sprite.SetCurrentAnimationFrame(FrameKey.Idle);

                // MOVEMENT FINISHED
                if (_frameCounter == _framesPerMovement)
                {
                    _movementJustFinished = true;

                    // Set position of the sprite to integers
                    Sprite.Position = new Vector2((int)Math.Round(Sprite.Position.X), (int)Math.Round(Sprite.Position.Y));

                    // Reset the frame counter
                    _frameCounter = 0;

                    // Not moving anymore
                    MoveState = MoveState.Still;

                    // Not animating anymore
                    Sprite.IsAnimating = false;

                    // Save the direction of the movement
                    _previousMovementDirection = Sprite.CurrentAnimation;
                }

                base.Update(gameTime);
            }

            Camera.LockToSprite(Sprite);

            Camera.Update(gameTime);

            // DEBUG
            //Console.Clear();
            //Console.WriteLine("Zone: " + (_map.CurrentMapComponent != null ? _map.CurrentMapComponent.Name : ""));
            //Console.WriteLine("Collision: " + _collision);
            //Console.WriteLine("Moving: " + MoveState);
            //Console.WriteLine("EnemyTrainer: " + (_encounteredEnemyTrainer != null ? _encounteredEnemyTrainer.Name : ""));

            foreach (var drawableBattleComponent in Components)
            {
                drawableBattleComponent.Position = Sprite.Position;
                drawableBattleComponent.Update(gameTime);
            }               
        }

        /// <summary>
        /// updates for drawing
        /// </summary>
        /// <param name="gameTime">provides a snapshot of the gametime</param>
        public override void Update(GameTime gameTime) {
            switch (_gameState)
            {
                case GameState.Active:
                    PlayGame(gameTime);
                    break;
                case GameState.Paused:
                    PauseScreen(gameTime);
                   break;                 
            }
        }

        /// <summary>
        /// draws everything to the screen
        /// </summary>
        /// <param name="gameTime">provides a snapshot of the gametime</param>
        /// <param name="spriteBatch">instance of spritebatch</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            Sprite.Draw(gameTime, spriteBatch);

            foreach (var drawableBattleComponent in Components.Where(drawableBattleComponent => drawableBattleComponent.Visible)) {
                drawableBattleComponent.Draw(spriteBatch, new Vector2(-110, 50));
            }

            // DEBUG
            //Console.Clear();
            //Console.WriteLine(_sprite.Position);

            //var colBox = new Texture2D(Game.GraphicsDevice, 1, 1);
            //colBox.SetData(new[] { Color.Blue });
            //spriteBatch.Draw(colBox, new Rectangle((int) _checkPoint.X, (int) _checkPoint.Y, 10, 10), Color.White);
        }
    }
}
