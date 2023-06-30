using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace onwardslib.input
{
    public class Mouse
    {
        public const int LEFT_BUTTON = 0;
        public const int RIGHT_BUTTON = 1;

        MouseState previousState;
        MouseState currentState;

        public int DragButton { get; set; }

        bool isTestingDrag;
        Point pressInitialPos;

        public bool startedDraggingThisFrame;
        public bool isDragging;
        public Point dragDelta;

        Point lastDragPosition;

        const int tolerance = 10;

        public Point Position => currentState.Position;
        public int ScrollDelta { get; set; }

        public bool Pressed(int id)
        {
            var previous = id == 0 ? previousState.LeftButton : previousState.RightButton;
            var current = id == 0 ? currentState.LeftButton : currentState.RightButton;

            return previous != ButtonState.Pressed && current == ButtonState.Pressed;
        }

        public bool Clicked(int id)
        {
            var previous = id == 0 ? previousState.LeftButton : previousState.RightButton;
            var current = id == 0 ? currentState.LeftButton : currentState.RightButton;

            return previous == ButtonState.Pressed && current == ButtonState.Released && !startedDraggingThisFrame && !isDragging;
        }

        public void Update()
        {
            startedDraggingThisFrame = false;
            previousState = currentState;
            currentState = Microsoft.Xna.Framework.Input.Mouse.GetState();

            // drag
            if (isDragging)
            {
                if ((DragButton == LEFT_BUTTON && currentState.LeftButton == ButtonState.Released) ||
                    (DragButton == RIGHT_BUTTON && currentState.RightButton == ButtonState.Released))
                {
                    isDragging = false;
                }
                else
                {
                    dragDelta = currentState.Position - lastDragPosition;
                    lastDragPosition = currentState.Position;
                }
            }
            else if (isTestingDrag)
            {
                if (DragButton == LEFT_BUTTON && currentState.LeftButton == ButtonState.Pressed
                || DragButton == RIGHT_BUTTON && currentState.RightButton == ButtonState.Pressed)
                {
                    var diff = currentState.Position - pressInitialPos;
                    if (Math.Abs(diff.X) >= tolerance || Math.Abs(diff.Y) >= tolerance)
                    {
                        isDragging = true;
                        startedDraggingThisFrame = true;
                        isTestingDrag = false;
                        lastDragPosition = pressInitialPos;
                    }
                }
                else
                {
                    isTestingDrag = false;
                }
            }
            else if (previousState.LeftButton == ButtonState.Released && currentState.LeftButton == ButtonState.Pressed)
            {
                pressInitialPos = currentState.Position;
                isTestingDrag = true;
                DragButton = LEFT_BUTTON;
            }
            else if (previousState.RightButton == ButtonState.Released && currentState.RightButton == ButtonState.Pressed)
            {
                pressInitialPos = currentState.Position;
                isTestingDrag = true;
                DragButton = RIGHT_BUTTON;
            }

            if (previousState.ScrollWheelValue != currentState.ScrollWheelValue)
            {
                ScrollDelta = currentState.ScrollWheelValue - previousState.ScrollWheelValue;
            }
            else
            {
                ScrollDelta = 0;
            }
        }
    }
}
