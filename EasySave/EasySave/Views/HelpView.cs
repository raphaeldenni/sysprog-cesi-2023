using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Views;

public class HelpView : IView
{
    // Properties from IView
    public string? Message { get; set; }


    // Constructor
    public HelpView()
    {
    }

    // Implementing IView interface methods
    public void DisplayMessage()
    {
        Console.WriteLine(Message);
    }

    public void DisplayCreate()
    {
        Message = @"easysave create <name> <source> <destination> <type>";
    }

    public void DisplayDelete()
    {
        Message = @"easysave delete <name>";
    }

    public void DisplayList()
    {
        Message = @"easysave list";
    }

    public void DisplayModify()
    {
        Message = @"easysave modify <name> [name|source|dest|type] <string>";
    }

    public void DisplayExecute() 
    {
        Message = @"easysave execute <name|number>";
    }

    public void DisplayAll()
    {
        MethodInfo[] methods = GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);

        foreach (var method in methods)
        {
            if (method.Name.StartsWith("Display") && method.Name != "DisplayAll" && method.Name != "DisplayMessage")
            {
                method.Invoke(this, null);
                DisplayMessage();
            }
        }
    }

}