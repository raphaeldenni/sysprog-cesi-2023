using EasySave.Types;

namespace EasySave.Views;


    public class CreateView : IView
    {
        // Properties from IView
        public string? Message { get; set; }
        public LangType Lang { get; set; }

        // Constructor
        public CreateView(LangType lang)
        {
            Lang = lang;
        }

        // Implementing IView interface methods
        public void DisplayMessage()
        {
            Console.WriteLine(Message);
        }

        public void ErrorBackupType(string validBackupTypes)
    {
        switch (Lang)
        {
            case LangType.En:
                Message = $"Error: Wrong backup type, use ({validBackupTypes})!";
                break;
            case LangType.Fr:
                Message = $"Erreur : Mauvais type de sauvegarde, utiliser ({validBackupTypes}) !";
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

    public void ErrorTooMuchTasks()
    {
        switch (Lang)
        {
            case LangType.En:
                Message = "Error : There are already five existing tasks !";
                break;
            case LangType.Fr:
                Message = "Erreur : Il y a déjà cinq tâches existantes !";
                break;
        }
    }

    public void SuccessfulCreation(string[] data)
    {
        switch (Lang)
        {
            case LangType.En:
                Message = $"Successful : Task {data[0]} named {data[1]} has been created.";
                break;
            case LangType.Fr:
                Message = $"Réussie : La tâche {data[0]} nommée {data[1]} a été créée.";
                break;
        }
    }
}

