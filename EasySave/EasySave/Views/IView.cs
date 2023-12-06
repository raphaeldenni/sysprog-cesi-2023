using EasySave.Types;

namespace EasySave.Views;

 public  interface IView
{
    
        // Properties
        string Message { get; set; }
        LangType Lang { get; set; }

        // Methods
        void DisplayMessage();
    }

