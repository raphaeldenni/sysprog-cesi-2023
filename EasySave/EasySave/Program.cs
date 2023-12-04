using EasySave.ViewModels;

namespace EasySave
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            try 
            {
                List<string> listArgs = new List<string>(args);

                for (int i = 0; i < listArgs.Count; i++)
                {
                    listArgs[i] = char.ToUpper(listArgs[i][0]) + listArgs[i].Substring(1).ToLower();
                }

                dynamic viewModel = null;
                string viewModelName = listArgs[0] + "ViewModel";
                Type viewModelType = Type.GetType("EasySave.ViewModels." + viewModelName);

                if (viewModelType == null)
                {
                    viewModel = new HelpViewModel();
                    return;
                }

                var parameters = new object[1];
                listArgs.RemoveAt(0);
                parameters[0] = listArgs.ToArray();

                viewModel = Activator.CreateInstance(viewModelType, parameters);
                viewModel = null;
            } 
            catch (Exception ex)
            {
                dynamic viewModel = new HelpViewModel();
            }
        }
    }
}