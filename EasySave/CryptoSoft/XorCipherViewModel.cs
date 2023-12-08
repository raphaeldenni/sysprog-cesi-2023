namespace CryptoSoft;

public class XorCipherViewModel
{
    // View and Model
    private ConfigModel ConfigModel { get; }
    private XorCipherModel XorCipherModel { get; }
    private XorCipherView XorCipherView { get; }
    
    // Properties
    private string? Lang { get; }
    private string? FilePath { get; }
    private string? Key { get; }
    
    // Constructor

    /// <summary>
    /// XorCipherViewModel constructor
    /// </summary>
    /// <param name="args"></param>
    public XorCipherViewModel(string[] args)
    {
        ConfigModel = new ConfigModel();
        
        Lang = ConfigModel.Config!.Lang.ToString().ToLower();
        FilePath = args.Length != 0 ? args[0] : string.Empty;
        Key = ConfigModel.Config.Key!;
        
        XorCipherModel = new XorCipherModel(FilePath, Key);
        XorCipherView = new XorCipherView(Lang);

        try
        {
            XorCipherModel.EncryptFile();
        }
        catch (Exception e)
        {
            var exceptionName = e.GetType().Name.Split("+").Last();
            
            XorCipherView.SetMessage(exceptionName, FilePath);
            XorCipherView.DisplayMessage();
            return;
        }
        
        XorCipherView.SetMessage("FileEncrypted", FilePath);
        XorCipherView.DisplayMessage();
    }
}