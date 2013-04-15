using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonTheRemakez.BattleSystem.Screens.Elements
{
    public class TextPanel : DrawableBattleComponent {
        public BattleText BattleText;
        public TextPromptArrow TextPromptArrow;

        public TextPanel(Game game, int x, int y, bool transparent) : base(game)
        {
            Texture = transparent ? game.Content.Load<Texture2D>("Resources/Battlesystem/textPanel") : game.Content.Load<Texture2D>("Resources/Battlesystem/basicControlpanelBackground");
            BattleText = new BattleText(game, x, y);
            TextPromptArrow = new TextPromptArrow(game, x, y) {Visible = false};

            Components.Add(BattleText);
            Components.Add(TextPromptArrow);

            Position = new Vector2(x, y);
        }

        /// <summary>
        /// Update logic for drawing elements
        /// </summary>
        /// <param name="gameTime">instance of gametime</param>
        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            foreach (var drawableBattleComponent in Components.Where(drawableBattleComponent => drawableBattleComponent.Enabled)) {
                drawableBattleComponent.Update(gameTime);
            }
        }

        /// <summary>
        /// Draws the elements to the screen
        /// </summary>
        /// <param name="spriteBatch">instance of spritebatch</param>
        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);

            foreach (var drawableBattleComponent in Components.Where(drawableBattleComponent => drawableBattleComponent.Visible)) {
                drawableBattleComponent.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Draws the elements to the screen
        /// </summary>
        /// <param name="spriteBatch">instance of spritebatch</param>
        public override void Draw(SpriteBatch spriteBatch, Vector2 offset) {
            base.Draw(spriteBatch, offset);

            foreach (var drawableBattleComponent in Components.Where(drawableBattleComponent => drawableBattleComponent.Visible)) {
                drawableBattleComponent.Draw(spriteBatch, offset);
            }    
        }
    }
}
