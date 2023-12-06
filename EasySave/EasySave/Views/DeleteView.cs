using EasySave.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Views;


public class DeleteView : IView
{
    // Properties from IView
    public string? Message { get; set; }
    public LangType Lang { get; set; }

    // Constructor
    public DeleteView(LangType lang)
    {
        Lang = lang;
    }

    // Implementing IView interface methods
    public void DisplayMessage()
    {
        Console.WriteLine(Message);
    }

    public void ErrorTaskNameNotFound()
    {
        switch (Lang)
        {
            case LangType.En:
                Message = $"Error : The task was not found !";
                break;
            case LangType.Fr:
                Message = $"Erreur : La tâche n'a pas été trouvé !";
                break;
        }
    }

    public void SuccessfulDelete(string name)
    {
        switch (Lang)
        {
            case LangType.En:
                Message = $"Successful : Task named {name} has been deleted.";
                break;
            case LangType.Fr:
                Message = $"Réussie : La tâche nommée {name} a été supprimée.";
                break;
        }
    }
}


