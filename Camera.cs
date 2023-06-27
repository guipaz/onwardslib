using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

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
        public int SpriteScale
        {
            get => _spriteScale;
            set
            {
                _spriteScale = value;
                _dirty = true;
            }
        }
        public RenderTarget2D RenderTarget { get; }
        public Matrix Matrix
        {
            get
            {
                if (_dirty)
                {
                    _dirty = false;

                    double tX = _position.X * _zoom;
                    double tY = _position.Y *_zoom;

                    _matrix = Matrix.CreateTranslation(-new Vector3(_position.X, _position.Y, 0)) *
                              Matrix.CreateScale(_zoom) *
                              Matrix.CreateTranslation(new Vector3((float)(tX - Math.Truncate(tX)),
                               (float)(tY - Math.Truncate(tY)), 0));
                }

                return _matrix;
            }
        }

        IEnumerator _currentZoomFunc;

        Matrix _matrix;
        bool _dirty = true;
        Vector2 _position;
        Vector2 _lastTargetPosition;
        float _zoom = 1;
        int _spriteScale = 1;

        public Camera(int width, int height)
        {
            Width = width;
            Height = height;
            RenderTarget = new RenderTarget2D(Engine.GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
        }

        public void CenterOn(Vector2 position, bool immediate = false, float speed = .1f)
        {
            const float snapTolerance = 1f;

            _lastTargetPosition = position;

            var finalPos = position * _spriteScale - Engine.ViewportResolution.ToVector2() / 2 / _zoom;
            if (immediate)
            {
                Position = finalPos;
            }
            else
            {
                var diff = Position - finalPos;
                var biggerDiff = Math.Max(Math.Abs(diff.X), Math.Abs(diff.Y));

                Position = Vector2.Lerp(Position, finalPos, speed);

                if (biggerDiff <= snapTolerance)
                {
                    Position = finalPos;
                }
            }
        }

        public void SetZoom(float toZoom)
        {
            _currentZoomFunc = ZoomFunc(toZoom);
        }

        IEnumerator ZoomFunc(float toZoom)
        {
            float time = .5f;
            float startZoom = Zoom;
            float diff = toZoom - startZoom;

            float t = 0;
            while (t < 1)
            {
                Zoom = startZoom + diff * Easing.SineEaseInOut(t);
                t += Engine.DeltaTime / time;
                CenterOn(_lastTargetPosition, false, .5f);
                yield return 0;
            }
            Zoom = toZoom;

            _currentZoomFunc = null;
        }

        public void UpdateFuncs()
        {
            _currentZoomFunc?.MoveNext();
        }
    }
}
