using EasySave.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
