using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PokemonTheRemakez.World.Input;

namespace PokemonTheRemakez.World.Controls
{
    public class LinkLabel : Control
    {
        private Color _selectedColor = Color.Red;

        public LinkLabel()
        {
            _tabStop = true;
            _hasFocus = false;
            _position = Vector2.Zero;
        }

        public Color SelectedColor
        {
            get { return _selectedColor; }
            set { _selectedColor = value; }
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(
                SpriteFont, 
                Text, 
                Position, 
                _hasFocus ? _selectedColor : Color,
                0,
                SpriteFont.MeasureString(Text)/2,
                1f,
                SpriteEffects.None,
                0);
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
            if (!_hasFocus) 
                return;

            if (InputHandler.KeyReleased(Keys.Enter) ||
                InputHandler.ButtonReleased(Buttons.A, playerIndex))
                base.OnSelected(null);            
        }
    }
}
