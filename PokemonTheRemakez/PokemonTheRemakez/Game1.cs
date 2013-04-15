using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.BattleSystem;
using PokemonTheRemakez.BattleSystem.Screens;
using PokemonTheRemakez.Intro.Screens;
using PokemonTheRemakez.Screens;
using PokemonTheRemakez.World.Audio;
using PokemonTheRemakez.World.Input;
using PokemonTheRemakez.World.Players;
using PokemonTheRemakez.World.Screens;
using PokemonTheRemakez.World.State;

namespace PokemonTheRemakez
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;

        public static GameStateManager StateManager;

        // Screens
        public GamePlayScreen GamePlayScreen;
        public BattleScreen BattleScreen;
        public IntroScreen IntroScreen;
        public GameOverScreen GameOverScreen;
        public VictoryScreen VictoryScreen;

        private const int ScreenWidth = 960;
        private const int ScreenHeight = 632;      

        public Rectangle ScreenRectangle = new Rectangle(0, 0, ScreenWidth, ScreenHeight);

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = ScreenWidth,
                PreferredBackBufferHeight = ScreenHeight 
            };

            IsFixedTimeStep = false;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;           

            var inputHandler = new InputHandler(this);
            Components.Add(inputHandler);
            Services.AddService((typeof(InputHandler)), inputHandler);

            var audioController = new AudioController(this);
            Components.Add(audioController);
            Services.AddService((typeof(AudioController)), audioController);

            StateManager = new GameStateManager(this);
            Components.Add(StateManager);

            GamePlayScreen = new GamePlayScreen(this, StateManager);
            BattleScreen = new BattleScreen(this, StateManager);
            IntroScreen = new IntroScreen(this, StateManager);
            GameOverScreen = new GameOverScreen(this, StateManager);
            VictoryScreen = new VictoryScreen(this, StateManager);

            StateManager.ChangeState(GamePlayScreen);   
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            var dataManager = new DataManager(this);

            GamePlayScreen.Player.PlayerTrainer = DataManager.Trainers["Trond"];
            IntroScreen.InitializeIntro();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
