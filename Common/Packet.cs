using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

public enum E_PacketType
{ 
    CONNECT,
    SPAWN_PLAYER,
    CLIENT_LIST,
    DISCONNECT,
    PLAYER_MOVED,
    PLAYER_ROTATED,
    PLAYER_SCALED
}

namespace Packets
{
    [Serializable]
    public abstract class Packet
    {
        private E_PacketType m_ePacketType;

        public E_PacketType Type
        {
            get { return m_ePacketType; }
            protected set { m_ePacketType = value; }
        }
    }

    [Serializable]
    public class ConnectPacket : Packet
    {
        public string m_IPEndPoint;
        public string m_sName;

        public ConnectPacket(string name, string endPoint)
        {
            m_IPEndPoint = endPoint;
            m_sName = name;
            Type = E_PacketType.CONNECT;
        }
    }

    [Serializable]
    public class PlayerConnectedPacket : Packet
    {
        public Guid m_iID;
        public string m_sName;

        public PlayerConnectedPacket(Guid id, string name)
        {
            m_iID = id;
            m_sName = name;
            Type = E_PacketType.SPAWN_PLAYER;
        }
    }

    [Serializable]
    public class ClientListPacket : Packet
    {
        public List<Guid> m_ClientGUIDs;

        public ClientListPacket(List<Guid> clientGUIDs)
        {
            m_ClientGUIDs = clientGUIDs;
            Type = E_PacketType.CLIENT_LIST;
        }
    }

    [Serializable]
    public class PlayerDisconnectedPacket : Packet
    {
        public Guid m_GUID;

        public PlayerDisconnectedPacket()
        {
            m_GUID = Guid.Empty;
            Type = E_PacketType.DISCONNECT;
        }
    }

    [Serializable]
    public class PlayerMovedPacket : Packet
    {
        public Guid m_GUID;
        public float m_fPosX;
        public float m_fPosY;

        public PlayerMovedPacket(float x, float y)
        {
            m_GUID = Guid.Empty;

            m_fPosX = x;
            m_fPosY = y;
            Type = E_PacketType.PLAYER_MOVED;
        }
    }

    [Serializable]
    public class PlayerRotatedPacket : Packet
    {
        public Guid m_GUID;
        public float m_fAngle;

        public PlayerRotatedPacket(float angle)
        {
            m_GUID = Guid.Empty;

            m_fAngle = angle;
            Type = E_PacketType.PLAYER_ROTATED;
        }
    }

    [Serializable]
    public class PlayerScaledPacket : Packet
    {
        public Guid m_GUID;
        public float m_fScaleX;
        public float m_fScaleY;

        public PlayerScaledPacket(float scaleX, float scaleY)
        {
            m_GUID = Guid.Empty;

            m_fScaleX = scaleX;
            m_fScaleY = scaleY;
            Type = E_PacketType.PLAYER_SCALED;
        }
    }
}