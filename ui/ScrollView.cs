using System;
using onwards.graphics;
using onwards.utils;

namespace onwards.ui
{
    public abstract class ScrollView : View
    {
        public Image Background { get; } = new Image
        {
            HorizontalContraint = ViewConstraint.Stretch,
            VerticalContraint = ViewConstraint.Stretch
        };

        public int StartAt { get; set; }
        public abstract int VisibleRows { get; }
        public abstract int TotalRows { get; }
        public Action<int> OnScroll { get; set; }

        protected ScrollView()
        {
            Background.Sprite = new NineSlicedSprite(TextureLoader.Get("ui"), 0, 0, 8);
            AddChild(Background);
        }

        public override void Update()
        {
            if (Bounds.Contains(Input.Mouse.Position))
            {
                if (Input.Mouse.ScrollDelta != 0)
                {
                    if (Input.Mouse.ScrollDelta < 0)
                    {
                        StartAt++;
                    }
                    else if (Input.Mouse.ScrollDelta > 0)
                    {
                        StartAt--;
                    }

                    if (StartAt > TotalRows - VisibleRows)
                    {
                        StartAt = TotalRows - VisibleRows;
                    }

                    if (StartAt < 0)
                    {
                        StartAt = 0;
                    }

                    OnScroll?.Invoke(StartAt);
                }
            }

            base.Update();
        }
    }
}