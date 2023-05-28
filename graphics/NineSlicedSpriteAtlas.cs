using System;
using Microsoft.Xna.Framework;

namespace onwards.graphics
{
    public class NineSlicedSprite : Sprite
    {
        public int Scale { get; set; } = 2;
        public int SliceSize { get; set; }
        
        Rectangle[] offsets;

        Rectangle workingRectangle;

        public NineSlicedSprite(OTexture texture, int x, int y, int size) : base(texture, Rectangle.Empty)
        {
            SliceSize = size;
            
            offsets = new[]
            {
                new Rectangle(x, y, SliceSize, SliceSize),
                new Rectangle(x + SliceSize, y, SliceSize, SliceSize),
                new Rectangle(x + SliceSize * 2, y, SliceSize, SliceSize),
                new Rectangle(x, y + SliceSize, SliceSize, SliceSize),
                new Rectangle(x + SliceSize, y +SliceSize, SliceSize, SliceSize),
                new Rectangle(x + SliceSize * 2, y +SliceSize, SliceSize, SliceSize),
                new Rectangle(x, y +SliceSize * 2, SliceSize, SliceSize),
                new Rectangle(x + SliceSize, y +SliceSize * 2, SliceSize, SliceSize),
                new Rectangle(x + SliceSize * 2, y + SliceSize * 2, SliceSize, SliceSize)
            };
        }

        public override void Draw(Rectangle destinationRectangle, float opacity = 1f)
        {
            var size = SliceSize * Scale;
            workingRectangle.Width = size;
            workingRectangle.Height = size;

            var xTimes = Math.Ceiling((destinationRectangle.Width - size * 2) / (double)size);
            var yTimes = Math.Ceiling((destinationRectangle.Height - size * 2) / (double)size);

            // top
            DrawSlice(0, destinationRectangle.X, destinationRectangle.Y);
            for (int i = 1; i <= xTimes; i++)
            {
                DrawSlice(1, destinationRectangle.X + size * i, destinationRectangle.Y);
            }
            DrawSlice(2, destinationRectangle.X + destinationRectangle.Width - size, destinationRectangle.Y);
            
            // middle
            for (int i = 1; i <= yTimes; i++)
            {
                DrawSlice(3, destinationRectangle.X, destinationRectangle.Y + size * i);

                for (int j = 1; j <= xTimes; j++)
                {
                    DrawSlice(4, destinationRectangle.X + size * j, destinationRectangle.Y + size * i);
                }

                DrawSlice(5, destinationRectangle.X + destinationRectangle.Width - size, destinationRectangle.Y + size * i);
            }

            // bottom
            DrawSlice(6, destinationRectangle.X, destinationRectangle.Y + destinationRectangle.Height - size);
            for (int i = 1; i <= xTimes; i++)
            {
                DrawSlice(7, destinationRectangle.X + size * i, destinationRectangle.Y + destinationRectangle.Height - size);
            }
            DrawSlice(8, destinationRectangle.X + destinationRectangle.Width - size, destinationRectangle.Y + destinationRectangle.Height - size);
        }
        
        void DrawSlice(int position, int x, int y)
        {
            SourceRectangle = offsets[position];
            workingRectangle.X = x;
            workingRectangle.Y = y;
            base.Draw(workingRectangle);
        }
    }
}