using System.Text;

using System.Net;
using System.Net.Sockets;

namespace EasySave.Models;

public class ServerModel
{
    private const string ServerIpAddress = "0.0.0.0";
    private const string ServerPort = "9000";
    
    public Socket ServerSocket { get; set; }
    
    private static string? _stringData;

    public static string? StringData
    {
        get => _stringData;
        set
        {
            _stringData = value;
            OnStringDataChanged?.Invoke(_stringData);
        }
    }
    
    public static event Action<string?>? OnStringDataChanged;

    /// <summary>
    /// ServerModel constructor.
    /// </summary>
    public ServerModel()
    { 
        StringData = string.Empty;
        
        var serverIpAddress = IPAddress.Parse(ServerIpAddress);
        var serverPort = int.Parse(ServerPort);

        ServerSocket = EstablishServer(serverIpAddress, serverPort);
    }
    
    /// <summary>
    /// Create a new socket and bind it to the server IP address and port of the server.
    /// </summary>
    /// <param name="serverIpAddress"></param>
    /// <param name="serverPort"></param>
    /// <returns>Socket</returns>
    private static Socket EstablishServer(IPAddress serverIpAddress, int serverPort)
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
        
        return newServerSocket;
    }
    
    /// <summary>
    /// Accept a connection from a client.
    /// </summary>
    /// <param name="serverSocket"></param>
    /// <returns>Socket</returns>
    public static Socket? AcceptConnection(Socket serverSocket)
    {
        try
        {
            var newClientSocket = serverSocket.Accept();
            return newClientSocket;
        } catch (SocketException)
        {
            return null;
        }
    }
    
    /// <summary>
    /// Listen to the network and print received messages and send a response.
    /// </summary>
    /// <param name="clientSocket"></param>
    public static void ClientListener(Socket? clientSocket)
    {
        if (clientSocket == null) return;
        
        var stringData = string.Empty;
        
        try
        {
            while (clientSocket is { Connected: true } && stringData != "exit()")
            {
                var data = new byte[1024];
        
                var dataSize = clientSocket.Receive(data);
                stringData = Encoding.UTF8.GetString(data, 0, dataSize);

                StringData = stringData;

                const string response = "Command received";
                var byteResponse = Encoding.UTF8.GetBytes(response);

                clientSocket.Send(byteResponse);
            }
        } catch (SocketException)
        {
            return;
        }
        
        clientSocket.Shutdown(SocketShutdown.Both);
        clientSocket.Close();
    }
}