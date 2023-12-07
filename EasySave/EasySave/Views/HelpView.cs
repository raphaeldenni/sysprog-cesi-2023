using EasySave.Types;
using System;
using System.Reflection;

namespace EasySave.Views
{
    public class HelpView : IView
    {
        // Propriétés de l'interface IView
        public string? Message { get; set; }
        public LangType Lang { get; set; }

        // Constructeur
        public HelpView(LangType lang)
        {
            Lang = lang;
        }

        // Implémentation des méthodes de l'interface IView
        public void DisplayMessage()
        {
            Console.WriteLine(Message);
        }

        public void ErrorCommandName()
        {
            switch (Lang)
            {
                case LangType.En:
                    Message = $"Error: This help command doesn't exist!";
                    break;
                case LangType.Fr:
                    Message = $"Erreur : Cette commande d'aide n'existe pas !";
                    break;
            }
        }

        public void DisplayCreate()
        {
            switch (Lang)
            {
                case LangType.En:
                    Message = $"easysave create <taskName> <source> <destination> <type> : Creating a new task";
                    break;
                case LangType.Fr:
                    Message = $"easysave create <taskName> <source> <destination> <type> : Création d'une nouvelle tâche";
                    break;
            }
        }

        public void DisplayDelete()
        {
            switch (Lang)
            {
                case LangType.En:
                    Message = $"easysave delete <taskName> : Deleting a task";
                    break;
                case LangType.Fr:
                    Message = $"easysave delete <taskName> : Suppression d'une tâche";
                    break;
            }
        }

        public void DisplayList()
        {
            switch (Lang)
            {
                case LangType.En:
                    Message = $"easysave list : Listing all tasks";
                    break;
                case LangType.Fr:
                    Message = $"easysave list : Liste de toutes les tâches";
                    break;
            }
        }

        public void DisplayModify()
        {
            switch (Lang)
            {
                case LangType.En:
                    Message = $"easysave modify <taskName> [name|source|dest|type] <string> : Modifying a task";
                    break;
                case LangType.Fr:
                    Message = $"easysave modify <taskName> [name|source|dest|type] <string> : Modification d'une tâche";
                    break;
            }
        }

        public void DisplayConfig()
        {
            switch (Lang)
            {
                case LangType.En:
                    Message = $"easysave config [lang|logExtension] <string> : Configuring EasySave";
                    break;
                case LangType.Fr:
                    Message = $"easysave config [lang|logExtension] <string> : Configuration d'EasySave";
                    break;
            }
        }

        public void DisplayExecute()
        {
            switch (Lang)
            {
                case LangType.En:
                    Message = $"easysave execute <taskName> : Executing one task\n" +
                              $"easysave execute <taskIdOne>-<taskIdTwo> : Executing a task n to a task m\n" +
                              $"easysave execute <taskIdOne>;<taskIdTwo>;* : Executing several tasks";
                    break;
                case LangType.Fr:
                    Message = $"easysave execute <taskName> : Exécution d'une tâche\n" +
                              $"easysave execute <taskIdOne>-<taskIdTwo> : Exécution d'une tâche n à la tâche m\n" +
                              $"easysave execute <taskIdOne>;<taskIdTwo>;* : Exécution de plusieurs tâches";
                    break;
            }
        }

        public void DisplayAll()
        {
            MethodInfo[] methods = GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);

            foreach (var method in methods)
            {
                if (method.Name.StartsWith("Display") && method.Name != "DisplayAll" && method.Name != "DisplayMessage")
                {
                    method.Invoke(this, null);
                    DisplayMessage();
                }
            }
        }
    }
}
