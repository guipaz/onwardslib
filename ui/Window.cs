using Microsoft.Xna.Framework.Input;

namespace onwards.ui
{
    public class Window : View
    {
        public override void Update()
        {
            base.Update();

            ConsumedController = true;

            if (Input.Keyboard.Pressed(Keys.Escape))
            {
                SlatedForRemoval = true;
            }
        }
    }
}