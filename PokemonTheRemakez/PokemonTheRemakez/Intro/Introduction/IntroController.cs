using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PokemonTheRemakez.BattleSystem;
using PokemonTheRemakez.Intro.Components;
using PokemonTheRemakez.World.Audio;
using PokemonTheRemakez.World.Input;
using PokemonTheRemakez.World.TileEngine;

namespace PokemonTheRemakez.Intro.Introduction
{
    public enum IntroScreenState
        {
            StartScreenLayout,
            InfoScreenLayout,
            NameScreenLayout,
        }

    public class IntroController : DrawableGameComponent
    {
        private IntroScreenState _currentState;

        private StartScreen _startScreen;
        private StartButton _startButton;
        private LogoFlash _logoFlash;
        private Pikachu _pikachu;
        private InfoScreen _infoScreen;
        private NextButton _nextButton;
        private float _timer;
        public event EventHandler Exit;
        private InfoScreen _infoSn;

        public List<DrawableBattleComponent> DrawableComponents = new List<DrawableBattleComponent>();

        /// <summary>
        /// Tests if the intro should exit
        /// </summary>
        protected virtual void OnExit()
        {
            EventHandler handler = Exit;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// controls the intro
        /// </summary>
        /// <param name="game">provides an instance of game</param>
        public IntroController(Game game) : base(game)
        {
            LoadContent();
            _infoSn = new InfoScreen(game);
        }

        /// <summary>
        /// Loads the content used
        /// </summary>
        public new void LoadContent()
        {
            _startScreen = new StartScreen(Game) { Visible = false };
            DrawableComponents.Add(_startScreen);

            _logoFlash = new LogoFlash(Game) { Visible = false };
            DrawableComponents.Add(_logoFlash);

            _startButton = new StartButton(Game) { Visible = false };
            DrawableComponents.Add(_startButton);

            _infoScreen = new InfoScreen(Game) { Visible = false };
            DrawableComponents.Add(_infoScreen);

            _pikachu = new Pikachu(Game) { Visible = false };
            DrawableComponents.Add(_pikachu);

            _nextButton = new NextButton(Game) { Visible = false };
            DrawableComponents.Add(_nextButton);

            base.LoadContent();
        }

        /// <summary>
        /// update logic for drawing
        /// </summary>
        /// <param name="gameTime">snapshot of gametime</param>
        public override void Update(GameTime gameTime)
        {
            AudioController.RequestTrack("intro").Play();

            _startScreen.Visible = false;
            switch (_currentState)
            {
                case IntroScreenState.StartScreenLayout:
                    StartScreen(gameTime);
                    break;

                case IntroScreenState.InfoScreenLayout:
                    InfoScreen(gameTime);
                    break;
            }

            if (_infoScreen.IntroTextIndex == 5)
                OnExit();

            foreach (var drawableBattleComponent in DrawableComponents.Where(drawableBattleComponent => drawableBattleComponent.Visible))
                drawableBattleComponent.Update(gameTime);

             base.Update(gameTime);
        }
       
        /// <summary>
        /// What should be displayed when starting the game
        /// </summary>
        /// <param name="gameTime">snapshot of gametime</param>
        private void StartScreen(GameTime gameTime)
        {
            _startScreen.Visible = true;
            _logoFlash.Visible = true;
            _startButton.Visible = true;
            _timer += gameTime.ElapsedGameTime.Milliseconds;
            
            if(InputHandler.ActionKeyPressed(ActionKey.ConfirmAndInteract, PlayerIndex.One) && _timer >= 600)
            {
                _currentState = IntroScreenState.InfoScreenLayout; 
                _startScreen.Visible = false;
                _logoFlash.Visible = false;
                _startButton.Visible = false;
                _timer = 0;
            }
        }

        /// <summary>
        /// what should be displayed at the intro screen
        /// </summary>
        /// <param name="gameTime">snapshot of gametime</param>
        private void InfoScreen(GameTime gameTime)
        {
            _infoScreen.Visible = true;
            _pikachu.Visible = true;

            if (_infoScreen.DisplayNextArrow)
                _nextButton.Visible = true;
            else
                _nextButton.Visible = false;
        }

        /// <summary>
        /// What sould be drawn
        /// </summary>
        /// <param name="gameTime">snapshot of gametime</param>
        public override void Draw(GameTime gameTime)
        {
            var game = (Game1) Game;

            game.SpriteBatch.Begin();

            foreach (var drawableBattleComponent in DrawableComponents.Where(drawableBattleComponent => drawableBattleComponent.Visible))
                drawableBattleComponent.Draw(game.SpriteBatch);

            game.SpriteBatch.End();
        }
    }
}
