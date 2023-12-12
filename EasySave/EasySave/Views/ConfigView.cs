using EasySave.Types;

namespace EasySave.Views;


internal class ConfigView : IView
{
    // Properties from IView
    public string? Message { get; set; }
    public LangType Lang { get; set; }

    // Constructor
    public ConfigView(LangType lang)
    {
        Lang = lang;
    }

    // Implementing IView interface methods
    public void DisplayMessage()
    {
        Console.WriteLine(Message);
    }

    public void ErrorLang(string validLangTypes)
    {
        switch (Lang)
        {
            case LangType.En:
                Message = $"Error : Wrong language, use ({validLangTypes}) !";
                break;
            case LangType.Fr:
                Message = $"Erreur : Mauvaise langue, utiliser ({validLangTypes}) !";
                break;
        }
    }

    public void ErrorLogExtension (string validLogTypes)
    {
        switch (Lang)
        {
            case LangType.En:
                Message = $"Error : Wrong extension, use ({validLogTypes})";
                break;
            case LangType.Fr:
                Message = $"Erreur : Mauvaise extension, utiliser ({validLogTypes}) !";
                break;
        }
    }

    public void SuccessfulKey()
    {
        switch (Lang)
        {
            case LangType.En:
                Message = $"Successful: The key has been updated";
                break;
            case LangType.Fr:
                Message = $"Réussie : La clef a ete mise a jour";
                break;
        }
    }

    public void SuccessfulExtensionsToEncrypt()
    {
        switch (Lang)
        {
            case LangType.En:
                Message = $"Successful: The list of extensions to encrypt has been updated";
                break;
            case LangType.Fr:
                Message = $"Réussie : La liste d'extensions a chiffre a ete mise a jour";
                break;
        }
    }

    public void SuccessfulLang(LangType langOut)
    {
        switch (langOut)
        {
            case LangType.En:
                Message = $"Successful: The application is now in english.";
                break;
            case LangType.Fr:
                Message = $"Réussie : L'application est maintenant en français.";
                break;
        }
    }

    public void SuccessfulLogExtension(LogType logExtension)
    {
        string logExtensionLowercase = logExtension.ToString().ToLower();

        switch (Lang)
        {
            case LangType.En:
                Message = $"Successful: Log file extension is now .{logExtensionLowercase}";
                break;
            case LangType.Fr:
                Message = $"Réussie : L'extension des fichiers log est maintenant .{logExtensionLowercase}";
                break;
        }
    }
}
