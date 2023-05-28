using System;
using Microsoft.Xna.Framework;
using onwards.graphics;

namespace onwards.ui
{
    public class Button : View
    {
        public static Sprite DefaultBackground;
        public static Sprite DefaultSelectedBackground;

        public Action<Button> OnClick { get; set; }

        public Image BackgroundImage { get; set; } = new Image
        {
            HorizontalContraint = ViewConstraint.Stretch,
            VerticalContraint = ViewConstraint.Stretch,
        };

        public Sprite BackgroundSprite { get; set; } = DefaultBackground;
        public Sprite SelectedBackgroundSprite { get; set; } = DefaultSelectedBackground;

        public Label Label { get; set; } = new Label
        {
            HorizontalContraint = ViewConstraint.Stretch,
            VerticalContraint = ViewConstraint.Stretch,
        };
        public bool Selected { get; set; }

        public Image Icon { get; set; } = new Image
        {
            Bounds = new Rectangle(0, 0, 32, 32),
            HorizontalContraint = ViewConstraint.Center,
            VerticalContraint = ViewConstraint.Center,
        };

        public Button()
        {
            AddChild(BackgroundImage);
            AddChild(Label);
            AddChild(Icon);
        }

        public override void Draw()
        {
            BackgroundImage.Sprite = Selected ? SelectedBackgroundSprite : BackgroundSprite;

            base.Draw();
        }

        public override void Update()
        {
            base.Update();

            if (IsMouseInside && Input.Mouse.Down(0))
            {
                ConsumedController = true;
            }

            if (ClickedInside())
            {
                OnClick?.Invoke(this);
                ConsumedController = true;
            }
        }
    }
}