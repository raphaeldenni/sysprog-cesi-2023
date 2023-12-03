using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Views;

public class HelpView : IView
{
    // Properties from IView
    public string[] CommandArgs { get; set; }
    public string[] ResultsMessage { get; set; }
    public string ErrorMessage { get; set; }

    // Additional property specific to HelpView
    public string HelpMessage { get; set; }

    // Constructor
    public HelpView(string[] args)
    {
        CommandArgs = args;
    }

    // Method HelpView
    public void DisplayHelpMessage()
    {
        Console.WriteLine($"Help Message: {HelpMessage}");
    }

    // Implementing IView interface methods
    public void CommandResult(string message)
    {
        Console.WriteLine($"Command Result: {message}");
    }

    public void CommandError(string message)
    {
        Console.WriteLine($"Command Error: {message}");
    }
}