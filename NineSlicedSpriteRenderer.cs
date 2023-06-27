using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace onwardslib
{
    public class NineSliceSpriteRenderer
    {
        public Texture2D Texture { get; set; }
        public int Scale { get; set; } = 1;

        private int _sourceX;
        public int SourceX
        {
            get => _sourceX;
            set
            {
                _sourceX = value;
                UpdateOffsets();
            }
        }

        private int _sourceY;

        public int SourceY
        {
            get => _sourceY;
            set
            {
                _sourceY = value;
                UpdateOffsets();
            }
        }

        private int _sliceSize;
        public int SliceSize
        {
            get => _sliceSize;
            set
            {
                _sliceSize = value;
                UpdateOffsets();
            }
        }

        Rectangle[] _offsets;
        Rectangle _workingRectangle;

        void UpdateOffsets()
        {
            var x = SourceX;
            var y = SourceY;

            _offsets = new[]
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

        public void Render()
        {
            var destinationRectangle = new Rectangle();//_rectTransform.Bounds;

            var size = SliceSize * Scale;
            _workingRectangle.Width = size;
            _workingRectangle.Height = size;

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
            _workingRectangle.X = x;
            _workingRectangle.Y = y;

            ODraw.Draw(Texture, _workingRectangle, _offsets[position]);
        }
    }
}
