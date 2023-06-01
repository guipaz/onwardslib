using Microsoft.Xna.Framework.Input;

namespace onwardslib.input
{
    public class Keyboard
    {
        public KeyboardState LastState { get; set; }
        public KeyboardState CurrentState { get; set; }

        public bool Pressed(Keys key)
        {
            return !LastState.IsKeyDown(key) && CurrentState.IsKeyDown(key);
        }

        public bool Released(Keys key)
        {
            return !LastState.IsKeyUp(key) && CurrentState.IsKeyUp(key);
        }

        public bool Down(Keys key)
        {
            return CurrentState.IsKeyDown(key);
        }

        public bool AnyKeyPressed()
        {
            return LastState.GetHashCode() != CurrentState.GetHashCode();
        }

        public void Update()
        {
            LastState = CurrentState;
            CurrentState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
        }
    }
}
