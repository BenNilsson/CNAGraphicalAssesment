using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

public class SceneManager
{
    private static SceneManager m_Instance;

    public ContentManager m_Content { private set; get; }

    public Scene m_CurrentScene;

    public static SceneManager Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new SceneManager();
            return m_Instance;
        }
    }

    public SceneManager()
    {
        m_CurrentScene = new GameScene();
    }

    public void LoadContent(ContentManager content)
    {
        this.m_Content = new ContentManager(content.ServiceProvider, "Content");
        m_CurrentScene.LoadContent();
    }

    public void UnloadContent()
    {
        m_CurrentScene.UnloadContent();
    }

    public void Update(GameTime gameTime)
    {
        m_CurrentScene.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        m_CurrentScene.Draw(spriteBatch);
    }

    public void OnExiting(object sender, EventArgs args)
    {
        m_CurrentScene.OnExiting(sender, args);
    }
}
