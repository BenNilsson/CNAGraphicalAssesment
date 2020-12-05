using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

public class Player : GameObject
{
    protected float m_fMovementSpeed = 200f;

    protected int m_iSingleSpriteWidth = 64;
    protected int m_iSingleSpriteHeight = 64;

    protected bool m_bMoving = false;
    protected bool m_bMovingLeft = false;

    protected int m_iFrame = 1;
    protected int m_iFrameCount = 4;
    protected float m_fCurFrameTime = 0;

    public Player(string imagePath, Vector2 startPos) : base(imagePath, startPos)
    {
        Transform.m_Scale = new Vector2(2, 2);
        m_SourceRect = new Rectangle(0, 0, m_iSingleSpriteWidth, m_iSingleSpriteHeight);
        m_Origin = new Vector2(32, 0);
    }

    public override void LoadContent()
    {
        m_Content = new ContentManager(SceneManager.Instance.m_Content.ServiceProvider, "Content");
        m_Texture = m_Content.Load<Texture2D>(m_ImagePath);
        m_SourceRect = new Rectangle(0, 0, m_iSingleSpriteWidth, m_iSingleSpriteHeight);
        m_Origin = new Vector2(m_iSingleSpriteWidth * 0.5f, m_iSingleSpriteHeight);
    }

    public override void UnloadContent()
    {
        base.UnloadContent();
    }

    public override void Update(GameTime gameTime)
    {
        m_bMoving = false;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Rectangle sourceRect = new Rectangle(0, 0, m_iSingleSpriteWidth, m_iSingleSpriteHeight);

        if (m_bMoving)
        {
            // Get position of the sprite sheet
            int left = m_iSingleSpriteWidth * (m_iFrame - 1);

            sourceRect = new Rectangle(left, 0, m_iSingleSpriteWidth, m_iSingleSpriteHeight);
        }

        if (m_bMovingLeft)
            spriteBatch.Draw(m_Texture, Transform.m_Position, sourceRect, Color.White, Transform.m_Angle, m_Origin, Transform.m_Scale, SpriteEffects.FlipHorizontally, 0.0f);
        else
            spriteBatch.Draw(m_Texture, Transform.m_Position, sourceRect, Color.White, Transform.m_Angle, m_Origin, Transform.m_Scale, SpriteEffects.None, 0.0f);
    }

    public override void OnExiting(object sender, EventArgs args)
    {
        base.OnExiting(sender, args);
    }

    protected void MoveLeft(GameTime gameTime)
    {
        float moveLeft = Transform.m_Position.X - m_fMovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (moveLeft > 0 + ((m_iSingleSpriteWidth * Transform.m_Scale.X) * 0.5f))
            Transform.m_Position.X = moveLeft;

        m_bMovingLeft = true;
        m_bMoving = true;
    }

    protected void MoveRight(GameTime gameTime)
    {
        float moveRight = Transform.m_Position.X + m_fMovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (moveRight < Constants.ScreenDimensions.X - ((m_iSingleSpriteWidth * Transform.m_Scale.X) * 0.5f))
            Transform.m_Position.X = moveRight;

        m_bMovingLeft = false;
        m_bMoving = true;
    }

    protected void UpdateAnimationFrame(GameTime gameTime)
    {
        if (m_bMoving)
        {
            m_fCurFrameTime += (float)gameTime.ElapsedGameTime.TotalSeconds * 10f;

            if (m_fCurFrameTime > 1)
            {
                m_iFrame++;

                if (m_iFrame > m_iFrameCount)
                    m_iFrame = 1;

                m_fCurFrameTime = 0;
            }
        }
    }
}
