﻿using CNAGraphicalApplication;
using Microsoft.Xna.Framework;
using Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

public class Client
{
    private TcpClient m_TcpClient;
    private UdpClient m_UdpClient;

    private NetworkStream m_NetworkStream;

    private BinaryWriter m_Writer;
    private BinaryReader m_Reader;
    private BinaryFormatter m_BinaryFormatter;

    public Dictionary<Guid, GameObject> m_ConnectedClients;

    public Client()
    {
        m_TcpClient = new TcpClient();
    }

    public bool Connect(string ip, int port)
    {
        try
        {
            m_TcpClient.Connect(ip, port);

            m_UdpClient = new UdpClient();
            m_UdpClient.Connect(ip, port);

            m_ConnectedClients = new Dictionary<Guid, GameObject>();

            m_NetworkStream = m_TcpClient.GetStream();
            m_Writer = new BinaryWriter(m_NetworkStream, Encoding.UTF8);
            m_Reader = new BinaryReader(m_NetworkStream, Encoding.UTF8);
            m_BinaryFormatter = new BinaryFormatter();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
            return false;
        }
    }

    public void Run()
    {
        Thread tcpThread = new Thread(() =>
        {
            TCP_ProcessServerResponse();
        });
        tcpThread.Start();

        Thread udpThread = new Thread(() =>
        {
            UDP_ProcessServerResponse();
        });
        udpThread.Start();

        UDP_Connect();
    }

    public void Disconnect()
    {
        m_UdpClient.Close();
        m_TcpClient.Close();
    }

    public void TCP_SendPacket(Packet packet)
    {
        MemoryStream memoryStream = new MemoryStream();

        m_BinaryFormatter.Serialize(memoryStream, packet);

        byte[] buffer = memoryStream.GetBuffer();

        m_Writer.Write(buffer.Length);
        m_Writer.Write(buffer);
        m_Writer.Flush();
    }

    public void UDP_SendPacket(Packet packet)
    {
        MemoryStream memoryStream = new MemoryStream();

        m_BinaryFormatter.Serialize(memoryStream, packet);

        byte[] buffer = memoryStream.GetBuffer();

        m_UdpClient.Send(buffer, buffer.Length);
    }

    public void UDP_Connect()
    {
        IPEndPoint endPoint = (IPEndPoint)m_UdpClient.Client.LocalEndPoint;
        ConnectPacket connectPacket = new ConnectPacket("Player", endPoint.ToString());

        TCP_SendPacket(connectPacket);
    }


    private void TCP_ProcessServerResponse()
    {
        int numberOfBytes = -1;
        try
        {
            while ((numberOfBytes = m_Reader.ReadInt32()) != -1)
            {
                byte[] buffer = m_Reader.ReadBytes(numberOfBytes);

                MemoryStream memoryStream = new MemoryStream(buffer);
                Packet packet = m_BinaryFormatter.Deserialize(memoryStream) as Packet;

                switch (packet.Type)
                {
                    /// ------------------ NEW PLAYER CONNECTED
                    case E_PacketType.SPAWN_PLAYER:
                        PlayerConnectedPacket playerConnectedPacket = packet as PlayerConnectedPacket;
                        GameObject newPlayer = new GameObject("SlimeSpriteSheet", new Vector2(Constants.ScreenDimensions.X * 0.5f, Constants.ScreenDimensions.Y));
                        newPlayer.LoadContent();
                        SceneManager.Instance.m_CurrentScene.m_GameObjects.Add(newPlayer);
                        m_ConnectedClients.Add(playerConnectedPacket.m_iID, newPlayer);
                        break;

                    /// ------------------ PLAYER DISCONNECT
                    case E_PacketType.DISCONNECT:
                        PlayerDisconnectedPacket playerDisconnectedPacket = packet as PlayerDisconnectedPacket;
                        SceneManager.Instance.m_CurrentScene.m_GameObjects.Remove(m_ConnectedClients[playerDisconnectedPacket.m_GUID]);
                        m_ConnectedClients.Remove(playerDisconnectedPacket.m_GUID);
                        break;
                    /// ------------------ RECEIVED CLIENT LIST (You connected)
                    case E_PacketType.CLIENT_LIST:
                        ClientListPacket clientListPacket = packet as ClientListPacket;
                        foreach (Guid id in clientListPacket.m_ClientGUIDs)
                        {
                            GameObject connectedPlayer = new GameObject("SlimeSpriteSheet", new Vector2(Constants.ScreenDimensions.X * 0.5f, Constants.ScreenDimensions.Y));
                            connectedPlayer.LoadContent();
                            SceneManager.Instance.m_CurrentScene.m_GameObjects.Add(connectedPlayer);
                            m_ConnectedClients.Add(id, connectedPlayer);
                        }
                        break;
                    default:
                        break;
                }

            }
        }
        catch (IOException e)
        {
            Console.WriteLine("User Disconnected: " + e.Message);
        }
    }

    private void UDP_ProcessServerResponse()
    {
        try
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                byte[] buffer = m_UdpClient.Receive(ref ipEndPoint);

                MemoryStream memoryStream = new MemoryStream(buffer);

                Packet packet = m_BinaryFormatter.Deserialize(memoryStream) as Packet;

                switch (packet.Type)
                {
                    case E_PacketType.PLAYER_MOVED:
                        PlayerMovedPacket playerMovedPacket = packet as PlayerMovedPacket;
                        Vector2 movement = new Vector2(playerMovedPacket.m_fPosX, playerMovedPacket.m_fPosY);
                        if (m_ConnectedClients.ContainsKey(playerMovedPacket.m_GUID))
                            m_ConnectedClients[playerMovedPacket.m_GUID].Transform.m_Position = movement;
                        break;
                    default:
                        break;
                }
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine("User Disconnected: " + e.Message);
        }
    }
}