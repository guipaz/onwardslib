using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using onwardslib.input;

namespace onwardslib
{
    public class OGame : Game
    {
        GraphicsDeviceManager _graphicsDeviceManager;

        IMaestro _maestro;

        public OGame(IMaestro maestro)
        {
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

            Onwards.Initialize(new SpriteBatch(GraphicsDevice), GraphicsDevice);

            _maestro.Load();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);

            _maestro.Draw();

            base.Draw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            Onwards.DeltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;

            Input.Keyboard.Update();
            Input.Mouse.Update();
            Input.Gamepad.Update();

            _maestro.Update();

            base.Update(gameTime);
        }

        public void ChangeResolution(int x, int y, bool fullscreen)
        {
            Onwards.ViewportResolution = new Point(x, y);
            _graphicsDeviceManager.IsFullScreen = fullscreen;
            _graphicsDeviceManager.PreferredBackBufferWidth = Onwards.ViewportResolution.X;
            _graphicsDeviceManager.PreferredBackBufferHeight = Onwards.ViewportResolution.Y;
            _graphicsDeviceManager.HardwareModeSwitch = false;
            _graphicsDeviceManager.ApplyChanges();
        }
    }
}
