using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Views;

public class ListView : IView
{
    // Properties from IView
    public string? Message { get; set; }

    // Constructor
    public ListView()
    {
    }

    // Implementing IView interface methods
    public void DisplayMessage()
    {
        Console.WriteLine(Message);
    }
}
