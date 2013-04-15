using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.World.Maps;
using PokemonTheRemakez.World.Players;
using PokemonTheRemakez.World.Sprites;
using PokemonTheRemakez.World.State;
using PokemonTheRemakez.World.Trainers;

namespace PokemonTheRemakez.World.Screens {
    public class GamePlayScreen : BaseGameState {
        public static Player Player;
        public static Dictionary<string, EnemyTrainer> Trainers;

        public Texture2D BackgroundTexture;
        public Texture2D BottomBorder;

        /// <summary>
        /// The map to be drawn on the screen
        /// </summary>
        private readonly Map _map = new Map();

        public GamePlayScreen(Game1 game, GameStateManager manager)
            : base(game, manager) {
            Player = new Player(game, _map);
            StateManager.OnStateChange += StateManagerOnOnStateChange;
        }

        /// <summary>
        /// What to do when a state changes
        /// </summary>
        /// <param name="sender">instance of an ojbect</param>
        /// <param name="eventArgs">instance of an EventArgs</param>
        private void StateManagerOnOnStateChange(object sender, EventArgs eventArgs)
        {
            if (StateManager.CurrentState != this) {
                Player.Enabled = false;
            }
            else {
                Player.Enabled = true;
                Player.MoveState = MoveState.Still;
            }
        }

        /// <summary>
        /// Loads content used with drawing
        /// </summary>
        protected override void LoadContent() {
            Trainers = new Dictionary<string, EnemyTrainer>();

            // Background
            BackgroundTexture = Game.Content.Load<Texture2D>(@"treeLoop");
            BottomBorder = Game.Content.Load<Texture2D>(@"frroute21");

            // Map setup       
            var s = "frpallet";
            var texture = Game.Content.Load<Texture2D>(s);
            var collisionTexture = Game.Content.Load<Texture2D>("frpallet_collision");
            _map.Add(new MapComponent(s, texture, collisionTexture, new Rectangle(0, 0, texture.Width, texture.Height), "pallet", GameRef));

            s = "frroute1";
            texture = Game.Content.Load<Texture2D>(s);
            collisionTexture = Game.Content.Load<Texture2D>("frroute1_collision");
            _map.Add(new MapComponent(s, texture, collisionTexture, new Rectangle(0, -640, texture.Width, texture.Height), "route1", GameRef));

            s = "pewtercity";
            texture = Game.Content.Load<Texture2D>(s);
            collisionTexture = Game.Content.Load<Texture2D>("pewtercity_collision");
            _map.Add(new MapComponent(s, texture, collisionTexture, new Rectangle(-160, -1280, texture.Width, texture.Height), "pewter", GameRef));
           
            // Load trainers
            Trainers.Add("Sabrina", new EnemyTrainer(GameRef, "Sabrina", "Stay a while, and listen!", "", 208, -80, AnimationKey.Left));
            Trainers.Add("Giovanni", new EnemyTrainer(GameRef, "Giovanni", "It's time to kick ass and chew", "bubble gum, and I'm all out of gum.", 208, -720, AnimationKey.Left));
            Trainers.Add("Lehmann", new EnemyTrainer(GameRef, "Lehmann", "Hei pus! Dette var en god ide.", "", 65, -1018, AnimationKey.Down));
            
            _map.Trainers = Trainers;

            base.LoadContent();
        }

        /// <summary>
        /// Draws your textures to the screen
        /// </summary>
        /// <param name="gameTime">provides a snapshot of the gametime</param>
        public override void Draw(GameTime gameTime) {
            GameRef.SpriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null,
                null,
                Player.Camera.Transformation);

            // Draw looping background
            for (int i = 0; i < 40; i++) {
                for (int j = 0; j < 70; j++) {
                    GameRef.SpriteBatch.Draw(BackgroundTexture, new Vector2(i * BackgroundTexture.Width - (BackgroundTexture.Width * 18), j * BackgroundTexture.Height + 7 - (BackgroundTexture.Width * 50)), Color.White);
                }
            }

            GameRef.SpriteBatch.Draw(BottomBorder, new Vector2(0, 320), Color.White);

            // Draw map
            foreach (var mapComponent in _map) {
                // Draw mapcomponent
                GameRef.SpriteBatch.Draw(mapComponent.Texture, mapComponent.Destination, Color.White);
                
                // DEBUG
                // Draw collision texture
                //GameRef.SpriteBatch.Draw(mapComponent.CollisionTexture, mapComponent.Destination, new Color(255,255,255,100));
            }

            Player.Draw(gameTime, GameRef.SpriteBatch);

            // Draw enemy trainers
            foreach (var enemyTrainer in Trainers)
            {
                enemyTrainer.Value.Draw(GameRef.SpriteBatch);
            }     

            base.Draw(gameTime);

            GameRef.SpriteBatch.End();
        }
    }
}
