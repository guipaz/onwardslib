using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace onwardslib
{
    public static class ODraw
    {
        static Camera _currentCamera;
        
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
            Draw(sprite, (int)position.X,
                         (int)position.Y,
                         sprite.SourceRectangle.Width, 
                         sprite.SourceRectangle.Height);
        }

        public static void Draw(Sprite sprite, int x, int y, int width, int height)
        {
            Draw(sprite.Texture, x, y, width, height, sprite.Opacity);
        }

        public static void Draw(Texture2D texture, int x, int y, int width, int height, float opacity = 1f)
        {
            Onwards.SpriteBatch.Draw(texture, GetScaledRectangle(x, y, width, height), Color.White * opacity);
        }

        static Rectangle GetScaledRectangle(int x, int y, int width, int height)
        {
            return new Rectangle(x * _currentCamera.SpriteScale,
                                 y * _currentCamera.SpriteScale,
                                 width * _currentCamera.SpriteScale,
                                 height * _currentCamera.SpriteScale);
        }
    }
}
