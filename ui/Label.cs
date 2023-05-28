using Microsoft.Xna.Framework;
using onwards.monobmfont;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace onwards.ui
{
    public enum LabelAlignment
    {
        Start, Center, End
    }

    public class Label : View
    {
        public static BMFont DefaultFont;
        public static Color DefaultColor = Color.White;

        string _text;

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                dirty = true;
            }
        }

        public BMFont Font { get; set; } = DefaultFont;
        public Color Color { get; set; } = DefaultColor;
        public LabelAlignment HorizontalAlignment { get; set; } = LabelAlignment.Center;
        public LabelAlignment VerticalAlignment { get; set; } = LabelAlignment.Center;

        Vector2 _renderPosition;

        public override void Draw()
        {
            if (Text != null && Font != null)
            {
                Engine.Instance.SpriteBatch.DrawString(Font, Text, new Vector2(Bounds.X + _renderPosition.X, Bounds.Y + _renderPosition.Y), Color);
            }

            base.Draw();
        }

        public override void UpdateBounds()
        {
            base.UpdateBounds();

            var x = 0;
            var y = 0;

            var measure = Font != null && Text != null ? Font.MeasureString(Text) : Vector2.Zero;

            switch (HorizontalAlignment)
            {
                case LabelAlignment.Start:
                    break;
                case LabelAlignment.Center:
                    x = globalBounds.Width / 2 - (int)(measure.X / 2f);
                    break;
                case LabelAlignment.End:
                    x = globalBounds.Width - (int)measure.X;
                    break;
            }

            switch (VerticalAlignment)
            {
                case LabelAlignment.Start:
                    break;
                case LabelAlignment.Center:
                    y = globalBounds.Height / 2 - (int)(measure.Y / 2f);
                    break;
                case LabelAlignment.End:
                    y = globalBounds.Height - (int)measure.Y;
                    break;
            }

            _renderPosition.X = x;
            _renderPosition.Y = y;
        }
    }
}