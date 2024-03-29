﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace onwardslib
{
    public static class Engine
    {
        public static string DataFolder { get; set; } = @"..\..\..\data";

        public static float DeltaTime { get; set; }
        public static SpriteBatch SpriteBatch { get; private set; }
        public static GraphicsDevice GraphicsDevice { get; private set; }
        public static Point ViewportResolution { get; set; }

        public static void Initialize(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            SpriteBatch = spriteBatch;
            GraphicsDevice = graphicsDevice;
        }
    }
}
