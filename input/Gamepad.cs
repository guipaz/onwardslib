using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace onwardslib.input
{
    public class Gamepad
    {
        GamePadState gamepadLast;
        GamePadState gamepadCurrent;

        float thumbstickThreshold = 0.1f;

        public bool Pressed(Buttons key)
        {
            return !gamepadLast.IsButtonDown(key) && gamepadCurrent.IsButtonDown(key);
        }

        public Vector2 GetLeftThumbstick()
        {
            return gamepadCurrent.ThumbSticks.Left;
        }

        private bool CheckThumbstickDown(Buttons key, bool allowDiagonals = false)
        {
            if (key == Buttons.LeftThumbstickDown)
            {
                var y = gamepadCurrent.ThumbSticks.Left.Y;
                return y <= -thumbstickThreshold && (allowDiagonals || Math.Abs(y) > Math.Abs(gamepadCurrent.ThumbSticks.Left.X));
            }

            if (key == Buttons.LeftThumbstickUp)
            {
                var y = gamepadCurrent.ThumbSticks.Left.Y;
                return y >= thumbstickThreshold && (allowDiagonals || Math.Abs(y) > Math.Abs(gamepadCurrent.ThumbSticks.Left.X));
            }

            if (key == Buttons.LeftThumbstickLeft)
            {
                var x = gamepadCurrent.ThumbSticks.Left.X;
                return x <= -thumbstickThreshold && (allowDiagonals || Math.Abs(x) > Math.Abs(gamepadCurrent.ThumbSticks.Left.Y));
            }

            if (key == Buttons.LeftThumbstickRight)
            {
                var x = gamepadCurrent.ThumbSticks.Left.X;
                return x >= thumbstickThreshold && (allowDiagonals || Math.Abs(x) > Math.Abs(gamepadCurrent.ThumbSticks.Left.Y));
            }

            return false;
        }

        public bool Released(Buttons key)
        {
            return !gamepadLast.IsButtonUp(key) && gamepadCurrent.IsButtonUp(key);
        }

        public bool Down(Buttons key)
        {
            if (key == Buttons.LeftThumbstickDown ||
                key == Buttons.LeftThumbstickLeft ||
                key == Buttons.LeftThumbstickRight ||
                key == Buttons.LeftThumbstickUp)
            {
                return CheckThumbstickDown(key, true);
            }

            return gamepadCurrent.IsButtonDown(key);
        }

        public void Update()
        {
            gamepadLast = gamepadCurrent;
            gamepadCurrent = GamePad.GetState(PlayerIndex.One);
        }

        public bool AnyButtonPressed()
        {
            return gamepadLast.GetHashCode() != gamepadCurrent.GetHashCode();
        }
    }
}
