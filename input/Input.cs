namespace onwardslib.input
{
    public static class Input
    {
        public static Keyboard Keyboard { get; } = new Keyboard();
        public static Mouse Mouse { get; } = new Mouse();
        public static Gamepad Gamepad { get; } = new Gamepad();
    }
}
