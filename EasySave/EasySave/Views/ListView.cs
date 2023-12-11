using EasySave.Types;

namespace EasySave.Views;

internal class ListView : IView
{
    // Properties from IView
    public string? Message { get; set; }
    public LangType Lang { get; set; }

    // Constructor
    public ListView(LangType lang)
    {
        Lang = lang;
    }

    // Implementing IView interface methods
    public void DisplayMessage()
    {
        Console.WriteLine(Message);
    }

    public void ErrorNoTask()
    {
        switch (Lang)
        {
            case LangType.En:
                Message = $"Error :  No tasks have been created !";
                break;
            case LangType.Fr:
                Message = $"Erreur : Aucun travail de sauvegarde n'a été créé !";
                break;
        }
    }

    public void DisplayTask(int id, string name, StateType state, BackupType type)
    {
        switch (Lang)
        {
            case LangType.En:
                Message = $"ID: {id}, Name: {name}, State: {state}, Type: {type}";
                break;
            case LangType.Fr:
                Message = $"ID: {id}, Nom: {name}, Etat: {state}, Type: {type}";
                break;
        }
    }
}
