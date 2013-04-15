using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PokemonTheRemakez.World.Input;

namespace PokemonTheRemakez.BattleSystem.Screens.Elements
{
    public enum TextArrowState {
        AwaitingClick,
        Inactive,
        Clicked
    }

    // Draws a textArrow for when you should press a button to move on in dialogue
    public class TextPromptArrow : DrawableBattleComponent
    {
        public TextArrowState State = TextArrowState.AwaitingClick;
        public int Timer;

        public TextPromptArrow(Game game, int x, int y) : base(game)
        {
            Offset = new Vector2(213, 25);
            Position = new Vector2(x, y);
            Texture = game.Content.Load<Texture2D>("Resources/Battlesystem/textArrow");
        }

        /// <summary>
        ///Makes you wait for reading the text and tests if you press enter and want to move on
        /// </summary>
        /// <param name="gameTime">Snapshot of the gametime</param>
        public void WaitingForTextToAppear(GameTime gameTime) {
            State = TextArrowState.AwaitingClick;
            Timer += gameTime.ElapsedGameTime.Milliseconds;
            if (Timer >= 600) {
                Visible = true;
                if (InputHandler.ActionKeyPressed(ActionKey.ConfirmAndInteract, PlayerIndex.One)) {
                    State = TextArrowState.Clicked;
                    Visible = false;
                    Timer = 0;
                }
            } else {
                Visible = false;
            }
        }

        /// <summary>
        /// makes you wait for reading the text and tests if you press enter and want to move on
        /// </summary>
        /// <param name="gameTime">instance of gametime</param>
        /// <param name="timerEnd">how long you want to wait in milliseconds</param>
        public void WaitingForTextToAppear(GameTime gameTime, int timerEnd) {
            State = TextArrowState.AwaitingClick;
            Timer += gameTime.ElapsedGameTime.Milliseconds;
            if (Timer >= timerEnd) {
                Visible = true;
                if (InputHandler.ActionKeyPressed(ActionKey.ConfirmAndInteract, PlayerIndex.One)) {
                    State = TextArrowState.Clicked;
                    Visible = false;
                    Timer = 0;
                }
            } else {
                Visible = false;
            }
        }
    }
}
