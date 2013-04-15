using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonTheRemakez.BattleSystem
{
    public abstract class DrawableBattleComponent : DrawableGameComponent
    {
        protected Texture2D Texture;
        private Vector2 _position = Vector2.Zero;
        public Vector2 Offset = Vector2.Zero;

        public List<DrawableBattleComponent> Components = new List<DrawableBattleComponent>();

        protected DrawableBattleComponent(Game game) : base(game)
        {
            Visible = true;
        }

        public Vector2 Position {
            get { return _position; }
            set {
                _position = value + Offset;
                foreach (var drawableBattleComponent in Components) {
                    drawableBattleComponent.Position = value;
                }
            }
        }

        protected void LoadContent(string texturePath)
        {
            Texture = Game.Content.Load<Texture2D>(texturePath);
        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Texture, _position, Color.White);            
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 offset) {
            spriteBatch.Draw(Texture, _position + offset, Color.White);            
        }
    }
}
