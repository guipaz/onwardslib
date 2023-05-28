using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using onwards.components;
using onwards.monobmfont;

namespace onwards.graphics
{
    public static class Draw
    {
        static Camera _currentCamera;
        
        public static void SetCurrentCamera(Camera camera)
        {
            _currentCamera = camera;
            Engine.Instance.Graphics.SetRenderTarget(_currentCamera?.RenderTarget);
        }

        public static float GetScaleRatio(float targetRatio, float textureRatio)
        {
            return targetRatio / textureRatio;
        }

        public static void DrawSprite(Sprite sprite, Vector2 position, Color? color = null, float rotation = 0, SpriteEffects spriteEffects = SpriteEffects.None, float plusScale = 1f)
        {
            var scale = GetScaleRatio(_currentCamera.UnitInPixels, sprite.Texture.PixelsToUnit);

            Engine.Instance.SpriteBatch.Draw(sprite.Texture.Texture2D,
                position * scale,
                sprite.SourceRectangle,
                color ?? Color.White,
                rotation,
                Vector2.Zero,
                scale * plusScale,
                spriteEffects,
                0);
        }

        public static void DrawSprite(Sprite sprite, Rectangle rectangle, Color? color = null, Vector2? origin = null, float rotation = 0, SpriteEffects spriteEffects = SpriteEffects.None)
        {
            var ratio = GetScaleRatio(_currentCamera.UnitInPixels, sprite.Texture.PixelsToUnit);

            rectangle.X = (int)(rectangle.X * ratio);
            rectangle.Y = (int)(rectangle.Y * ratio);
            rectangle.Width = (int)(rectangle.Width * ratio);
            rectangle.Height = (int)(rectangle.Height * ratio);

            Engine.Instance.SpriteBatch.Draw(sprite.Texture.Texture2D,
                rectangle,
                sprite.SourceRectangle,
                color ?? Color.White,
                rotation,
                origin ?? Vector2.Zero,
                spriteEffects,
                0);
        }

        public static void DrawTexture(OTexture texture, Rectangle rectangle, Color? color = null)
        {
            var ratio = GetScaleRatio(_currentCamera.UnitInPixels, texture.PixelsToUnit);

            rectangle.X = (int)(rectangle.X * ratio);
            rectangle.Y = (int)(rectangle.Y * ratio);
            rectangle.Width = (int)(rectangle.Width * ratio);
            rectangle.Height = (int)(rectangle.Height * ratio);

            Engine.Instance.SpriteBatch.Draw(texture.Texture2D,
                rectangle,
                texture.Texture2D.Bounds,
                color ?? Color.White);
        }

        public static void DrawTexture(Texture2D texture, Rectangle rectangle, Color? color = null)
        {
            var ratio = GetScaleRatio(_currentCamera.UnitInPixels, OTexture.DEFAULT_PIXELS_TO_UNIT);

            rectangle.X = (int)(rectangle.X * ratio);
            rectangle.Y = (int)(rectangle.Y * ratio);
            rectangle.Width = (int)(rectangle.Width * ratio);
            rectangle.Height = (int)(rectangle.Height * ratio);

            Engine.Instance.SpriteBatch.Draw(texture,
                rectangle,
                texture.Bounds,
                color ?? Color.White);
        }

        public static void DrawTexture(OTexture texture, Rectangle destinationRectangle, Rectangle sourceRectangleOverride, Color? color = null)
        {
            var ratio = GetScaleRatio(_currentCamera.UnitInPixels, texture.PixelsToUnit);

            destinationRectangle.X = (int)(destinationRectangle.X * ratio);
            destinationRectangle.Y = (int)(destinationRectangle.Y * ratio);
            destinationRectangle.Width = (int)(destinationRectangle.Width * ratio);
            destinationRectangle.Height = (int)(destinationRectangle.Height * ratio);

            Engine.Instance.SpriteBatch.Draw(texture.Texture2D,
                destinationRectangle,
                sourceRectangleOverride,
                color ?? Color.White);
        }

        public static void DrawText(string text, BMFont font, Vector2 position, Color color)
        {
            Engine.Instance.SpriteBatch.DrawString(font, text, position, color);
        }

        public static void DrawGrid(int width, int height, int tileSize, int offsetX = 0, int offsetY = 0)
        {
            for (var y = 0; y <= height; y++)
            {
                DrawTexture(new OTexture(ColorTexture.Get(Color.White * 0.5f)),
                    new Rectangle(-tileSize / 2 + offsetX, y * tileSize - tileSize / 2 + offsetY, width * tileSize, 1));
            }

            for (var x = 0; x <= width; x++)
            {
                DrawTexture(new OTexture(ColorTexture.Get(Color.White * 0.5f)),
                    new Rectangle(x * tileSize - tileSize / 2 + offsetX, -tileSize / 2 + offsetY, 1,
                        height * tileSize));
            }
        }
    }
}