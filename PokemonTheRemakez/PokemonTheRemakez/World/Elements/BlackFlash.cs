using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokemonTheRemakez.BattleSystem;
using PokemonTheRemakez.World.Screens;

namespace PokemonTheRemakez.World.Elements
{
    public enum BlackFlashState {
        Active,
        Inactive,
        Finished
    }

    public class BlackFlash : DrawableBattleComponent
    {
        public BlackFlashState AnimationState = BlackFlashState.Inactive;

        public int FrameCounter;
        public int FlickerInterval = 10;
        public int FlickerDuration = 8;
        public int FlickerCounter;

        public BlackFlash(Game game)
            : base(game)
        {
            Position = Vector2.Zero;
        }

        /// <summary>
        /// Updates the logic used to draw
        /// </summary>
        /// <param name="gameTime">snapshot of gametime</param>
        public override void Update(GameTime gameTime) {
            if (AnimationState == BlackFlashState.Active) {
                Position = GamePlayScreen.Player.Sprite.Position;
                if (FrameCounter == 0)
                    Visible = !Visible;
                FrameCounter++;
                if (FrameCounter == FlickerInterval) {
                    FrameCounter = 0;
                    FlickerCounter++;
                }
                if (FlickerCounter == FlickerDuration) {
                    FlickerCounter = 0;
                    AnimationState = BlackFlashState.Finished;
                    Visible = false;
                }
            }
        }

        /// <summary>
        /// Draws stuff the screen
        /// </summary>
        /// <param name="spriteBatch">instance of spritebatch</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            var colBox = new Texture2D(Game.GraphicsDevice, 1, 1);
            colBox.SetData(new[] { Color.Black });
            spriteBatch.Draw(colBox, new Rectangle(-2000,-2000,10000,10000), Color.White);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset) {
            Draw(spriteBatch);
        }
    }
}