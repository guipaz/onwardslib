using onwards.graphics;

namespace onwards.ui
{
    public class Image : View
    {
        public Sprite Sprite { get; set; }

        public override void Draw()
        {
            Sprite?.Draw(Bounds);

            base.Draw();
        }
    }
}