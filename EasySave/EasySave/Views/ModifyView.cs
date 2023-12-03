using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Views
{
    internal interface ModifyView
    {
        public interface ModifyView : IView
        {
            // Properties
            public string ModifyMessage { get; set; }

            // Constructor
            public ModifyView(string[] args)
            {
                CommandArgs = args;
            }

            // Method
            public void DisplayModifyMessage()
            {
                Console.WriteLine($"Modify Message: {ModifyMessage}");
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

    }
}
