using EasySave.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Views;



public class ModifyView : IView
{
    // Properties from IView
    public string? Message { get; set; }
    public LangType Lang { get; set; }

    // Constructor
    public ModifyView(LangType lang)
    {
        Lang = lang;
    }

    // Implementing IView interface methods
    public void DisplayMessage()
    {
        Console.WriteLine(Message);
    }

    public void ErrorBackupType()
    {
        switch (Lang)
        {
            case LangType.En:
                Message = "Error : Wrong backup type, use (Complete|Differential) !";
                break;
            case LangType.Fr:
                Message = "Erreur : Mauvais type de sauvegarde, utiliser (Complete|Differential) !";
                break;
        }
    }

    public void ErrorSourcePathNotFound()
    {
        switch (Lang)
        {
            case LangType.En:
                Message = "Error : Source path not found !";
                break;
            case LangType.Fr:
                Message = "Erreur : Le dossier source n'a pas été trouvé !";
                break;
        }
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

    public void ErrorDuplicateTaskName()
    {
        switch (Lang)
        {
            case LangType.En:
                Message = "Error : This task name is already taken !";
                break;
            case LangType.Fr:
                Message = "Erreur : Ce nom de tâche est déjà utilisé !";
                break;
        }
    }

    public void SuccessfulModify(string[] data)
    {
        switch (Lang)
        {
            case LangType.En:
                Message = $"Successful : Task {data[0]} named {data[1]} has been modify.";
                break;
            case LangType.Fr:
                Message = $"Réussie : La tâche {data[0]} nommée {data[1]} a été modifiée.";
                break;
        }
    }
}


