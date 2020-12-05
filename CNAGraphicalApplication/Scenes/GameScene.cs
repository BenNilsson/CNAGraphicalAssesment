using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Packets;
using System;
using System.Collections.Generic;

public class GameScene : Scene
{
    public Client m_Client;
    public Player m_LocalPlayer;

    public override void LoadContent()
    {
        m_GameObjects = new List<GameObject>();

        // Add Player
        m_LocalPlayer = new Player("SlimeSpriteSheet", new Vector2(Constants.ScreenDimensions.X * 0.5f, Constants.ScreenDimensions.Y));
        m_GameObjects.Add(m_LocalPlayer);

        // Create an instance of the client
        m_Client = new Client();
        if (m_Client.Connect("127.0.0.1", 4444))
            m_Client.Run();
        else Console.WriteLine("Could not connect to the server");

        base.LoadContent();
    }

    public override void UnloadContent()
    {
        base.UnloadContent();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        PlayerMovedPacket movedPacket = new PlayerMovedPacket(m_LocalPlayer.Transform.m_Position.X, m_LocalPlayer.Transform.m_Position.Y);
        m_Client.UDP_SendPacket(movedPacket);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }

    public override void OnExiting(object sender, EventArgs args)
    {
        PlayerDisconnectedPacket disconnectPacket = new PlayerDisconnectedPacket();
        m_Client.TCP_SendPacket(disconnectPacket);

        m_Client.Disconnect();

        base.OnExiting(sender, args);
    }
}

