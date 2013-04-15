using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonTheRemakez.BattleSystem.Screens.Elements
{
    public class ActionMenu : DrawableBattleComponent
    {
        // defines where to load the menu of actions
        public ActionMenu(Game game) : base(game)
        {
            Position = new Vector2(120, 110); 
            Texture = game.Content.Load<Texture2D>("Resources/Battlesystem/actionMenu");
        }
    }
}
