using System.Net;
using System.Net.Sockets;
using System.Text;
using NeonGateWeb.Utils;

namespace NeonGateWeb.Server.UDP;

public static class Socket
{
    public static void Receive()
    {
        // create a new UdpClient that will listen on port 1234
        var udpClient = new UdpClient(8081);

        // receive a UDP datagram
        var remoteEndpoint = new IPEndPoint(IPAddress.Any, 0); // remote endpoint is unknown
        var datagram = udpClient.Receive(ref remoteEndpoint);
        var byteArray = datagram.Convert();
        
        // reads head, prepares for decoding
        byteArray.ReadHead();
        
        // set the packet id as a top level enumerator
        // byteArray.PacketId
        
        // interpret the head of the datagram:
        // delDict[byteArray.packetId].Invoke(byteArray.Extract());
        // create attribute classes to handle this, allow user defined methods to be called for datagram handling

        // todo: datagram reading
        // create an implementation to handle data transfer 
        // responses handled through delegates
        // Invoke the respective delegate on client to interpret data
        
        //example:
        // delegate = Send;
        // foreach (var receiver in GetReceivers(bytes))
        // delegate.Invoke(receiver, bytes);

        // interpret the datagram
        var message = Encoding.UTF8.GetString(datagram);
        Console.WriteLine("Received message: " + message);
    }

    public static void Send()
    {
        // Create a UDP client and specify the remote endpoint to send the datagram to
        var client = new UdpClient();
        var remoteEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8081);

        // Convert the message to bytes
        const string MESSAGE = "Hello, world!";
        var bytes = new ByteArray();
        bytes.WriteString(MESSAGE);
        // todo validate the buffer array is the right bytes to send
        // todo, write the head of the packet
        // Send the datagram
        client.Send(bytes.BufferArray, bytes.BufferArray.Length, remoteEndpoint);
    }
}