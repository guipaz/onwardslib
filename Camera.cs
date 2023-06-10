using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace onwardslib
{
    public class Camera
    {
        public int Width { get; }
        public int Height { get; }
        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                _dirty = true;
            }
        }
        public float Zoom
        {
            get => _zoom;
            set
            {
                _zoom = value;
                _dirty = true;
            }
        }
        public int PixelsPerUnit { get; set; } = 16;
        public RenderTarget2D RenderTarget { get; }
        public Matrix Matrix
        {
            get
            {
                if (_dirty)
                {
                    _dirty = false;

                    double tX = _position.X * Zoom;
                    double tY = _position.Y * Zoom;

                    _matrix = Matrix.CreateTranslation(-new Vector3(_position.X, _position.Y, 0)) * Matrix.CreateScale(_zoom) *
                           Matrix.CreateTranslation(new Vector3((float)(tX - Math.Truncate(tX)),
                               (float)(tY - Math.Truncate(tY)), 0));
                }

                return _matrix;
            }
        }

        Matrix _matrix;
        bool _dirty;
        Vector2 _position;
        float _zoom;

        public Camera(int width, int height)
        {
            RenderTarget = new RenderTarget2D(Onwards.GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
        }
    }
}
