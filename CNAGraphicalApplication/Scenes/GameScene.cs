using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Packets;
using System;
using System.Collections.Generic;

public class GameScene : Scene
{
    public Client m_Client;

    public GameScene(Client client)
    {
        m_Client = client;
    }

    public PlayerLocal m_LocalPlayer;

    public override void LoadContent()
    {
        m_Content = new ContentManager(SceneManager.Instance.m_Content.ServiceProvider, "Content");
        
        // Create a list of gameobjects for the scene to update
        m_GameObjects = new List<GameObject>();

        // Add Player
        m_LocalPlayer = new PlayerLocal(m_Client.m_sCharacterLoaded, new Vector2(Constants.ScreenDimensions.X * 0.5f, Constants.ScreenDimensions.Y));
        m_GameObjects.Add(m_LocalPlayer);


        base.LoadContent();
    }

    public override void UnloadContent()
    {
        base.UnloadContent();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        PlayerMovedPacket movedPacket = new PlayerMovedPacket(m_LocalPlayer.Transform.m_Position.X, m_LocalPlayer.Transform.m_Position.Y, m_LocalPlayer.m_bMovingLeft);
        m_Client.UDP_SendPacket(movedPacket);

        PlayerAnimationPacket animationPacket = new PlayerAnimationPacket(m_LocalPlayer.m_iFrame, m_LocalPlayer.m_fCurFrameTime);
        m_Client.UDP_SendPacket(animationPacket);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }

    public override void OnExiting(object sender, EventArgs args)
    {
        PlayerDisconnectedPacket disconnectedPacket = new PlayerDisconnectedPacket();
        m_Client.TCP_SendPacket(disconnectedPacket);

        m_Client.Disconnect();

        base.OnExiting(sender, args);
    }
}

