using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Views;

public class ListView : IView
{
    // Properties from IView
    public string[] CommandArgs { get; set; }
    public string[] ResultsMessage { get; set; }
    public string ErrorMessage { get; set; }

    // Additional property specific to ListView
    public string[] TaskList { get; set; }

    // Constructor
    public ListView(string[] args)
    {
        CommandArgs = args;
    }

    // Method
    public void DisplayTaskList()
    {
        Console.WriteLine("Task List:");
        foreach (var task in TaskList)
        {
            Console.WriteLine(task);
        }
    }

    // Implementing IView interface methods
    public void CommandResult(string success)
    {
        Console.WriteLine($"Command Result: {success}");
    }

    public void CommandError(string error)
    {
        Console.WriteLine($"Command Error: {error}");
    }
}
