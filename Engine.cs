using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using onwards.ecs;
using onwards.utils;

namespace onwards
{
    public class Engine : Game
    {
        public static Engine Instance { get; set; }

        public int TileSize { get; set; } = 16;

        public static ResolutionConfig Windowed720p { get; } = new ResolutionConfig(1280, 720, false);
        public static ResolutionConfig Windowed900p { get; } = new ResolutionConfig(1600, 900, false);
        public static ResolutionConfig Fullscreen1080p { get; } = new ResolutionConfig(1920, 1080, true);
        
        public GraphicsDevice Graphics { get; private set; }
        public float DeltaTime { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }
        public ResolutionConfig CurrentResolution { get; private set; }
        public Action<Keys, char> OnTextInput { get; set; }

        public FrameCounter FrameCounter = new FrameCounter();
        public float Time { get; private set; }

        GraphicsDeviceManager graphicsManager;

        List<Manager> _managers = new List<Manager>();
        List<Entity> _entities = new List<Entity>();

        public Engine()
        {
            Instance = this;
            graphicsManager = new GraphicsDeviceManager(this);

            Window.TextInput += TextInputHandler;
        }

        protected override void Initialize()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Graphics = GraphicsDevice;

            SetResolution(Windowed720p);

            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            
        }

        public void AddManager(Manager manager)
        {
            _managers.Add(manager);
        }

        public void RemoveManager(Manager manager)
        {
            _managers.Remove(manager);
        }

        public void ClearEntities()
        {
            foreach (var entity in _entities)
            {
                foreach (var manager in _managers)
                {
                    foreach (var component in entity.Components)
                    {
                        manager.RemoveComponent(component);
                    }
                }

                entity.Destroy();
            }

            _entities.Clear();
        }

        public void AddEntity(Entity entity)
        {
            _entities.Add(entity);
            entity.Load();

            entity.OnAddComponent += RegisterComponent;
            entity.OnRemoveComponent += UnregisterComponent;

            foreach (var component in entity.Components)
            {
                RegisterComponent(component);
            }
        }

        void RegisterComponent(Component component)
        {
            foreach (var manager in _managers)
            {
                manager.AddComponent(component);
            }
        }

        void UnregisterComponent(Component component)
        {
            foreach (var manager in _managers)
            {
                manager.RemoveComponent(component);
            }
        }

        public void RemoveEntity(Entity entity)
        {
            entity.Destroy();
            _entities.Remove(entity);
        }

        public void SetResolution(ResolutionConfig config)
        {
            CurrentResolution = config;

            graphicsManager.IsFullScreen = config.IsFullScreen;
            graphicsManager.PreferredBackBufferWidth = config.Width;
            graphicsManager.PreferredBackBufferHeight = config.Height;
            graphicsManager.HardwareModeSwitch = false;
            graphicsManager.ApplyChanges();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Time = (float)gameTime.TotalGameTime.TotalSeconds;
            FrameCounter.Update(deltaTime);
            
            foreach (var manager in _managers)
            {
                manager.Draw();
            }

            base.Draw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update();

            if (Input.Keyboard.Down(Keys.LeftShift) && Input.Keyboard.Down(Keys.Escape))
                Exit();
            
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f; ;
            
            foreach (var manager in _managers)
            {
                manager.Update();
            }

            base.Update(gameTime);
        }

        void RenderToScreen()
        {
            //onwards.Draw.SetCurrentContextRenderer(null);
            //SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
            //    SamplerState.PointClamp, DepthStencilState.Default,
            //    RasterizerState.CullNone);
            //foreach (var renderer in contextRenderers)
            //{
            //    SpriteBatch.Draw(renderer.RenderTarget,
            //        new Rectangle(0, 0, Engine.Graphics.Viewport.Width, Engine.Graphics.Viewport.Height), Color.White);
            //}
            //SpriteBatch.End();
        }

        public void ShowMouse(bool b)
        {
             IsMouseVisible = b;
        }

        void TextInputHandler(object sender, TextInputEventArgs args)
        {
            OnTextInput?.Invoke(args.Key, args.Character);
        }
    }
}