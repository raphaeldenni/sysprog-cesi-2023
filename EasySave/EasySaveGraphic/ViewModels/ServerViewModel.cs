using EasySave.Models;

namespace EasySaveGraphic.ViewModels;

public class ServerViewModel
{
    private ServerModel ServerModel { get; set; } 
    
    public ServerViewModel()
    {
        ServerModel = new ServerModel();
    }
}