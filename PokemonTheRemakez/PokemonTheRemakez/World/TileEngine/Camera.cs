using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PokemonTheRemakez.World.Input;
using PokemonTheRemakez.World.Sprites;
// camera logic
namespace PokemonTheRemakez.World.TileEngine {
    public enum CameraMode { Free, Follow }

    public class Camera {
        private Vector2 _position;
        private float _speed;
        public float Zoom;
        private Rectangle _viewportRectangle;
        private CameraMode _mode;

        private const float DefaultSpeed = 4f;
        private const float DefaultZoom = 2f;   // 1f

        public Vector2 Position {
            get { return _position; }
            set { _position = value; }
        }

        public float Speed {
            get { return _speed; }
            set { _speed = MathHelper.Clamp(value, 1f, 16f); }
        }

        public CameraMode Mode {
            get { return _mode; }
        }

        public Matrix Transformation {
            get { return Matrix.CreateScale(Zoom) * Matrix.CreateTranslation(new Vector3(-Position, 0f)); }
        }
        
        public void Update (GameTime gametime){}

        public Rectangle ViewportRectangle {
            get {
                return new Rectangle(
                    _viewportRectangle.X,
                    _viewportRectangle.Y,
                    _viewportRectangle.Width,
                    _viewportRectangle.Height);
            }
        }


        public Camera(Rectangle viewportRectangle) {
            _speed = DefaultSpeed;
            Zoom = DefaultZoom;
            _viewportRectangle = viewportRectangle;
            _mode = CameraMode.Follow;
        }

        public Camera(Rectangle viewportRectangle, Vector2 position) {
            _speed = DefaultSpeed;
            Zoom = DefaultZoom;
            _position = position;
            _viewportRectangle = viewportRectangle;
            _mode = CameraMode.Follow;
        }

        public void ZoomIn() {
            Zoom += .25f;

            if (Zoom > 10f)
                Zoom = 10f;

            Vector2 newPosition = Position * Zoom;
            SnapToPosition(newPosition);
        }

        public void ZoomOut() {
            Zoom -= .25f;

            if (Zoom < 0.25f)
                Zoom = 0.25f;

            Vector2 newPosition = Position * Zoom;
            SnapToPosition(newPosition);
        }
        /// <summary>
        /// Jumps to position
        /// </summary>
        /// <param name="newPosition">takes which position to use</param>
        private void SnapToPosition(Vector2 newPosition) {
            _position.X = newPosition.X - (float)_viewportRectangle.Width / 2;
            _position.Y = newPosition.Y - (float)_viewportRectangle.Height / 2;
        }

        
        /// <summary>
        /// Locks camera to the spite
        /// </summary>
        /// <param name="sprite">which sprite to lock on to</param>
        public void LockToSprite(AnimatedSprite sprite) {
            _position.X = (sprite.Position.X + (float)sprite.Width / 2) * Zoom - ((float)ViewportRectangle.Width / 2);

            _position.Y = (sprite.Position.Y + (float)sprite.Height / 2) * Zoom - ((float)ViewportRectangle.Height / 2);

        }
        /// <summary>
        /// Toogles camera mode
        /// </summary>
        public void ToggleCameraMode() {
            if (_mode == CameraMode.Follow)
                _mode = CameraMode.Free;
            else if (_mode == CameraMode.Free)
                _mode = CameraMode.Follow;
        }
        /// <summary>
        /// locks camera to center of screen
        /// </summary>
        /// <param name="screenRectangle">which rectangle to use</param>
        public void LockToCenter(Rectangle screenRectangle) {
            _position.X = 0;

            _position.Y = 0;
        }
    }
}
