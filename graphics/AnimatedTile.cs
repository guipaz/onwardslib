using Microsoft.Xna.Framework;

namespace onwards.graphics
{
    public class AnimatedTile : Sprite
    {
        float frameRate = 0.35f;
        
        float duration;

        int amountOfFrames;
        Point size = new Point(16);

        Rectangle _workingRect;

        public AnimatedTile(OTexture texture, Rectangle animationSourceRectangle, Point? offset = null) : base(texture, Rectangle.Empty, offset)
        {
            _workingRect = animationSourceRectangle;
            _workingRect.Width = size.X;
            _workingRect.Height = size.Y;

            amountOfFrames = animationSourceRectangle.Width / size.X;
            duration = amountOfFrames * frameRate;
        }
        
        public override void Draw(Point position, float opacity = 1f)
        {
            UpdateRectangle();

            base.Draw(position, opacity);
        }

        public override void Draw(Rectangle destinationRectangle, float opacity = 1f)
        {
            UpdateRectangle();

            base.Draw(destinationRectangle, opacity);
        }

        void UpdateRectangle()
        {
            var time = Engine.Instance.Time;

            var delta = time % duration;
            var ratio = delta / duration;
            var frame = (int)(amountOfFrames * ratio);

            _workingRect.X = frame * size.X;
            SourceRectangle = _workingRect;
        }
    }
}