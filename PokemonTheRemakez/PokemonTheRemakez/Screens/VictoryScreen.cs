using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.BattleSystem;
using PokemonTheRemakez.BattleSystem.Screens;
using PokemonTheRemakez.Intro.Screens;
using PokemonTheRemakez.World.Input;
using PokemonTheRemakez.World.Screens;
using PokemonTheRemakez.World.State;

namespace PokemonTheRemakez.Screens
{
    public class VictoryScreen : BaseGameState
    {
        private int _timer;
        private Texture2D _texture;

        public VictoryScreen(Game1 game, GameStateManager manager) : base(game, manager)
        {          
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _texture = GameRef.Content.Load<Texture2D>("Victory");
        }

        public override void Update(GameTime gameTime)
        {
            _timer++;

            base.Update(gameTime);

            if (_timer > 120)
            {
                if (InputHandler.ActionKeyPressed(ActionKey.ConfirmAndInteract, PlayerIndex.One))
                {
                    // Reset everything in the game
                    GameRef.GamePlayScreen = new GamePlayScreen(GameRef, StateManager);
                    GameRef.BattleScreen = new BattleScreen(GameRef, StateManager);
                    var datamanager = new DataManager(GameRef);
                    GamePlayScreen.Player.PlayerTrainer = DataManager.Trainers["Trond"];
                    StateManager.ChangeState(GameRef.GamePlayScreen);
                    GameRef.IntroScreen.InitializeIntro();
                    _timer = 0;
                }
                else if (InputHandler.ActionKeyPressed(ActionKey.Exit, PlayerIndex.One))
                    Game.Exit();
            }           
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GameRef.SpriteBatch.Begin();

            GameRef.SpriteBatch.Draw(_texture, Vector2.Zero, Color.White);

            GameRef.SpriteBatch.End();
        }
    }
}
