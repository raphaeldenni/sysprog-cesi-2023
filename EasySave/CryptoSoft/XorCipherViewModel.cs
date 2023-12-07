namespace CryptoSoft;

public class XorCipherViewModel
{
    // View and Model
    private XorCipherModel XorCipherModel { get; }
    private XorCipherView XorCipherView { get; }
    
    // Properties
    private string FilePath { get; }
    private string Key { get; }
    
    // Constructor
    
    /// <summary>
    /// XorCipherViewModel constructor
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="key"></param>
    public XorCipherViewModel(string filePath, string key)
    {
        FilePath = filePath;
        Key = key;
        
        XorCipherModel = new XorCipherModel(FilePath, Key);
        XorCipherView = new XorCipherView("en");

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