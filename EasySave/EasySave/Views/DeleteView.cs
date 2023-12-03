using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Views
{
    internal interface DeleteView
    {
        public interface DeleteView : IView
        {
            // Properties
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

    }
}
