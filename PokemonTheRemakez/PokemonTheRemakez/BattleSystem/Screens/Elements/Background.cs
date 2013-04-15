using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonTheRemakez.BattleSystem.Screens.Elements
{
    public enum BackgroundType { Grass }

    public class Background : DrawableBattleComponent
    {
        public Background(Game game, BackgroundType backgroundType) : base(game)
        {
            string textureName = "";

            switch (backgroundType)
            {
                case BackgroundType.Grass:
                default:
                    textureName = "grass";
                    break;
            }
            var texturePath = "Resources/Battlesystem/background" + textureName;
            LoadContent(texturePath);
        }
    }
}
