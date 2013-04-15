using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PokemonTheRemakez.World.Input;

namespace PokemonTheRemakez.World.Controls
{
    public class ControlManager : List<Control>
    {
        public event EventHandler FocusChanged;

        private int _selectedControl = 0;

        private static SpriteFont _spriteFont;

        public ControlManager(SpriteFont spriteFont)
            : base()
        {
            _spriteFont = spriteFont;
        }

        public static SpriteFont SpriteFont
        {
            get { return _spriteFont; }
        }

        public void Update(GameTime gameTime, PlayerIndex playerIndex)
        {
            if (Count == 0)
                return;
            foreach (Control c in this)
            {
                if (c.Enabled)
                    c.Update(gameTime);
                if (c.HasFocus)
                    c.HandleInput(playerIndex);
            }
            if (InputHandler.ButtonPressed(Buttons.LeftThumbstickUp, playerIndex) ||
            InputHandler.ButtonPressed(Buttons.DPadUp, playerIndex) ||
            InputHandler.KeyPressed(Keys.Up))
                PreviousControl(); if (InputHandler.ButtonPressed(Buttons.LeftThumbstickDown, playerIndex) ||
                InputHandler.ButtonPressed(Buttons.DPadDown, playerIndex) ||
                InputHandler.KeyPressed(Keys.Down))
                NextControl();    
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Control c in this.Where(c => c.Visible))
                c.Draw(spriteBatch);
        }

        public void NextControl()
        {
            if (Count == 0)
                return;

            int currentControl = _selectedControl;
            this[_selectedControl].HasFocus = false;

            do
            {
                _selectedControl++;

                if (_selectedControl == Count)
                    _selectedControl = 0;

                if (this[_selectedControl].TabStop && this[_selectedControl].Enabled)
                {
                    if (FocusChanged != null)
                        FocusChanged(this[_selectedControl], null);

                    break;    
                }
                    
            } while (currentControl != _selectedControl);

            this[_selectedControl].HasFocus = true;
        }

        public void PreviousControl()
        {
            if (Count == 0)
                return;

            int currentControl = _selectedControl;

            this[_selectedControl].HasFocus = false;

            do
            {
                _selectedControl--;

                if (_selectedControl < 0)
                    _selectedControl = Count - 1;

                if (this[_selectedControl].TabStop && this[_selectedControl].Enabled)
                {
                    if (FocusChanged != null)
                        FocusChanged(this[_selectedControl], null);

                    break;
                }

            } while (currentControl != _selectedControl);

            this[_selectedControl].HasFocus = true;
        }
    }
}
