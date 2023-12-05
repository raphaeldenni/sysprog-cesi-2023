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

        percentage = Math.Max(0, Math.Min(100, percentage));

        Console.Clear();

        Message = ("Loading: [");
        int progressChars = (int)(percentage / 2.0);
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