using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonTheRemakez.BattleSystem.Screens.Elements
{
    public class BattleText : DrawableBattleComponent
    {
        public string FirstLine = "";
        public string SecondLine = "";
        public Vector2 FirstLineVector = new Vector2(11, 118);
        public Vector2 SecondLineVector = new Vector2(11, 133);
        public SpriteFont Font;

        public BattleText(Game game, int x, int y) : base(game)
        {           
            Position = new Vector2(x, y);
            Font = Game.Content.Load<SpriteFont>("Size10");
            FirstLineVector = new Vector2(11, 8);
            SecondLineVector = new Vector2(11, 23);
        }

        /// <summary>
        /// Draws the elements to the screen
        /// </summary>
        /// <param name="spriteBatch">instance of spritebatch</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, FirstLine, Position + FirstLineVector, Color.GhostWhite);
            spriteBatch.DrawString(Font, SecondLine, Position + SecondLineVector, Color.GhostWhite);
        }

        /// <summary>
        /// Draws the elements to the screen
        /// </summary>
        /// <param name="spriteBatch">instance of spritebatch</param>
        public override void Draw(SpriteBatch spriteBatch, Vector2 offset) {
            spriteBatch.DrawString(Font, FirstLine, Position + FirstLineVector + offset, Color.GhostWhite);
            spriteBatch.DrawString(Font, SecondLine, Position + SecondLineVector + offset, Color.GhostWhite);
        }
    }
}
