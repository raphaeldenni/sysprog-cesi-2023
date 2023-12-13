using EasySave.ViewModels;

namespace EasySave
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            try 
            {
                var argsList = new List<string>(args);
                
                // Capitalize the first letter of the first argument to match the view model name.
                argsList[0] = char.ToUpper(argsList[0][0]) + argsList[0].Substring(1).ToLower();
                
                // Construct the view model type name from the first argument.
                var viewModelName = argsList[0] + "ViewModel";
                var viewModelType = Type.GetType("EasySave.ViewModels." + viewModelName) ?? throw new Exception();
                
                // Create parameters for the view model constructor.
                var viewModelParameters = new object[1];
                argsList.RemoveAt(0);
                viewModelParameters[0] = argsList.ToArray();
                
                // Launch the view model.
                _ = Activator.CreateInstance(viewModelType, viewModelParameters);
            } 
            catch (Exception)
            {
                _ = new HelpViewModel();
            }
        }
    }
}