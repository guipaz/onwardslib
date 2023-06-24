using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using onwardslib.input;

namespace onwardslib
{
    public class OGame : Game
    {
        public static OGame Instance { get; protected set; }

        GraphicsDeviceManager _graphicsDeviceManager;

        IMaestro _maestro;

        public OGame(IMaestro maestro)
        {
            Instance = this;

            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            _maestro = maestro;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Engine.Initialize(new SpriteBatch(GraphicsDevice), GraphicsDevice);

            _maestro.Load();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);

            _maestro.Draw();

            GraphicsDevice.SetRenderTarget(null);
            Engine.SpriteBatch.Begin();
            foreach (var toRender in _maestro.ToRender)
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

            _maestro.Update();

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
