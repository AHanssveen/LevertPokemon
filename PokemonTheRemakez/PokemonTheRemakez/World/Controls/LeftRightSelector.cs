using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PokemonTheRemakez.World.Input;

namespace PokemonTheRemakez.World.Controls
{
    public class LeftRightSelector : Control
    {
        public event EventHandler SelectionChanged;

        private readonly List<string> _items = new List<string>();

        private Texture2D _leftTexture;
        private Texture2D _rightTexture;
        private Texture2D _stopTexture;

        private Color _selectedColor = Color.Red;
        private int _maxItemWidth;
        private int _selectedItem;

        public LeftRightSelector(Texture2D leftArrow, Texture2D rightArrow, Texture2D stop)
        {
            _leftTexture = leftArrow;
            _rightTexture = rightArrow;
            _stopTexture = stop;
            TabStop = true;
            Color = Color.White;
        }

        public Color SelectedColor
        {
            get { return _selectedColor; }
            set { _selectedColor = value; }
        }

        public int SelectedIndex
        {
            get { return _selectedItem; }
            set { _selectedItem = (int)MathHelper.Clamp(value, 0f, _items.Count); }
        }

        public string SelectedItem
        {
            get { return Items[_selectedItem]; }
        }

        public List<string> Items
        {
            get { return _items; }
        } 

        public void SetItems(string[] items, int maxWidth)
        {
            _items.Clear();

            foreach (var s in items)
                _items.Add(s);

            _maxItemWidth = maxWidth;
        }

        protected void OnSelectionChanged()
        {
            if (SelectionChanged != null)
                SelectionChanged(this, null);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var drawTo = _position;

            if (_selectedItem != 0)
                spriteBatch.Draw(_leftTexture, drawTo, Color.White);
            else
                spriteBatch.Draw(_stopTexture, drawTo, Color.White);

            drawTo.X += _leftTexture.Width + 5f;

            float itemWidth = _spriteFont.MeasureString(Items[_selectedItem]).X;
            float offset = (_maxItemWidth - itemWidth)/2;

            drawTo.X += offset;

            spriteBatch.DrawString(_spriteFont, _items[_selectedItem], drawTo, _hasFocus ? SelectedColor : Color);

            drawTo.X += -1*offset + _maxItemWidth + 5f;

            spriteBatch.Draw(_selectedItem != _items.Count - 1 ? _rightTexture : _stopTexture, drawTo, Color.White);
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
            if (_items.Count == 0)
                return;

            if (InputHandler.ButtonReleased(Buttons.LeftThumbstickLeft, playerIndex) ||
                InputHandler.ButtonReleased(Buttons.DPadLeft, playerIndex) ||
                InputHandler.KeyReleased(Keys.Left))
            {
                _selectedItem--;
                if (_selectedItem < 0)
                    _selectedItem = 0;
                OnSelectionChanged();
            }

            if (InputHandler.ButtonReleased(Buttons.LeftThumbstickRight, playerIndex) ||
                InputHandler.ButtonReleased(Buttons.DPadRight, playerIndex) ||
                InputHandler.KeyReleased(Keys.Right))
            {
                _selectedItem++;
                if (_selectedItem >= _items.Count)
                    _selectedItem = _items.Count - 1;
                OnSelectionChanged();
            }    
        }
    }
}
