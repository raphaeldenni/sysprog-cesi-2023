using EasySave.Types;

namespace EasySave.Views;


public class ConfigView : IView
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

    public void ErrorLang()
    {
        switch (Lang)
        {
            case LangType.En:
                Message = "Error : Wrong language, use (En|Fr) !";
                break;
            case LangType.Fr:
                Message = "Erreur : Mauvaise langue, utiliser (En|Fr) !";
                break;
        }
    }

    public void ErrorLogExtension ()
    {
        switch (Lang)
        {
            case LangType.En:
                Message = "Error : Wrong extension, use (Json|Xml)";
                break;
            case LangType.Fr:
                Message = "Erreur : Mauvaise extension, utiliser (Json|Xml) !";
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
        switch (Lang)
        {
            case LangType.En:
                Message = $"Successful: Log file extension is now . {logExtension}.";
                break;
            case LangType.Fr:
                Message = $"Réussie : L'extension des fichiers log est maintenant .{logExtension}.";
                break;
        }
    }
}
