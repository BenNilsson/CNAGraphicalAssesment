using Packets;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

public class Client
{
    public Socket m_Socket;
    public NetworkStream m_Stream;

    public BinaryReader m_Reader;
    public BinaryWriter m_Writer;

    public IPEndPoint m_IPEndPoint;

    public string m_sUsername;
    public Guid m_GUID { get; private set; }

    private BinaryFormatter m_BinaryFormatter;
    private object m_ReadLock;
    private object m_WriteLock;

    public Client(Socket socket, Guid guid)
    {
        m_WriteLock = new object();
        m_ReadLock = new object();

        m_Socket = socket;
        m_GUID = guid;

        m_Stream = new NetworkStream(socket);

        m_Reader = new BinaryReader(m_Stream, Encoding.UTF8);
        m_Writer = new BinaryWriter(m_Stream, Encoding.UTF8);

        m_BinaryFormatter = new BinaryFormatter();
    }

    public void Close()
    {
        m_Stream.Close();
        m_Reader.Close();
        m_Writer.Close();
        m_Reader.Close();
    }

    public void TCP_SendPacket(Packet packet)
    {
        lock (m_WriteLock)
        {
            MemoryStream memoryStream = new MemoryStream();

            m_BinaryFormatter.Serialize(memoryStream, packet);

            byte[] buffer = memoryStream.GetBuffer();

            m_Writer.Write(buffer.Length);
            m_Writer.Write(buffer);
            m_Writer.Flush();

            Console.WriteLine($"[SENT] Sent {m_sUsername} Packet {packet.Type}");
        }
    }

    public Packet TCP_ReadPacket()
    {
        lock (m_ReadLock)
        {
            int numberOfBytes = -1;
            try
            {
                if ((numberOfBytes = m_Reader.ReadInt32()) != -1)
                {
                    byte[] buffer = m_Reader.ReadBytes(numberOfBytes);

                    MemoryStream memoryStream = new MemoryStream(buffer);

                    Packet packet = m_BinaryFormatter.Deserialize(memoryStream) as Packet;
                    return packet;
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("[ERROR] " + e.Message);
            }

            return null;
        }
    }
}
