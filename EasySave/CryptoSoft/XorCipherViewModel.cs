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
    /// <param name="filePath"></param>
    public XorCipherViewModel(string filePath)
    {
        ConfigModel = new ConfigModel();
        
        Lang = ConfigModel.Config!.Lang.ToString().ToLower();
        FilePath = filePath;
        Key = ConfigModel.Config.Key!;
        
        XorCipherModel = new XorCipherModel(FilePath, Key);
        XorCipherView = new XorCipherView(Lang);

        try
        {
            XorCipherModel.EncryptFile();
        }
        catch (Exception e)
        {
            XorCipherView.SetMessage(e.ToString(), FilePath);
            XorCipherView.DisplayMessage();
            throw;
        }
        
        XorCipherView.SetMessage("FileEncrypted", FilePath);
        XorCipherView.DisplayMessage();
    }
}