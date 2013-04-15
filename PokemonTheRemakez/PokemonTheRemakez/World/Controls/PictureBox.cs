using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonTheRemakez.World.Controls
{
    public class PictureBox : Control
    {
        private Texture2D _image;
        private Rectangle _sourceRect;
        private Rectangle _destRect;

        public new Vector2 Position
        {
            get { return new Vector2(_destRect.X, _destRect.Y); }
            set 
            { 
                _position = value;
                _destRect.X = (int) _position.X;
                _destRect.Y = (int)_position.Y;
            }
        } 

        public Texture2D Image
        {
            get { return _image; }
            set { _image = value; }
        }

        public Rectangle SourceRect
        {
            get { return _sourceRect; }
            set { _sourceRect = value; }
        }

        public Rectangle DestRect
        {
            get { return _destRect; }
            set { _destRect = value; }
        }

        public PictureBox(Texture2D image, Rectangle destRect)
        {
            _image = image;
            _destRect = destRect;
            _sourceRect = new Rectangle(0, 0, image.Width, image.Height);
            Color = Color.White;
        }

        public PictureBox(Texture2D image, Vector2 position)
        {
            _image = image;
            _destRect = new Rectangle((int)position.X, (int)position.Y, _image.Width, _image.Height);
            _sourceRect = new Rectangle(0, 0, image.Width, image.Height);
            Color = Color.White;
        }

        public PictureBox(Texture2D image, Rectangle destRect, Rectangle sourceRect)
        {
            _destRect = destRect;
            _sourceRect = sourceRect;
            _image = image;
            Color = Color.White;
        }

        // Abstract methods
        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_image, _destRect, _sourceRect, Color);
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
        }

        // Picture Box methods

        public void SetPosition(Vector2 newPosition)
        {
            _destRect = new Rectangle(
                (int)newPosition.X,
                (int)newPosition.Y,
                _sourceRect.Width,
                _sourceRect.Height);
        }
    }
}
