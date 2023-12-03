using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Views;


public class DeleteView : IView
{
    // Properties from IView
    public string[] CommandArgs { get; set; }
    public string[] ResultsMessage { get; set; }
    public string ErrorMessage { get; set; }

    // Additional property specific to DeleteView
    public string DeleteMessage { get; set; }

    // Constructor
    public DeleteView(string[] args)
    {
        CommandArgs = args;
    }

    // Method
    public void DisplayDeleteMessage()
    {
        Console.WriteLine($"Delete Message: {DeleteMessage}");
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


