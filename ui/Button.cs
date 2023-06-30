namespace onwardslib.ui
{
    public class Button : View
    {
        public Action OnClick { get; set; }
        public Image BackgroundImage { get; set; }
        public Label TextLabel { get; set; }

        public Button()
        {
            BackgroundImage = new Image();
            TextLabel = new Label();

            AddChild(BackgroundImage);
            AddChild(TextLabel);
        }
    }
}
