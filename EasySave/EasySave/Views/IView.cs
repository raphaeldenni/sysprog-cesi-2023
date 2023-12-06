using EasySave.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Views;

 public  interface IView
{
    
        // Properties
        string Message { get; set; }
        LangType Lang { get; set; }

        // Methods
        void DisplayMessage();
    }

