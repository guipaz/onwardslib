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

            Engine.GraphicsDevice.SetRenderTarget(_currentCamera.RenderTarget);
            Engine.GraphicsDevice.Clear(Color.Transparent);
            Engine.SpriteBatch.Begin(transformMatrix: _currentCamera.Matrix);
        }

        public static void End()
        {
            _currentCamera = null;
            Engine.SpriteBatch.End();
            Engine.GraphicsDevice.SetRenderTarget(null);
        }

        public static void Draw(Texture2D texture, Vector2 position)
        {
            Draw(texture, new Rectangle((int)(position.X),
                         (int)(position.Y),
                         texture.Width,
                         texture.Height));
        }

        public static void Draw(Sprite sprite, Vector2 position, Vector2 offset)
        {
            Draw(sprite, (int)(position.X + offset.X),
                         (int)(position.Y + offset.Y),
                         sprite.SourceRectangle.Width, 
                         sprite.SourceRectangle.Height);
        }

        public static void Draw(Sprite sprite, int x, int y, int width, int height)
        {
            Draw(sprite, new Rectangle(x, y, width, height));
        }

        public static void Draw(Sprite sprite, Rectangle rectangle)
        {
            Draw(sprite.Texture, rectangle, sprite.SourceRectangle, sprite.Opacity);
        }

        public static void Draw(Texture2D texture, Rectangle destinationRectangle, float opacity = 1f)
        {
            Draw(texture, destinationRectangle, texture.Bounds, opacity);
        }

        public static void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, float opacity = 1f)
        {
            Engine.SpriteBatch.Draw(texture, GetScaledRectangle(destinationRectangle), sourceRectangle, Color.White * opacity);
        }

        public static void DrawRepeated(RepeatAxis repeatAxis, Texture2D texture, float fromX, float fromY, int to, float opacity = 1f)
        {
            var isX = repeatAxis == RepeatAxis.X;
            var textureSize = isX ? texture.Width : texture.Height;
            var repeatingT = to / (isX ? texture.Width : texture.Height);
            for (var t = 0; t < repeatingT; t++)
            {
                Draw(texture,
                     new Rectangle((int)fromX + (isX ? textureSize * t : 0),
                                   (int)fromY + (isX ? 0 : textureSize * t),
                                   texture.Width, texture.Height),
                     opacity);
            }
        }

        static Rectangle GetScaledRectangle(Rectangle rectangle)
        {
            return new Rectangle(rectangle.X * _currentCamera.SpriteScale,
                                 rectangle.Y * _currentCamera.SpriteScale,
                                 rectangle.Width * _currentCamera.SpriteScale,
                                 rectangle.Height * _currentCamera.SpriteScale);
        }

        public enum RepeatAxis
        {
            X = 0, Y
        }
    }
}
