using System;

[Serializable]
public class PlayerInformation
{
    public string m_sName;
    public string m_sSpriteName;

    public PlayerInformation(string name, string spriteName)
    {
        m_sName = name;
        m_sSpriteName = spriteName;
    }

}
