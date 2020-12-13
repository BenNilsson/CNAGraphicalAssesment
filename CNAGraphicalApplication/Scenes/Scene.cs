using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

public class Scene
{
    protected ContentManager m_Content;

    public List<GameObject> m_GameObjects;

    public virtual void LoadContent()
    {
        m_Content = new ContentManager(SceneManager.Instance.m_Content.ServiceProvider, "Content");

        if (m_GameObjects == null) return;
        for (int i = 0; i < m_GameObjects.Count; i++)
        {
            m_GameObjects[i].LoadContent();
        }
    }

    public virtual void UnloadContent()
    {
        if (m_GameObjects == null) return;
        for (int i = 0; i < m_GameObjects.Count; i++)
        {
            m_GameObjects[i].UnloadContent();
        }
    }

    public virtual void Update(GameTime gameTime)
    {
        if (m_GameObjects == null) return;
        for (int i = 0; i < m_GameObjects.Count; i++)
        {
            m_GameObjects[i].Update(gameTime);
        }
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (m_GameObjects == null) return;
        for (int i = 0; i < m_GameObjects.Count; i++)
        {
            m_GameObjects[i].Draw(spriteBatch);
        }
    }

    public virtual void OnExiting(object sender, EventArgs args)
    {
        if (m_GameObjects == null) return;
        for (int i = 0; i < m_GameObjects.Count; i++)
        {
            m_GameObjects[i].OnExiting(sender, args);
        }
    }
}
