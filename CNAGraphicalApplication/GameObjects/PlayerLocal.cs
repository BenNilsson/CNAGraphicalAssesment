using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class PlayerLocal : Player
{
    public PlayerLocal(string path, Vector2 startPosition) : base(path, startPosition)
    {
        
    }

    public override void Update(GameTime gameTime)
    {
        m_bMoving = false;

        var keyboardState = Keyboard.GetState();
        

        if (keyboardState.IsKeyDown(Keys.Left))
        {
            MoveLeft(gameTime);
        }

        if (keyboardState.IsKeyDown(Keys.Right))
        {
            MoveRight(gameTime);
        }

        UpdateAnimationFrame(gameTime);
    }

    
}