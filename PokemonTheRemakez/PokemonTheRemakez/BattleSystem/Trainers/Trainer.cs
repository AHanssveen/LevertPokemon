using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.BattleSystem.Pokemon;

namespace PokemonTheRemakez.BattleSystem.Trainers
{
    public enum TrainerPosition { Starting, CombatStart, PokemonDeployment, Withdrawing }

    public enum RenderingPosition { Ally, Enemy }

    public class Trainer : DrawableBattleComponent
    {
        public String Name;
        public PokemonInventorySet PokemonSet = new PokemonInventorySet();
        public int Money;

        // BattleScreen positioning
        public RenderingPosition RenderingPosition = RenderingPosition.Ally;
        public Vector2 AllyPosition = new Vector2(250, 60);
        public Vector2 EnemyStartPosition = new Vector2(-50, 0);
        public TrainerPosition TrainerPosition = TrainerPosition.Starting;

        // BattleScreen rendering
        private readonly Texture2D _frontTexture;
        private readonly Texture2D _backTexture;

        public Trainer(Game game, String name, String textureName) : base(game)
        {
            Name = name;
            var texturePath = "Resources/Trainers/" + textureName;
            _frontTexture = game.Content.Load<Texture2D>(texturePath);
            _backTexture = game.Content.Load<Texture2D>("Resources/Trainers/TrainerHero");
        }

        /// <summary>
        /// Sets up positions in preparation for combat
        /// </summary>
        /// <param name="renderingPosition"></param>
        public void PrepareForCombat(RenderingPosition renderingPosition)
        {
            Visible = true;
            RenderingPosition = renderingPosition;
            switch (renderingPosition)
            {
                case RenderingPosition.Ally:
                    Position = AllyPosition;
                    break;
                case RenderingPosition.Enemy:
                    Position = EnemyStartPosition;
                    break;
            }
        }

        /// <summary>
        /// draws what needs tp be drawn
        /// </summary>
        /// <param name="spriteBatch">instance of spritebatch</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (RenderingPosition)
            {
                case RenderingPosition.Ally:
                    DrawAsAlly(spriteBatch);
                    break;
                case RenderingPosition.Enemy:
                    DrawAsEnemy(spriteBatch);
                    break;
            }
        }

        /// <summary>
        /// Draws trainer as an enemy
        /// </summary>
        /// <param name="spriteBatch">instance of spritebatch</param>
        public void DrawAsEnemy(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_frontTexture, Position, Color.White);
        }

        /// <summary>
        /// draws trainer as an ally
        /// </summary>
        /// <param name="spriteBatch">instance of spritebatch</param>
        public void DrawAsAlly(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_backTexture, Position, Color.White);
        }
    }

    
}
