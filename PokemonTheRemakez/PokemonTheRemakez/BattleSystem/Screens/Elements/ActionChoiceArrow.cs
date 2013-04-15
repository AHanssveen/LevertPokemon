using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.World.Input;

namespace PokemonTheRemakez.BattleSystem.Screens.Elements
{
    public enum ActionChoiceArrowPosition
    {
        Fight,
        Pokemon,
        Bag,
        Run
    }
    // The pointer logic for choosing actions
    public class ActionChoiceArrow : DrawableBattleComponent
    {
        public ActionChoiceArrowPosition ActionChoiceArrowPosition = ActionChoiceArrowPosition.Fight;

        public ActionChoiceArrow(Game game) : base(game)
        {
            Position = new Vector2(125, 120);
            Texture = game.Content.Load<Texture2D>("Resources/Battlesystem/choiceArrow");
        }
        /// <summary>
        /// Sets position of pointer
        /// </summary>
        /// <param name="actionChoiceArrowPosition">Which position to use</param>
        public void SetPosition(ActionChoiceArrowPosition actionChoiceArrowPosition)
        {
            ActionChoiceArrowPosition = actionChoiceArrowPosition;

            switch (actionChoiceArrowPosition)
            {
                case ActionChoiceArrowPosition.Fight:
                    Position = new Vector2(125, 120);
                    break;
                case ActionChoiceArrowPosition.Pokemon:
                    Position = new Vector2(125, 135);
                    break;
                case ActionChoiceArrowPosition.Bag:
                    Position = new Vector2(180, 120);
                    break;
                case ActionChoiceArrowPosition.Run:
                    Position = new Vector2(180, 135);
                    break;
            }
        }
        /// <summary>
        /// Pointer logic
        /// </summary>
        /// <param name="gameTime">gets gametime</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputHandler.ActionKeyPressed(ActionKey.Left, PlayerIndex.One))
            {
                switch (ActionChoiceArrowPosition)
                {
                    case ActionChoiceArrowPosition.Bag:
                        SetPosition(ActionChoiceArrowPosition.Fight);
                        break;
                    case ActionChoiceArrowPosition.Run:
                        SetPosition(ActionChoiceArrowPosition.Pokemon);
                        break;
                }
            }
            else if (InputHandler.ActionKeyPressed(ActionKey.Right, PlayerIndex.One))
            {
                switch (ActionChoiceArrowPosition)
                {
                    case ActionChoiceArrowPosition.Fight:
                        SetPosition(ActionChoiceArrowPosition.Bag);
                        break;
                    case ActionChoiceArrowPosition.Pokemon:
                        SetPosition(ActionChoiceArrowPosition.Run);
                        break;
                }    
            } 
            else if (InputHandler.ActionKeyPressed(ActionKey.Up, PlayerIndex.One))
            {
                switch (ActionChoiceArrowPosition)
                {
                    case ActionChoiceArrowPosition.Pokemon:
                        SetPosition(ActionChoiceArrowPosition.Fight);
                        break;
                    case ActionChoiceArrowPosition.Run:
                        SetPosition(ActionChoiceArrowPosition.Bag);
                        break;
                }    
            } 
            else if (InputHandler.ActionKeyPressed(ActionKey.Down, PlayerIndex.One))
            {
                switch (ActionChoiceArrowPosition)
                {
                    case ActionChoiceArrowPosition.Fight:
                        SetPosition(ActionChoiceArrowPosition.Pokemon);
                        break;
                    case ActionChoiceArrowPosition.Bag:
                        SetPosition(ActionChoiceArrowPosition.Run);
                        break;
                }    
            }
        }
    }
}
