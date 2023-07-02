using Microsoft.Xna.Framework;

namespace onwardslib.ui
{
    public class SlicedImage : Image
    {
        public Point Spacing { get; } = new Point(4);

        public Rectangle NWRect { get; }
        public Rectangle NRect { get; }
        public Rectangle NERect { get; }
        public Rectangle WRect { get; }
        public Rectangle CRect { get; }
        public Rectangle ERect { get; }
        public Rectangle SWRect { get; }
        public Rectangle SRect { get; }
        public Rectangle SERect { get; }

        public SlicedImage(Sprite sprite, Point? spacing = null)
        {
            Sprite = sprite;

            if (spacing != null)
            {
                Spacing = (Point)spacing;
            }
            
            NWRect = new Rectangle(sprite.SourceRectangle.Location, Spacing);
            NRect = new Rectangle(sprite.SourceRectangle.Location + new Point(Spacing.X, 0), Spacing);
            NERect = new Rectangle(sprite.SourceRectangle.Location + new Point(Spacing.X * 2, 0), Spacing);

            WRect = new Rectangle(sprite.SourceRectangle.Location + new Point(0, Spacing.Y), Spacing);
            CRect = new Rectangle(sprite.SourceRectangle.Location + new Point(Spacing.X, Spacing.Y), Spacing);
            ERect = new Rectangle(sprite.SourceRectangle.Location + new Point(Spacing.X * 2, Spacing.Y), Spacing);

            SWRect = new Rectangle(sprite.SourceRectangle.Location + new Point(0, Spacing.Y * 2), Spacing);
            SRect = new Rectangle(sprite.SourceRectangle.Location + new Point(Spacing.X, Spacing.Y * 2), Spacing);
            SERect = new Rectangle(sprite.SourceRectangle.Location + new Point(Spacing.X * 2, Spacing.Y * 2), Spacing);
        }
    }
}
