using System.Text;

using System.Net;
using System.Net.Sockets;

namespace EasySave.Models;

public class ServerModel
{
    private const string ServerIpAddress = "0.0.0.0";
    private const string ServerPort = "9000";
    
    public static string? StringData { get; set; }
    
    /// <summary>
    /// ServerModel constructor.
    /// </summary>
    public ServerModel()
    { 
        StringData = string.Empty;
        
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
        
        // Listen to incoming connections
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
            return newClientSocket;
        } catch (SocketException)
        {
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

            StringData = stringData;

            const string response = "Command received";
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
    }
}