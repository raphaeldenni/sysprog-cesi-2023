using System.Net.Sockets;
using EasySave.Models;

namespace EasySaveGraphic.ViewModels;

public class ServerViewModel
{
    private ServerModel ServerModel { get; } 
    private Socket? ClientSocket { get; set; }
    
    /// <summary>
    /// ServerViewModel constructor
    /// </summary>
    public ServerViewModel()
    {
        ServerModel = new ServerModel();
        
        // Bind the HandleStringDataChange method to the OnStringDataChanged event
        ServerModel.OnStringDataChanged += HandleStringDataChange;
        
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
                
                ClientSocket = ServerModel.AcceptConnection(ServerModel.ServerSocket);
                ServerModel.DataReceiver(ClientSocket);
            }
        });
    }
    
    /// <summary>
    /// Handle the string data received from the client and use it.
    /// </summary>
    /// <param name="stringData"></param>
    private void HandleStringDataChange(string? stringData)
    {
        if (stringData == null)
        {
            ServerModel.DataSender(
                ClientSocket, 
                "Please type \"help\" to display the available commands."
                );
            
            return;
        }
        
        var args = stringData.Split(' ');
        
        switch (args[0])
        {
            case "help":
                ServerModel.DataSender(
                    ClientSocket, 
                    "Available commands: help, list, pause, resume, stop"
                    );
                break;
                
            case "list":
                ListTasks();
                break;
            
            case "pause":
                break;
            
            case "resume":
                break;
            
            case "stop":
                break;
            
            case "exit":
                break;
            
            default:
                ServerModel.DataSender(
                    ClientSocket, 
                    $"Unknown command \"{args[0]}\", type \"help\" to display the available commands."
                    );
                break;
        }
    }
    
    /// <summary>
    /// List the tasks.
    /// </summary>
    private void ListTasks()
    {
        var taskModel = new TaskModel();
        taskModel.PullStateFile();
        
        var tasks = taskModel.TasksList;

        if (tasks == null)
        {
            ServerModel.DataSender(ClientSocket, "No tasks.");
            return;
        }
        
        var tasksString = tasks.Aggregate(
            string.Empty, 
            (current, task) => current + $"Task {task.Id}: {task.Name} is {task.State}\n"
                );
        
        ServerModel.DataSender(ClientSocket, tasksString);
    }
}