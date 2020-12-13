using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

public class ConnectScene : Scene
{
    public Client m_Client;

    Button m_SlimeGreenButton;
    Button m_SlimePinkButton;
    Button m_SlimeBlueButton;

    public override void LoadContent()
    {
        // Create a new instance of content
        m_Content = new ContentManager(SceneManager.Instance.m_Content.ServiceProvider, "Content");

        // Create a list of gameobjects for the scene to update
        m_GameObjects = new List<GameObject>();

        // Create an instance of the client
        m_Client = new Client();

        // Buttons
        m_SlimeGreenButton = new Button("ButtonImageSmall", new Vector2(55f, 300f), "Green Slime", m_Content.Load<SpriteFont>("Ubuntu32"));
        m_SlimeGreenButton.OnClick += GreenSlimeClicked;
        m_GameObjects.Add(m_SlimeGreenButton);

        m_SlimePinkButton = new Button("ButtonImageSmall", new Vector2(235f, 300f), "Pink Slime", m_Content.Load<SpriteFont>("Ubuntu32"));
        m_SlimePinkButton.OnClick += PinkSlimeClicked;
        m_GameObjects.Add(m_SlimePinkButton);
        
        m_SlimeBlueButton = new Button("ButtonImageSmall", new Vector2(415f, 300f), "Blue Slime", m_Content.Load<SpriteFont>("Ubuntu32"));
        m_SlimeBlueButton.OnClick += BlueSlimeClicked;
        m_GameObjects.Add(m_SlimeBlueButton);

        base.LoadContent();
    }

    private void BlueSlimeClicked(object sender, EventArgs e)
    {
        m_Client.m_sCharacterLoaded = "SlimeBlueSpriteSheet";
        Connect();
    }

    private void PinkSlimeClicked(object sender, EventArgs e)
    {
        m_Client.m_sCharacterLoaded = "SlimePinkSpriteSheet";
        Connect();
    }

    private void GreenSlimeClicked(object sender, EventArgs e)
    {
        m_Client.m_sCharacterLoaded = "SlimeSpriteSheet";
        Connect();
    }

    public override void UnloadContent()
    {
        m_SlimeGreenButton.OnClick -= GreenSlimeClicked;
        m_SlimePinkButton.OnClick -= PinkSlimeClicked;
        m_SlimeBlueButton.OnClick -= BlueSlimeClicked;
        
        base.UnloadContent();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }

    public override void OnExiting(object sender, EventArgs args)
    {
        base.OnExiting(sender, args);
    }

    public void Connect()
    {
        SceneManager.Instance.LoadScene(new GameScene(m_Client));
        if (m_Client.Connect("127.0.0.1", 4444))
            m_Client.Run("Player");
        else Console.WriteLine("Could not connect to the server");
    }
}

