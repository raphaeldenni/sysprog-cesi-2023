using System.Text;

using System.Net;
using System.Net.Sockets;

namespace EasySave.Models;

public class ServerModel
{
    private const string ServerIpAddress = "0.0.0.0";
    private const string ServerPort = "9000";
    
    /// <summary>
    /// ServerModel constructor.
    /// </summary>
    public ServerModel()
    {
        Console.InputEncoding = Console.OutputEncoding = Encoding.UTF8;
        
        var serverIpAddress = IPAddress.Parse(ServerIpAddress);
        var serverPort = int.Parse(ServerPort);

        var serverSocket = EstablishConnection(serverIpAddress, serverPort);
        var clientSocket = AcceptConnection(serverSocket);

        NetworkListener(clientSocket);
        TerminateConnection(serverSocket);
    }
    
    /// <summary>
    /// Create a new socket and bind it to the server IP address and port of the server.
    /// </summary>
    /// <param name="serverIpAddress"></param>
    /// <param name="serverPort"></param>
    /// <returns>Socket</returns>
    private static Socket EstablishConnection(IPAddress serverIpAddress, int serverPort)
    {
        var newServerSocket = new Socket(
            AddressFamily.InterNetwork, 
            SocketType.Stream, 
            ProtocolType.Tcp
            );
        
        var localEndPoint = new IPEndPoint(
            serverIpAddress, 
            serverPort
            );
        
        newServerSocket.Bind(localEndPoint);
        
        Console.WriteLine("Waiting for connection...");
        newServerSocket.Listen(10);

        return newServerSocket;
    }
    
    /// <summary>
    /// Accept a connection from a client.
    /// </summary>
    /// <param name="serverSocket"></param>
    /// <returns>Socket</returns>
    private static Socket AcceptConnection(Socket serverSocket)
    {
        try
        {
            var newClientSocket = serverSocket.Accept();
            var clientEndPoint = (IPEndPoint)newClientSocket.RemoteEndPoint!;
        
            Console.WriteLine(
                "Connection established with IP: {0} on port: {1}", 
                clientEndPoint.Address,
                clientEndPoint.Port
            );
        
            return newClientSocket;
        } catch (SocketException e)
        {
            Console.WriteLine("Connection failed : {0}", e.Message);
            Environment.Exit(1);
            return null;
        }
    }
    
    /// <summary>
    /// Listen to the network and print received messages and send a response.
    /// </summary>
    /// <param name="clientSocket"></param>
    private static void NetworkListener(Socket clientSocket)
    {
        var stringData = string.Empty;
        
        while (stringData != "exit()")
        {
            var data = new byte[1024];
        
            var dataSize = clientSocket.Receive(data);
            stringData = Encoding.UTF8.GetString(data, 0, dataSize);
        
            Console.WriteLine("Data received: {0}", stringData);

            var response = "Text received";
            var byteResponse = Encoding.UTF8.GetBytes(response);

            clientSocket.Send(byteResponse);
        }
    }
    
    /// <summary>
    /// Terminate the connection between the server and the client.
    /// </summary>
    /// <param name="serverSocket"></param>
    private static void TerminateConnection(Socket serverSocket)
    {
        serverSocket.Shutdown(SocketShutdown.Both);
        serverSocket.Close();
        
        Console.WriteLine("Connection terminated.");
    }
}