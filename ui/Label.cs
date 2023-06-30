using Microsoft.Xna.Framework;
using monobmfont;

namespace onwardslib.ui
{
    public class Label : View
    {
        public BMFont Font { get; set; }
        public string Text { get; set; }
        public Color Color { get; set; } = Color.White;

        public Label()
        {
            Font = FontLoader.Load("kubasta_24");
        }
    }
}
