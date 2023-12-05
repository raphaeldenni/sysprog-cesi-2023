using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Views;

public class ExecuteView : IView
{
    // Properties from IView
    public string? Message { get; set; }

    // Constructor
    public ExecuteView()
    {
    }

    // Implementing IView interface methods
    public void DisplayMessage()
    {
        Console.WriteLine(Message);
    }
    public void DisplayLoading(int percentage)
    {
        // Ensure the percentage is within the valid range [0, 100]
        percentage = Math.Max(0, Math.Min(100, percentage));

        Console.Clear(); // Clear the console

        Message = ("Loading: [");
        int progressChars = (int)(percentage / 2.0); // Calculate the number of '=' characters to display
        for (int i = 0; i < progressChars; i++)
        {
            Message += ("=");
        }
        for (int i = progressChars; i < 50; i++)
        {
            Message += (" ");
        }
        Message += ($"] {percentage}%\r");

        DisplayMessage();
    }
}