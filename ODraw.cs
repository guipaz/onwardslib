using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monobmfont;
using onwardslib.ui;

namespace onwardslib
{
    public static class ODraw
    {
        static Camera _currentCamera;
        
        public static void Begin(Camera camera, SamplerState samplerState = null)
        {
            _currentCamera = camera;

            Engine.GraphicsDevice.SetRenderTarget(_currentCamera.RenderTarget);
            Engine.GraphicsDevice.Clear(Color.Transparent);
            Engine.SpriteBatch.Begin(transformMatrix: _currentCamera.Matrix, samplerState: samplerState ?? SamplerState.PointWrap);
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

        public static void Draw(Sprite sprite, Rectangle rectangle, float opacity = 1f, Vector2? origin = null)
        {
            Draw(sprite.Texture, rectangle, sprite.SourceRectangle, opacity, 0, sprite.FlipH, sprite.FlipV, origin == null ? Vector2.Zero : (Vector2)origin);
        }

        public static void Draw(Texture2D texture, float opacity = 1f)
        {
            Draw(texture, texture.Bounds, texture.Bounds, opacity);
        }

        public static void Draw(Texture2D texture, Rectangle destinationRectangle, float opacity = 1f)
        {
            Draw(texture, destinationRectangle, texture.Bounds, opacity);
        }

        public static void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, float opacity = 1f, int rotation = 0, bool flipH = false, bool flipV = false, Vector2? origin = null)
        {
            Engine.SpriteBatch.Draw(texture, GetScaledRectangle(destinationRectangle), sourceRectangle, Color.White * opacity, MathHelper.ToRadians(rotation), origin == null ? Vector2.Zero : (Vector2)origin, (flipH ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (flipV ? SpriteEffects.FlipVertically : SpriteEffects.None), 1);
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

        public static void DrawText(BMFont font, string text, Vector2 position, Color color)
        {
            Engine.SpriteBatch.DrawString(font, text, position, color);
        }

        public static void DrawSlicedImage(SlicedImage image, Rectangle bounds, float opacity = 1f)
        {
            var loc = bounds.Location;
            var spaceX = image.Spacing.X;
            var spaceY = image.Spacing.Y;

            // lines
            var lineToCover = bounds.Width - spaceX * 2;
            var lineCount = lineToCover / spaceX;
            var lineRemaining = lineToCover - lineCount * spaceX;

            // columns
            var columnToCover = bounds.Height - spaceY * 2;
            var columnCount = columnToCover / spaceY;
            var columnRemaining = columnToCover - columnCount * spaceY;

            for (var i = 0; i <= lineCount; i++)
            {
                var w = image.Spacing.X;
                if (i == lineCount)
                {
                    w = lineRemaining;
                }

                Draw(image.Sprite.Texture, new Rectangle(loc + new Point(spaceX * (i + 1), 0), new Point(w, spaceY)), image.NRect, opacity); //N
                Draw(image.Sprite.Texture, new Rectangle(loc + new Point(spaceX * (i + 1), bounds.Size.Y - spaceY), new Point(w, spaceY)), image.SRect, opacity); //S
            }

            for (var i = 0; i <= columnCount; i++)
            {
                var h = image.Spacing.Y;
                if (i == columnCount)
                {
                    h = columnRemaining;
                }

                Draw(image.Sprite.Texture, new Rectangle(loc + new Point(0, spaceY * (i + 1)), new Point(spaceX, h)), image.WRect, opacity); //W
                Draw(image.Sprite.Texture, new Rectangle(loc + new Point(bounds.Size.X - spaceX, spaceY * (i + 1)), new Point(spaceX, h)), image.ERect, opacity); //E
            }

            // middle
            var horizontalCount = (bounds.Width - spaceX * 2) / spaceX;
            var verticalCount = (bounds.Height - spaceY * 2) / spaceY;
            for (var i = 0; i <= horizontalCount; i++)
            {
                for (var j = 0; j <= verticalCount; j++)
                {
                    var w = image.Spacing.X;
                    var h = image.Spacing.Y;
                    if (i == horizontalCount)
                    {
                        w = lineRemaining;
                    }
                    if (j == verticalCount)
                    {
                        h = columnRemaining;
                    }

                    Draw(image.Sprite.Texture, new Rectangle(loc + new Point(spaceX * (i + 1), spaceY * (j + 1)), new Point(w, h)), image.CRect, opacity); //C
                }
            }

            // corners
            Draw(image.Sprite.Texture, new Rectangle(loc, image.Spacing), image.NWRect, opacity); //NW
            Draw(image.Sprite.Texture, new Rectangle(loc + new Point(bounds.Size.X - spaceX, 0), image.Spacing), image.NERect, opacity); //NE
            Draw(image.Sprite.Texture, new Rectangle(loc + new Point(0, bounds.Size.Y - spaceY), image.Spacing), image.SWRect, opacity); //SW
            Draw(image.Sprite.Texture, new Rectangle(loc + new Point(bounds.Size.X - spaceX, bounds.Size.Y - spaceY), image.Spacing), image.SERect, opacity); //SE
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
