using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Views;


    public class CreateView : IView
    {
        // Properties from IView
        public string[] CommandArgs { get; set; }
        public string[] ResultsMessage { get; set; }
        public string ErrorMessage { get; set; }

        // Additional property specific to CreateView
        public string CreateMessage { get; set; }

        // Constructor
        public CreateView(string[] args)
        {
            CommandArgs = args;
        }

        // Method specific to CreateView
        public void DisplayCreateMessage()
        {
            Console.WriteLine($"Create Message: {CreateMessage}");
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

