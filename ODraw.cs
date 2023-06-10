using Microsoft.Xna.Framework;

namespace onwardslib
{
    public static class ODraw
    {
        static Camera _currentCamera;
        static Rectangle _rect = new Rectangle();

        public static void Begin(Camera camera)
        {
            _currentCamera = camera;
            Onwards.SpriteBatch.Begin(transformMatrix: _currentCamera.Matrix);
        }

        public static void End()
        {
            _currentCamera = null;
            Onwards.SpriteBatch.End();
        }

        public static void Draw(Sprite sprite, Vector2 position)
        {
            //TODO PixelToUnit ratio

            _rect.X = (int) position.X;
            _rect.Y = (int) position.Y;
            _rect.Width = (int)sprite.SourceRectangle.Width;
            _rect.Height = (int)sprite.SourceRectangle.Height;
            Onwards.SpriteBatch.Draw(sprite.Texture.Texture2D, _rect, Color.White); //TODO opacity
        }
    }
}
