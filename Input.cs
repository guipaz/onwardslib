using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace onwards
{
    public static class Input
    {
        public static MouseHandler Mouse { get; } = new MouseHandler();
        public static KeyboardHandler Keyboard { get; } = new KeyboardHandler();

        public static void Update()
        {
            Mouse.Update();
            Keyboard.Update();
        }

        public class KeyboardHandler
        {
            KeyboardState previousState;
            KeyboardState currentState;

            public void Update()
            {
                previousState = currentState;
                currentState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            }
            
            public bool Pressed(Keys key)
            {
                return !previousState.IsKeyDown(key) && currentState.IsKeyDown(key);
            }

            public bool Released(Keys key)
            {
                return !previousState.IsKeyUp(key) && currentState.IsKeyUp(key);
            }

            public bool Down(Keys key)
            {
                return currentState.IsKeyDown(key);
            }

            public bool AnyKeyPressed()
            {
                return previousState.GetHashCode() != currentState.GetHashCode();
            }
        }

        public class MouseHandler
        {
            public const int LEFT_BUTTON = 0;
            public const int RIGHT_BUTTON = 1;

            const int tolerance = 10;

            public Point Position => currentState.Position;
            public int ScrollDelta { get; private set; }
            public int DragButton { get; private set; }
            public bool IsDragging { get; private set; }
            public Point DragDelta { get; private set; }
            public Point PressInitialPosition { get; private set; }

            bool stoppedDraggingThisFrame;

            MouseState previousState;
            MouseState currentState;
            
            bool isTestingDrag;
            Point lastDragPosition;
            
            public bool Pressed(int id)
            {
                var previous = id == 0 ? previousState.LeftButton : previousState.RightButton;
                var current = id == 0 ? currentState.LeftButton : currentState.RightButton;

                return previous != ButtonState.Pressed && current == ButtonState.Pressed;
            }

            public bool Down(int id)
            {
                var current = id == 0 ? currentState.LeftButton : currentState.RightButton;

                return current == ButtonState.Pressed;
            }

            public bool Up(int id)
            {
                var previous = id == 0 ? previousState.LeftButton : previousState.RightButton;
                var current = id == 0 ? currentState.LeftButton : currentState.RightButton;

                return previous == ButtonState.Pressed && current != ButtonState.Pressed;
            }

            public bool Clicked(int id)
            {
                var previous = id == 0 ? previousState.LeftButton : previousState.RightButton;
                var current = id == 0 ? currentState.LeftButton : currentState.RightButton;

                return previous == ButtonState.Pressed && current == ButtonState.Released && !IsDragging && !stoppedDraggingThisFrame;
            }

            public void Update()
            {
                stoppedDraggingThisFrame = false;
                previousState = currentState;
                currentState = Microsoft.Xna.Framework.Input.Mouse.GetState();

                // drag
                if (IsDragging)
                {
                    if ((DragButton == LEFT_BUTTON && currentState.LeftButton == ButtonState.Released) ||
                        (DragButton == RIGHT_BUTTON && currentState.RightButton == ButtonState.Released))
                    {
                        stoppedDraggingThisFrame = true;
                        IsDragging = false;
                    }
                    else
                    {
                        DragDelta = currentState.Position - lastDragPosition;
                        lastDragPosition = currentState.Position;
                    }
                }
                else if (isTestingDrag)
                {
                    if (DragButton == LEFT_BUTTON && currentState.LeftButton == ButtonState.Pressed
                    || DragButton == RIGHT_BUTTON && currentState.RightButton == ButtonState.Pressed)
                    {
                        var diff = currentState.Position - PressInitialPosition;
                        if (Math.Abs(diff.X) >= tolerance || Math.Abs(diff.Y) >= tolerance)
                        {
                            IsDragging = true;
                            isTestingDrag = false;
                            PressInitialPosition = currentState.Position;
                            lastDragPosition = PressInitialPosition;
                        }
                    }
                    else
                    {
                        isTestingDrag = false;
                    }
                }
                else if (previousState.LeftButton == ButtonState.Released && currentState.LeftButton == ButtonState.Pressed)
                {
                    PressInitialPosition = currentState.Position;
                    isTestingDrag = true;
                    DragButton = LEFT_BUTTON;
                }
                else if (previousState.RightButton == ButtonState.Released && currentState.RightButton == ButtonState.Pressed)
                {
                    PressInitialPosition = currentState.Position;
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
}