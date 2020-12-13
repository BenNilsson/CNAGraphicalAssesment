using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

public class Player : GameObject
{
    protected float m_fMovementSpeed = 100f;

    protected int m_iSingleSpriteWidth = 64;
    protected int m_iSingleSpriteHeight = 64;

    public bool m_bMoving;
    public bool m_bMovingLeft;

    public int m_iFrame = 1;
    public float m_fCurFrameTime = 0;
    protected int m_iFrameCount = 4;

    public string m_sPlayerUsername;

    private SpriteFont m_Ubuntu32;

    public Player(string imagePath, Vector2 startPos) : base(imagePath, startPos)
    {
        Transform.m_Scale = new Vector2(0.5f);
        m_SourceRect = new Rectangle(0, 0, m_iSingleSpriteWidth, m_iSingleSpriteHeight);
        m_Origin = new Vector2(32, 0);
    }

    public override void LoadContent()
    {
        m_Content = new ContentManager(SceneManager.Instance.m_Content.ServiceProvider, "Content");
        m_Texture = m_Content.Load<Texture2D>(m_ImagePath);
        m_SourceRect = new Rectangle(0, 0, m_iSingleSpriteWidth, m_iSingleSpriteHeight);
        m_Origin = new Vector2(m_iSingleSpriteWidth * 0.5f, m_iSingleSpriteHeight);

        m_Ubuntu32 = m_Content.Load<SpriteFont>("Ubuntu32");
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        // Draw player
        Rectangle sourceRect = new Rectangle(0, 0, m_iSingleSpriteWidth, m_iSingleSpriteHeight);

        // Get position of the sprite sheet
        if (m_bMoving)
        {
            int left = m_iSingleSpriteWidth * (m_iFrame - 1);
            sourceRect = new Rectangle(left, 0, m_iSingleSpriteWidth, m_iSingleSpriteHeight);
        }
        else m_iFrame = 1;

        if (m_bMovingLeft)
            spriteBatch.Draw(m_Texture, Transform.m_Position, sourceRect, Color.White, Transform.m_Angle, m_Origin, Transform.m_Scale, SpriteEffects.FlipHorizontally, 0.0f);
        else
            spriteBatch.Draw(m_Texture, Transform.m_Position, sourceRect, Color.White, Transform.m_Angle, m_Origin, Transform.m_Scale, SpriteEffects.None, 0.0f);

        // Draw username
        if (m_sPlayerUsername != null && m_sPlayerUsername.Length > 0)
        {
            Vector2 textPos = Transform.m_Position;
            Vector2 stringMeasure = m_Ubuntu32.MeasureString(m_sPlayerUsername);
            textPos.Y -= (m_iSingleSpriteHeight + 20) * Transform.m_Scale.Y;
            textPos.X -= (stringMeasure.X * 0.5f) * Transform.m_Scale.X;
            spriteBatch.DrawString(m_Ubuntu32, m_sPlayerUsername, textPos, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
        }
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
        m_bMoving = true;
        m_bMovingLeft = true;
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
