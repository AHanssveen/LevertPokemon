using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PokemonTheRemakez.World.Input {
    public enum ThumbStickDirection { Up, Down, Left, Right, Center}

    public enum ActionKey {
        ConfirmAndInteract,
        Back,
        Pause,
        Exit,
        Up,
        Down,
        Left,
        Right,
        Sprint
    }

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class InputHandler : GameComponent {
        private static Dictionary<ActionKey,Action> _bindings = new Dictionary<ActionKey, Action>();

        private static KeyboardState _keyboardState;
        private static KeyboardState _lastKeyboardState;

        private static GamePadState[] _gamePadStates;
        private static GamePadState[] _lastGamePadStates;

        // Thumbstick constants
        private const float ThumbStickSensitivity = 0.7f;

        // Thumbstick directions for player one
        public static ThumbStickDirection ThumbStickLeft;
        public static ThumbStickDirection ThumbStickRight;

        public InputHandler(Game game)
            : base(game) {
            _keyboardState = Keyboard.GetState();

            _gamePadStates = new GamePadState[Enum.GetValues(typeof(PlayerIndex)).Length];

            foreach (PlayerIndex index in Enum.GetValues(typeof(PlayerIndex)))
                _gamePadStates[(int)index] = GamePad.GetState(index);

            // Add bindings
            var confirmAndInteract = new Action();
            confirmAndInteract.Buttons.Add(Buttons.A);
            confirmAndInteract.Keys.Add(Keys.Enter);

            _bindings.Add(ActionKey.ConfirmAndInteract, confirmAndInteract);

            var back = new Action();
            back.Buttons.Add(Buttons.B);
            back.Keys.Add(Keys.Back);

            _bindings.Add(ActionKey.Back, back);

            var pause = new Action();
            pause.Buttons.Add(Buttons.Start);
            pause.Keys.Add(Keys.P);

            _bindings.Add(ActionKey.Pause, pause);

            var exit = new Action();
            exit.Buttons.Add(Buttons.Back);
            exit.Keys.Add(Keys.Escape);

            _bindings.Add(ActionKey.Exit, exit);

            var up = new Action();
            up.Buttons.Add(Buttons.DPadUp);
            up.ThumbStickDirectionMapped = true;
            up.ThumbStickDirection = ThumbStickDirection.Up;
            up.Keys.Add(Keys.W);
            up.Keys.Add(Keys.Up);

            _bindings.Add(ActionKey.Up, up);

            var down = new Action();
            down.Buttons.Add(Buttons.DPadDown);
            down.ThumbStickDirectionMapped = true;
            down.ThumbStickDirection = ThumbStickDirection.Down;
            down.Keys.Add(Keys.S);
            down.Keys.Add(Keys.Down);

            _bindings.Add(ActionKey.Down, down);

            var left = new Action();
            left.Buttons.Add(Buttons.DPadLeft);
            left.ThumbStickDirectionMapped = true;
            left.ThumbStickDirection = ThumbStickDirection.Left;
            left.Keys.Add(Keys.A);
            left.Keys.Add(Keys.Left);

            _bindings.Add(ActionKey.Left, left);

            var right = new Action();
            right.Buttons.Add(Buttons.DPadRight);
            right.ThumbStickDirectionMapped = true;
            right.ThumbStickDirection = ThumbStickDirection.Right;
            right.Keys.Add(Keys.D);
            right.Keys.Add(Keys.Right);

            _bindings.Add(ActionKey.Right, right);

            var sprint = new Action();
            sprint.Buttons.Add(Buttons.LeftShoulder);
            sprint.Buttons.Add(Buttons.RightShoulder);
            sprint.Keys.Add(Keys.Space);

            _bindings.Add(ActionKey.Sprint, sprint);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime) {
            _lastKeyboardState = _keyboardState;
            _keyboardState = Keyboard.GetState();

            _lastGamePadStates = (GamePadState[])_gamePadStates.Clone();
            foreach (PlayerIndex index in Enum.GetValues(typeof(PlayerIndex)))
                _gamePadStates[(int)index] = GamePad.GetState(index);


            // Set thumbstick directions
            Vector2 thumbStickLeftVector = _gamePadStates[(int) PlayerIndex.One].ThumbSticks.Left;

            if (thumbStickLeftVector.Y < -ThumbStickSensitivity)
                ThumbStickLeft = ThumbStickDirection.Down;
            else if (thumbStickLeftVector.Y > ThumbStickSensitivity)
                ThumbStickLeft = ThumbStickDirection.Up;
            else if (thumbStickLeftVector.X > ThumbStickSensitivity)
                ThumbStickLeft = ThumbStickDirection.Right;
            else if (thumbStickLeftVector.X < -ThumbStickSensitivity)
                ThumbStickLeft = ThumbStickDirection.Left;
            else
                ThumbStickLeft = ThumbStickDirection.Center;

            // DEBUG
            //Console.Clear();
            //Console.WriteLine(ThumbStickLeft + " " + thumbStickLeftVector.X + ", " + thumbStickLeftVector.Y);

            base.Update(gameTime);
        }

        // General Methods

        public static void Flush() {
            _lastKeyboardState = _keyboardState;
        }

        // Keyboard
        private static bool KeyReleased(Keys key) {
            return _keyboardState.IsKeyUp(key) && _lastKeyboardState.IsKeyDown(key);
        }

        private static bool KeyPressed(Keys key) {
            return _keyboardState.IsKeyDown(key) && _lastKeyboardState.IsKeyUp(key);
        }

        private static bool KeyDown(Keys key) {
            return _keyboardState.IsKeyDown(key);
        }

        // Game Pad
        private static bool ButtonReleased(Buttons button, PlayerIndex index) {
            return _gamePadStates[(int)index].IsButtonUp(button) &&
                   _lastGamePadStates[(int)index].IsButtonDown(button);
        }

        private static bool ButtonPressed(Buttons button, PlayerIndex index)
        {
            //return KeyPressed(Keys.M);
            return _gamePadStates[(int)index].IsButtonDown(button) &&
                _lastGamePadStates[(int)index].IsButtonUp(button);
        }

        private static bool ButtonDown(Buttons button, PlayerIndex index) {
            return _gamePadStates[(int)index].IsButtonDown(button);
        }

        // Actions
        public static bool ActionKeyReleased(ActionKey action, PlayerIndex index) {
            var binding = _bindings[action];

            // Check buttons
            if (binding.Buttons.Any(button => ButtonReleased(button, index)))
                return true;
                
            // Check keys
            if (binding.Keys.Any(key => KeyReleased(key)))
                return true; 

            if (binding.ThumbStickDirectionMapped && binding.ThumbStickDirection == ThumbStickLeft)
                return true;
            
            return false;
        }

        /// <summary>
        /// Tests for key pressed once
        /// </summary>
        /// <param name="action">action used</param>
        /// <param name="index">index of player</param>
        /// <returns>boolean value</returns>
        public static bool ActionKeyPressed(ActionKey action, PlayerIndex index)
        {
            //Console.Clear();
            //Console.WriteLine(_gamePadStates[(int)index].ToString());
            //Console.WriteLine("A: " + _gamePadStates[(int)index].IsButtonDown(Buttons.A));
            //Console.WriteLine("B: " + _gamePadStates[(int)index].IsButtonDown(Buttons.B));
            //Console.WriteLine("LB: " + _gamePadStates[(int)index].IsButtonDown(Buttons.LeftShoulder));
            //Console.WriteLine("RB: " + _gamePadStates[(int)index].IsButtonDown(Buttons.RightShoulder));

            var binding = _bindings[action];

            // Check buttons
            if (binding.Buttons.Any(button => ButtonPressed(button, index)))
                return true;

            // Check keys
            if (binding.Keys.Any(key => KeyPressed(key)))
                return true; 

            if (binding.ThumbStickDirectionMapped && binding.ThumbStickDirection == ThumbStickLeft)
                return true;
            
            return false;
        }

        /// <summary>
        /// Tests for key pressed down constantly
        /// </summary>
        /// <param name="action">action used</param>
        /// <param name="index">index of player</param>
        /// <returns>boolean value</returns>
        public static bool ActionKeyDown(ActionKey action, PlayerIndex index) {
            var binding = _bindings[action];
            
            // Check buttons
            if (binding.Buttons.Any(button => ButtonDown(button, index)))
                return true;

            // Check keys
            if (binding.Keys.Any(key => KeyDown(key)))
                return true;               

            // Check direction
            if (binding.ThumbStickDirectionMapped && binding.ThumbStickDirection == ThumbStickLeft)
                return true;
            
            return false;
        }
    }
}
