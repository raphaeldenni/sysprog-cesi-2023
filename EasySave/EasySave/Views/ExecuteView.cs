using EasySave.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Views;

public class ExecuteView : IView
{
    // Properties from IView
    public string? Message { get; set; }
    public LangType Lang { get; set; }

    // Constructor
    public ExecuteView(LangType lang)
    {
        Lang = lang;
    }

    // Implementing IView interface methods
    public void DisplayMessage()
    {
        Console.WriteLine(Message);
    }

    public void DisplayLoading(int percentage)
    {

        percentage = Math.Max(0, Math.Min(100, percentage));

        Console.Clear();

        if (Lang == LangType.En)
        {
            Message = ("Loading: [");
        }
        else if (Lang == LangType.Fr)
        {
            Message = ("Chargement: [");
        }

        int progressChars = (int)(percentage / 2.0);
        for (int i = 0; i < progressChars; i++)
        {
            Message += ("=");
        }
        for (int i = progressChars; i < 50; i++)
        {
            Message += (" ");
        }
        Message += ($"] {percentage}%\r");

        DisplayMessage();
    }

    public void TasksState(string name, bool state)//State true = started 
    {
        switch (Lang)
        {
            case LangType.En:
                Message = $"Successful : Task named {name} is {(!state ? "complete" : "started")}.";
                break;
            case LangType.Fr:
                Message = $"Réussie : La tâche nommée {name} est {(!state ? "terminée" : "lancée")}.";
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
}