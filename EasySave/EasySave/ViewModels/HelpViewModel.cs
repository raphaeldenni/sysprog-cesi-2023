using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySave.Views;

namespace EasySave.ViewModels
{
    public class HelpViewModel
    {
        public HelpView HelpView { get; set; }

        public HelpViewModel()
        {
            HelpView = new HelpView();
            HelpView.DisplayAll();
        }

        public HelpViewModel(string[] args)
        {
            HelpView = new HelpView();

            string methodName = "Display" + args[0];

            var method = typeof(HelpView).GetMethod(methodName);

            if (method != null)
            {
                method.Invoke(HelpView, null);
                HelpView.DisplayMessage();
            }
            else
            {
                HelpView.Message = "Vous avez renseigné une commande non existante";
                HelpView.DisplayMessage();
            }
        }
    }
}
