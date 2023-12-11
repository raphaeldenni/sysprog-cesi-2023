namespace CryptoSoft;

public class CryptoSoftViewModel
{
    // View and Model
    private ConfigModel ConfigModel { get; }
    private CryptoSoftModel? CryptoSoftModel { get; }
    private CryptoSoftView CryptoSoftView { get; }
    
    // Properties
    private string? Lang { get; }
    private string? Key { get; }
    private string[]? ExtensionsToEncrypt { get; }
    private string? SourcePath { get; }
    private string? DestPath { get; }
    
    // Constructor

    /// <summary>
    /// CryptoSoftViewModel constructor
    /// </summary>
    /// <param name="args"></param>
    public CryptoSoftViewModel(IReadOnlyList<string> args)
    {
        ConfigModel = new ConfigModel();
        
        // Check if the config file is valid.   
        if (ConfigModel.Config?.Key == null)
        {
            CryptoSoftView = new CryptoSoftView("en");
            
            CryptoSoftView.SetMessage("ConfigFileIsInvalid");
            CryptoSoftView.DisplayMessage();
            
            return;
        }
        
        // Set the properties according to the config file.
        Lang = ConfigModel.Config.Language.ToString().ToLower();
        Key = ConfigModel.Config.Key;
        ExtensionsToEncrypt = ConfigModel.Config.ExtensionsToEncrypt ?? new []{""};
        
        // Set the properties according to the arguments, if any.
        if (args.Count is > 0 and < 2)
        {
            SourcePath = args[0];
            DestPath = args[1];
        }
        else
        {
            SourcePath = DestPath = string.Empty;
        }
        
        CryptoSoftModel = new CryptoSoftModel(SourcePath, DestPath, Key, ExtensionsToEncrypt);
        CryptoSoftView = new CryptoSoftView(Lang);

        try
        {
            CryptoSoftModel.CreateEncryptedFile();
        }
        catch (Exception e)
        {
            var exceptionName = e.GetType().Name.Split("+").Last();
            
            CryptoSoftView.SetMessage(exceptionName, SourcePath);
            CryptoSoftView.DisplayMessage();
            
            return;
        }
        
        CryptoSoftView.SetMessage("FileEncrypted", DestPath);
        CryptoSoftView.DisplayMessage();
    }
}