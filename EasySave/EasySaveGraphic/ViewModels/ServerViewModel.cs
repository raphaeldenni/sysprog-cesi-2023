using System.Net.Sockets;
using EasySave.Models;
using EasySaveGraphic.Views;

namespace EasySaveGraphic.ViewModels;

public class ServerViewModel
{
    // Resources properties
    private TaskModel TaskModel { get; }
    private HomeView HomeView { get; }
    private static List<TaskEntity>? TasksList { get; set; }
    
    // Server properties
    private ServerModel ServerModel { get; }
    private Socket? ClientSocket { get; set; }
    
    /// <summary>
    /// ServerViewModel constructor
    /// </summary>
    public ServerViewModel(HomeView homeView)
    {
        // Resources initialization
        HomeView = homeView;
        
        TaskModel = new TaskModel();
        TaskModel.PullStateFile();
        TasksList = TaskModel.TasksList;
        
        // Server initialization
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
        // Start listening for incoming connections in a task to avoid blocking the UI thread
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
        // If the string data is null, send a message to the client
        if (stringData == null)
        {
            ServerModel.DataSender(
                ClientSocket, 
                "Please type \"help\" to display the available commands."
                );
            
            return;
        }
        
        var args = stringData.Split(' ');
        
        // If the array of strings is empty, send a message to the client. Else, use the first string as a command
        // and the second string as a task name.
        switch (args[0])
        {
            case "help":
                ServerModel.DataSender(
                    ClientSocket, 
                    "Available commands:\n" +
                    "list: list the tasks\n" +
                    "pause <task-name>: pause a task\n" +
                    "resume <task-name>: resume a task\n" +
                    "stop <task-name>: stop a task\n" +
                    "exit: exit the server"
                );
                break;
               
            case "list":
                ListTasks();
                break;
            
            case "pause":
            case "resume":
            case "stop":
                ActionOnTask(args[0], args[1]);
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
        // If the tasks list is null, send a message to the client
        if (TasksList == null)
        {
            ServerModel.DataSender(ClientSocket, "No tasks.");
            return;
        }
        
        // Else, send the tasks list to the client
        var tasksString = TasksList.Aggregate(
            string.Empty, 
            (current, task) => current + $"Task {task.Id}: {task.Name} is {task.State}\n"
                );
        
        ServerModel.DataSender(ClientSocket, tasksString);
    }
    
    private void ActionOnTask(string action, string taskName)
    {
        var task = GetTaskByName(taskName);
        
        // If the task doesn't exist, send a message to the client
        if (task == null)
        {
            ServerModel.DataSender(ClientSocket, $"Task {taskName} not found.");
            return;
        }
        
        switch (action)
        {
            case "pause":
                HomeView.HomeViewModel.PauseTask(task);
                break;
            
            case "resume":
                HomeView.HomeViewModel.ResumeTask(task);
                break;
            
            case "stop":
                HomeView.HomeViewModel.StopTask(task);
                break;
        }
        
        ServerModel.DataSender(ClientSocket, $"On task {taskName}: {action}");
    }
    
    /// <summary>
    /// Get a task entity by its name.
    /// </summary>
    /// <param name="taskName"></param>
    /// <returns>TaskEntity</returns>
    private TaskEntity? GetTaskByName(string taskName)
    {
        return TasksList?.FirstOrDefault(task => task.Name == taskName);
    }
}