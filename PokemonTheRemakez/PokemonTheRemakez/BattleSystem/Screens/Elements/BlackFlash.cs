using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RemakezBattleSystem.Resources.Screens.Elements
{
    public class BlackFlash : DrawableBattleComponent
    {
        public BlackFlash(Game game) : base(game)
        {
            Position = new Vector2(0, 110);
            Texture = game.Content.Load<Texture2D>("Resources/Trainers/BlackFlash");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
