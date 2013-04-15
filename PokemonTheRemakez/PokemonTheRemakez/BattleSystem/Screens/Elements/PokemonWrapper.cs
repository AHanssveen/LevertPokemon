using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.BattleSystem.Pokemon;

namespace PokemonTheRemakez.BattleSystem.Screens.Elements
{
    public class PokemonWrapper : DrawableBattleComponent
    {
        public PokemonInstance PokemonInstance;

        public PokemonWrapper(Game game, PokemonInstance pokemonInstance) : base(game)
        {
            PokemonInstance = pokemonInstance;
        }

        public PokemonWrapper(Game game): base(game) {}

        public override void Update(GameTime gameTime)
        {
            PokemonInstance.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            PokemonInstance.Draw(spriteBatch);
        }
    }
}
