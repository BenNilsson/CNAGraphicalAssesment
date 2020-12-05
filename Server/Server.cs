﻿using Packets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace Server
{
    public class Server
    {
        private ConcurrentDictionary<int, Client> m_Clients;

        private TcpListener m_TcpListener;
        private UdpClient m_UdpListener;

        public Server(string ipAddress, int port)
        {
            IPAddress ip = IPAddress.Parse(ipAddress);

            m_TcpListener = new TcpListener(ip, port);

            m_UdpListener = new UdpClient(port);
            Thread udpThread = new Thread(() =>
            {
                UDP_Listen();
            });
            udpThread.Start();
        }


        public void StartServer()
        {
            Console.WriteLine("[SERVER] Started The Server");
            m_Clients = new ConcurrentDictionary<int, Client>();
            m_TcpListener.Start();

            int clientIndex = 0;

            while (true)
            {
                int index = clientIndex;
                clientIndex++;

                Socket socket = m_TcpListener.AcceptSocket();
                Client client = new Client(socket, Guid.NewGuid());
                m_Clients.TryAdd(index, client);
                Thread tcpThread = new Thread(() => { TCP_ClientMethod(index); });
                tcpThread.Start();
            }
        }

        public void StopServer()
        {
            m_TcpListener.Stop();
        }

        private void UDP_Listen()
        {
            while (true)
            {
                try
                {
                    IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] buffer = m_UdpListener.Receive(ref ipEndPoint);

                    MemoryStream memoryStream = new MemoryStream(buffer);

                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    Packet packet = binaryFormatter.Deserialize(memoryStream) as Packet;

                    foreach (Client client in m_Clients.Values)
                    {
                        if (client.m_IPEndPoint != null)
                        {
                            if (ipEndPoint.ToString() == client.m_IPEndPoint.ToString())
                            {
                                switch (packet.Type)
                                {
                                    case E_PacketType.PLAYER_MOVED:
                                        PlayerMovedPacket playerMovedPacket = packet as PlayerMovedPacket;
                                        playerMovedPacket.m_GUID = client.m_GUID;

                                        MemoryStream stream = new MemoryStream();
                                        binaryFormatter.Serialize(stream, playerMovedPacket);
                                        byte[] playerMovedBuffer = stream.GetBuffer();

                                        foreach (Client c in m_Clients.Values)
                                        {
                                            if (c != client && c.m_IPEndPoint != null)
                                            {
                                                // Send the packet received to everyone else
                                                m_UdpListener.Send(playerMovedBuffer, playerMovedBuffer.Length, c.m_IPEndPoint);
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine($"[ERROR] Client UDP Read Method Exception: {e.Message}");
                }
            }
            
        }

        private void TCP_ClientMethod(int index)
        {
            try
            {
                Packet receivedPacket = null;
                bool quit = false;

                Console.WriteLine("[SERVER] User Connected");

                while ((receivedPacket = m_Clients[index].TCP_ReadPacket()) != null && !quit)
                {
                    switch (receivedPacket.Type)
                    {

                        /// ------------------ PLAYER CONNECTED
                        case E_PacketType.CONNECT:
                            ConnectPacket connectPacket = (ConnectPacket)receivedPacket;
                            m_Clients[index].m_IPEndPoint = IPEndPoint.Parse(connectPacket.m_IPEndPoint);
                            m_Clients[index].m_sUsername = connectPacket.m_sName;

                            PlayerConnectedPacket playerConnectedPacket = new PlayerConnectedPacket(m_Clients[index].m_GUID, connectPacket.m_sName);
                            List<Guid> guids = new List<Guid>();
                            foreach (Client c in m_Clients.Values)
                            {
                                if (c != m_Clients[index])
                                {
                                    // Tell everyone else that the client has connected
                                    c.TCP_SendPacket(playerConnectedPacket);
                                    guids.Add(c.m_GUID);
                                }
                            }
                            // Send newly connected client a list of all current clients
                            ClientListPacket clientListPacket = new ClientListPacket(guids);
                            m_Clients[index].TCP_SendPacket(clientListPacket);
                            break;

                        /// ------------------ PLAYER DISCONNECTED
                        case E_PacketType.DISCONNECT:
                            PlayerDisconnectedPacket disconnectedPacket = (PlayerDisconnectedPacket)receivedPacket;
                            disconnectedPacket.m_GUID = m_Clients[index].m_GUID;
                            foreach (Client c in m_Clients.Values)
                            {
                                if (c != m_Clients[index])
                                {
                                    // Tell everyone else that the client has disconnected
                                    c.TCP_SendPacket(disconnectedPacket);
                                }
                            }
                            quit = true;
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (EndOfStreamException e)
            {
                Console.WriteLine("[SERVER] User disconnected : " + e.Message);
            }
            finally
            {
                m_Clients[index].Close();
                Client c;
                m_Clients.TryRemove(index, out c);
            }
        }
    }
}