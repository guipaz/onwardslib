using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace onwardslib
{
    public class OGame : Game
    {
        GraphicsDeviceManager _graphicsDeviceManager;

        IMaestro _maestro;

        public static Point ViewportResolution { get; private set; }

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
        }

        protected override void Draw(GameTime gameTime)
        {
            _maestro.Draw();

            base.Draw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            _maestro.Update();

            base.Update(gameTime);
        }

        public void ChangeResolution(int x, int y, bool fullscreen)
        {
            ViewportResolution = new Point(x, y);
            _graphicsDeviceManager.IsFullScreen = fullscreen;
            _graphicsDeviceManager.PreferredBackBufferWidth = ViewportResolution.X;
            _graphicsDeviceManager.PreferredBackBufferHeight = ViewportResolution.Y;
            _graphicsDeviceManager.HardwareModeSwitch = false;
            _graphicsDeviceManager.ApplyChanges();
        }
    }
}
