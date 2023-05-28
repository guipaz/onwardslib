using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using onwards.graphics;

namespace onwards.ui
{
    public class InputField : View
    {
        public static Sprite DefaultBackground;

        bool _focused;

        public Action<string> OnConfirm { get; set; }
        public Action OnCancel { get; set; }

        public bool Focused
        {
            get => _focused;
            set
            {
                _focused = value;

                if (_focused)
                {
                    Engine.Instance.OnTextInput = OnTextInput;
                }
                else
                {
                    if (Engine.Instance.OnTextInput == OnTextInput)
                    {
                        Engine.Instance.OnTextInput = null;
                    }
                }
            }
        }

        public Image BackgroundImage { get; set; } = new Image
        {
            Sprite = DefaultBackground,
            HorizontalContraint = ViewConstraint.Stretch,
            VerticalContraint = ViewConstraint.Stretch
        };
        public string Text { get; private set; }

        public Label Placeholder { get; } = new Label
        {
            Color = Color.Brown * 0.8f,
            HorizontalContraint = ViewConstraint.Stretch,
            VerticalContraint = ViewConstraint.Stretch
        };

        public Label Label { get; } = new Label
        {
            HorizontalContraint = ViewConstraint.Stretch,
            VerticalContraint = ViewConstraint.Stretch
        };

        public InputField()
        {
            AddChild(BackgroundImage);
            AddChild(Placeholder);
            AddChild(Label);
        }

        public void Set(string text)
        {
            Text = text;
            Label.Text = Text;
            if (string.IsNullOrEmpty(Text))
            {
                Placeholder.Active = true;
            }
            else
            {
                Placeholder.Active = false;
            }
        }

        void OnTextInput(Keys key, char character)
        {
            if (key == Keys.Enter)
            {
                Engine.Instance.OnTextInput = null;
                OnConfirm?.Invoke(Text);
            }
            else if (key == Keys.Back)
            {
                if (Text.Length > 0)
                {
                    Text = Text[..^1];
                }
            }
            else if (key == Keys.Escape)
            {
                Engine.Instance.OnTextInput = null;
                OnCancel?.Invoke();
            }
            else
            {
                Text += character;
            }

            Set(Text);
        }

        public override void Update()
        {
            base.Update();

            if (Input.Mouse.Clicked(0))
            {
                Focused = Bounds.Contains(Input.Mouse.Position);
            }
        }
    }
}