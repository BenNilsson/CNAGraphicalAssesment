using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

public class ConnectScene : Scene
{
    public Client m_Client;

    GameObject m_Logo;

    SpriteFont m_SpriteFont;
    string m_sHelpText;

    Button m_SlimeGreenButton;
    GameObject m_SlimeGreenSingle;

    Button m_SlimePinkButton;
    GameObject m_SlimePinkSingle;

    Button m_SlimeBlueButton;
    GameObject m_SlimeBlueSingle;

    public override void LoadContent()
    {
        // Create a new instance of content
        m_Content = new ContentManager(SceneManager.Instance.m_Content.ServiceProvider, "Content");

        // Create a list of gameobjects for the scene to update
        m_GameObjects = new List<GameObject>();

        // Create an instance of the client
        m_Client = new Client();

        // Background color
        SceneManager.Instance.m_BackgroundColor = new Color(15, 15, 15);

        m_SpriteFont = m_Content.Load<SpriteFont>("Ubuntu32");
        m_sHelpText = "Choose your character!";

        // Logo
        m_Logo = new GameObject("SlimeLogo", new Vector2(325, 150));
        m_Logo.Transform.m_Scale = new Vector2(1.5f);
        m_GameObjects.Add(m_Logo);

        // GameObjects
        m_SlimeGreenSingle = new GameObject("SlimeGreenSingle", new Vector2(130f, 375));
        m_SlimeGreenSingle.Transform.m_Scale = new Vector2(2f);
        m_SlimePinkSingle = new GameObject("SlimePinkSingle", new Vector2(310f, 375));
        m_SlimePinkSingle.Transform.m_Scale = new Vector2(2f);
        m_SlimeBlueSingle = new GameObject("SlimeBlueSingle", new Vector2(490f, 375));
        m_SlimeBlueSingle.Transform.m_Scale = new Vector2(2f);

        m_GameObjects.Add(m_SlimeGreenSingle);
        m_GameObjects.Add(m_SlimePinkSingle);
        m_GameObjects.Add(m_SlimeBlueSingle);

        // Buttons
        m_SlimeGreenButton = new Button("ButtonImageSmall", new Vector2(55f, 400f), "Green Slime", m_Content.Load<SpriteFont>("Ubuntu32"), Color.Green);
        m_SlimeGreenButton.OnClick += GreenSlimeClicked;
        m_GameObjects.Add(m_SlimeGreenButton);

        m_SlimePinkButton = new Button("ButtonImageSmall", new Vector2(235f, 400f), "Pink Slime", m_Content.Load<SpriteFont>("Ubuntu32"), Color.HotPink);
        m_SlimePinkButton.OnClick += PinkSlimeClicked;
        m_GameObjects.Add(m_SlimePinkButton);
        
        m_SlimeBlueButton = new Button("ButtonImageSmall", new Vector2(415f, 400f), "Blue Slime", m_Content.Load<SpriteFont>("Ubuntu32"), Color.DarkCyan);
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

        // Draw text
        if (!string.IsNullOrEmpty(m_sHelpText))
        {
            float x = (Constants.ScreenDimensions.X * 0.5f) - (m_SpriteFont.MeasureString(m_sHelpText).X * 0.5f);

            // Draw text
            spriteBatch.DrawString(m_SpriteFont, m_sHelpText, new Vector2(x, 200f), Color.White);
        }
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

