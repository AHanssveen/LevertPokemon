using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace PokemonTheRemakez.World.Input {
    public class Action {
        public List<Keys> Keys = new List<Keys>();
        public List<Buttons> Buttons = new List<Buttons>();
        public bool ThumbStickDirectionMapped;
        public ThumbStickDirection ThumbStickDirection;
    }
}
