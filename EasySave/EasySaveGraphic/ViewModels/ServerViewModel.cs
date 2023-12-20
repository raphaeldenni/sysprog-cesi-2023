using EasySave.Models;

namespace EasySaveGraphic.ViewModels;

public class ServerViewModel
{
    private ServerModel ServerModel { get; } 
    
    /// <summary>
    /// ServerViewModel constructor
    /// </summary>
    public ServerViewModel()
    {
        ServerModel = new ServerModel();
        
        // Bind the HandleStringDataChanged method to the OnStringDataChanged event
        ServerModel.OnStringDataChanged += HandleStringDataChanged;
        
        StartListening();
    }
    
    /// <summary>
    /// Asynchronously listen for incoming connections.
    /// </summary>
    private async void StartListening()
    {
        await Task.Run(() =>
        {
            while (true)
            {
                ServerModel.ServerSocket.Listen(10);
        
                var clientSocket = ServerModel.AcceptConnection(ServerModel.ServerSocket);
                ServerModel.ClientListener(clientSocket);
            }
        });
    }
    
    /// <summary>
    /// Handle the string data received from the client and use it.
    /// </summary>
    /// <param name="stringData"></param>
    private static void HandleStringDataChanged(string? stringData)
    {
         Console.WriteLine(stringData);
    }
}