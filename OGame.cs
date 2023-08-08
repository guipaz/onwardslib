using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using onwardslib.input;

namespace onwardslib
{
    public abstract class OGame : Game
    {
        protected abstract void GameLoad();
        protected abstract void GameRender();
        protected abstract void GameUpdate();
        protected abstract IEnumerable<Texture2D> RenderToScreen { get; }

        GraphicsDeviceManager _graphicsDeviceManager;

        public OGame()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Engine.Initialize(new SpriteBatch(GraphicsDevice), GraphicsDevice);

            GameLoad();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);

            GameRender();

            GraphicsDevice.SetRenderTarget(null);
            Engine.SpriteBatch.Begin();
            foreach (var toRender in RenderToScreen)
            {
                Engine.SpriteBatch.Draw(toRender,
                                        new Rectangle(0, 0,
                                                      _graphicsDeviceManager.PreferredBackBufferWidth,
                                                      _graphicsDeviceManager.PreferredBackBufferHeight),
                                        Color.White);
            }
            Engine.SpriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            Engine.DeltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;

            Input.Keyboard.Update();
            Input.Mouse.Update();
            Input.Gamepad.Update();

            GameUpdate();

            base.Update(gameTime);
        }

        public void ChangeResolution(int width, int height, bool fullscreen, bool borderless)
        {
            Engine.ViewportResolution = new Point(width, height);
            
            _graphicsDeviceManager.PreferredBackBufferWidth = Engine.ViewportResolution.X;
            _graphicsDeviceManager.PreferredBackBufferHeight = Engine.ViewportResolution.Y;

            _graphicsDeviceManager.IsFullScreen = fullscreen;
            _graphicsDeviceManager.HardwareModeSwitch = !borderless;

            _graphicsDeviceManager.ApplyChanges();
        }
    }
}
