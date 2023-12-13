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

                listArgs[0] = char.ToUpper(listArgs[0][0]) + listArgs[0].Substring(1).ToLower();

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