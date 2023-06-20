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
            _rect.X = (int) (position.X * _currentCamera.SpriteScale);
            _rect.Y = (int) (position.Y * _currentCamera.SpriteScale);
            _rect.Width = (int)(sprite.SourceRectangle.Width * _currentCamera.SpriteScale);
            _rect.Height = (int)(sprite.SourceRectangle.Height * _currentCamera.SpriteScale);
            Onwards.SpriteBatch.Draw(sprite.Texture, _rect, Color.White); //TODO opacity
        }
    }
}
