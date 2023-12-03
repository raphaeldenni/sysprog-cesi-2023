using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Views;

 public  interface IView
{
    
        // Properties
        string[] CommandArgs { get; set; }
        string[] ResultsMessage { get; set; }
        string ErrorMessage { get; set; }

        // Methods
        void CommandResult(string result);
        void CommandError(string error);
    }

