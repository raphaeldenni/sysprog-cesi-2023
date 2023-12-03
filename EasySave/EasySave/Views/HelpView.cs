using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Views
{
    public class HelpView : IView
    {
        // Property helpview
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
    }
}