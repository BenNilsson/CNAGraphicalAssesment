using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CNAGraphicalApplication
{
    public class SlimeGame : Game
    {
        private GraphicsDeviceManager m_Graphics;
        private SpriteBatch m_SpriteBatch;

        public SlimeGame()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            m_Graphics.PreferredBackBufferWidth = (int)Constants.ScreenDimensions.X;
            m_Graphics.PreferredBackBufferHeight = (int)Constants.ScreenDimensions.Y;
            m_Graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);

            SceneManager.Instance.LoadContent(Content);
        }

        protected override void UnloadContent()
        {
            SceneManager.Instance.UnloadContent();
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            SceneManager.Instance.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(SceneManager.Instance.m_BackgroundColor);

            m_SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            SceneManager.Instance.Draw(m_SpriteBatch);
            m_SpriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            SceneManager.Instance.OnExiting(sender, args);

            base.OnExiting(sender, args);
        }
    }
}
