using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;

public class GameObject
{
    public Transform Transform { get; private set; }

    protected Rectangle m_SourceRect;
    protected Vector2 m_Origin;
    protected Texture2D m_Texture;
    protected ContentManager m_Content;
    protected string m_ImagePath;

    public GameObject(string imagePath, Vector2 startPosition)
    {
        m_ImagePath = imagePath;
        Transform = new Transform();
        Transform.m_Position = startPosition;
    }

    public virtual void LoadContent()
    {
        m_Content = new ContentManager(SceneManager.Instance.m_Content.ServiceProvider, "Content");

        m_Texture = m_Content.Load<Texture2D>(m_ImagePath);
        m_SourceRect = new Rectangle(0, 0, m_Texture.Width, m_Texture.Height);
        m_Origin = new Vector2(m_Texture.Width * 0.5f, m_Texture.Height);
    }

    public virtual void UnloadContent()
    {

    }

    public virtual void Update(GameTime gameTime)
    {

    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(m_Texture, Transform.m_Position, m_SourceRect, Color.White, Transform.m_Angle, m_Origin, Transform.m_Scale, SpriteEffects.None, 0.0f);
    }

    public virtual void OnExiting(object sender, EventArgs args)
    {
        
    }
}