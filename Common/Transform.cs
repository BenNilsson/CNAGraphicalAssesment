using Microsoft.Xna.Framework;

public class Transform
{
    public Vector2 m_Position;
    public float m_Angle;
    public Vector2 m_Scale;

    public Transform()
    {
        m_Position = Vector2.Zero;
        m_Angle = 0f;
        m_Scale = Vector2.One;
    }

    public Transform(Vector2 pos, float angle, Vector2 scale)
    {
        m_Position = pos;
        m_Angle = angle;
        m_Scale = scale;
    }
}