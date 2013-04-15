using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RemakezBattleSystem.Resources.Screens.Elements
{
    public class ExclamationBubble : DrawableBattleComponent
    {
        public ExclamationBubble(Game game) : base(game)
        {
            Position = new Vector2(0, 0);
            Texture = game.Content.Load<Texture2D>("Resources/Trainers/ChatBubble");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
