using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PokemonTheRemakez.World.Input;
// Handles the choice arrow.
namespace PokemonTheRemakez.BattleSystem.Screens.Elements
{
    [Flags]
    public enum AttackChoiceArrowPosition
    {
        Move1 = 0,
        Move2 = 1,
        Move3 = 2,
        Move4 = 3
    }

    public class AttackChoiceArrow : DrawableBattleComponent
    {
        public AttackChoiceArrowPosition AttackChoiceArrowPosition = AttackChoiceArrowPosition.Move1;

        public AttackChoiceArrow(Game game)
            : base(game)
        {
            SetPosition(AttackChoiceArrowPosition.Move1);
            Texture = game.Content.Load<Texture2D>("Resources/Battlesystem/choiceArrow");
        }

        public int MoveCount;
        /// <summary>
        /// Sets position of the arrow
        /// </summary>
        /// <param name="attackChoiceArrowPosition"></param>
        public void SetPosition(AttackChoiceArrowPosition attackChoiceArrowPosition)
        {
            AttackChoiceArrowPosition = attackChoiceArrowPosition;

            switch (attackChoiceArrowPosition)
            {
                case AttackChoiceArrowPosition.Move1:
                    Position = new Vector2(10, 120);
                    break;
                case AttackChoiceArrowPosition.Move2:
                    Position = new Vector2(80, 120);                    
                    break;
                case AttackChoiceArrowPosition.Move3:
                    Position = new Vector2(10, 135);
                    break;
                case AttackChoiceArrowPosition.Move4:
                    Position = new Vector2(80, 135);
                    break;
            }
        } 
        /// <summary>
        /// Arrow logic.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputHandler.ActionKeyPressed(ActionKey.Left, PlayerIndex.One))
            {
                switch (AttackChoiceArrowPosition)
                {
                    case AttackChoiceArrowPosition.Move2:
                        SetPosition(AttackChoiceArrowPosition.Move1);
                        break;
                    case AttackChoiceArrowPosition.Move4:
                        if (MoveCount >= 3)
                            SetPosition(AttackChoiceArrowPosition.Move3);
                        break;
                }
            } 
            else if (InputHandler.ActionKeyPressed(ActionKey.Right, PlayerIndex.One))
            {
                switch (AttackChoiceArrowPosition)
                {
                    case AttackChoiceArrowPosition.Move1:
                        if (MoveCount >= 2)
                            SetPosition(AttackChoiceArrowPosition.Move2);
                        break;
                    case AttackChoiceArrowPosition.Move3:
                        if (MoveCount == 4)
                            SetPosition(AttackChoiceArrowPosition.Move4);
                        break;
                }
            } else if (InputHandler.ActionKeyPressed(ActionKey.Up, PlayerIndex.One))
            {
                switch (AttackChoiceArrowPosition)
                {
                    case AttackChoiceArrowPosition.Move3:
                        SetPosition(AttackChoiceArrowPosition.Move1);
                        break;
                    case AttackChoiceArrowPosition.Move4:
                        if (MoveCount >= 2)
                            SetPosition(AttackChoiceArrowPosition.Move2);
                        break;
                }
            } 
            else if (InputHandler.ActionKeyPressed(ActionKey.Down, PlayerIndex.One))
            {
                switch (AttackChoiceArrowPosition)
                {
                    case AttackChoiceArrowPosition.Move1:
                        if (MoveCount >= 3)
                            SetPosition(AttackChoiceArrowPosition.Move3);
                        break;
                    case AttackChoiceArrowPosition.Move2:
                        if (MoveCount == 4)
                            SetPosition(AttackChoiceArrowPosition.Move4);
                        break;
                }
            }
        }
    }
}
