using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


public class Button : GameObject
{
    private MouseState m_CurMouseState;
    private MouseState m_PreviousMouseState;

    private SpriteFont m_SpriteFont;

    private bool m_bIsHovering;

    public event EventHandler OnClick;

    public Color TextColor { get; set; }

    public Rectangle Rectangle { get { return new Rectangle((int)Transform.m_Position.X, (int)Transform.m_Position.Y, m_Texture.Width, m_Texture.Height); } }

    public string Text { get; set; }

    public Button(string imagePath, Vector2 position, string text, SpriteFont font, Color textColor) : base(imagePath, position)
    {
        m_SpriteFont = font;
        Text = text;
        TextColor = textColor;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        // Set button color based on hovering
        Color btnColor = Color.White;

        if (!m_bIsHovering)
            btnColor = Color.LightGray;

        // Draw button
        spriteBatch.Draw(m_Texture, Rectangle, btnColor);

        // Center text if not empty
        if (!string.IsNullOrEmpty(Text))
        { 
            float x = (Rectangle.X + (Rectangle.Width * 0.5f)) - (m_SpriteFont.MeasureString(Text).X * 0.5f);
            float y = (Rectangle.Y + (Rectangle.Height * 0.5f)) - (m_SpriteFont.MeasureString(Text).Y * 0.5f);

            // Draw text
            spriteBatch.DrawString(m_SpriteFont, Text, new Vector2(x, y), TextColor);
        }
    }

    public override void Update(GameTime gameTime)
    {
        // Set current and previous mouse state
        m_PreviousMouseState = m_CurMouseState;
        m_CurMouseState = Mouse.GetState();

        // Create a new rectangle based on the mouse location
        Rectangle mouseRect = new Rectangle(m_CurMouseState.X, m_CurMouseState.Y, 1, 1);

        // Reset hover state and re-calculate it
        m_bIsHovering = false;

        if (mouseRect.Intersects(Rectangle))
        {
            m_bIsHovering = true;

            // Fire click event if clicked
            if (m_CurMouseState.LeftButton == ButtonState.Pressed && m_PreviousMouseState.LeftButton == ButtonState.Pressed)
                OnClick?.Invoke(this, new EventArgs());
        }
    }
}

