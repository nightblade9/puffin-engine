using Puffin.Core;
using Puffin.Core.Ecs.Systems;
using Puffin.Infrastructure.MonoGame.Drawing;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Puffin.Infrastructure.MonoGame.IO;

namespace Puffin.Infrastructure.MonoGame
{
    public class PuffinGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont defaultFont;
        private Scene currentScene;

        public PuffinGame()
        {
            this.graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        public void ShowScene(Scene s)
        {
            if (this.currentScene != null)
            {
                this.currentScene.Dispose();
            }
            
            var drawingSurface = new MonoGameDrawingSurface(this.GraphicsDevice, spriteBatch, this.defaultFont);

            var systems = new ISystem[]
            {
                new DrawingSystem(drawingSurface),
            };

            s.Initialize(systems, new MonoGameMouseProvider());

            this.currentScene = s;
        }

        virtual protected void Ready()
        {

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Not used since we currently load sprites outside the pipeline
            this.defaultFont = Content.Load<SpriteFont>("OpenSans");

            this.Ready();
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //   Exit();

            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            if (this.currentScene != null)
            {
                this.currentScene.OnUpdate();
            }

            base.Draw(gameTime);
        }
    }
}