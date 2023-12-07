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
        XorCipherModel = new XorCipherModel(filePath, key);
        XorCipherView = new XorCipherView("en");
         
        FilePath = filePath;
        Key = key;
        
        // Check if the file(s) located at the given path exist(s) and if the key is empty or not.
        if (!System.IO.File.Exists(FilePath))
        {
            XorCipherView.SetMessage("FileNotFound", FilePath);
            XorCipherView.DisplayMessage();
            return;
        }
        
        if (System.IO.File.ReadAllText(FilePath).Length == 0)
        {
            XorCipherView.SetMessage("FileIsEmpty", FilePath);
            XorCipherView.DisplayMessage();
            return;
        }
        
        if (Key.Length == 0)
        {
            XorCipherView.SetMessage("KeyIsEmpty", FilePath);
            XorCipherView.DisplayMessage();
            return;
        }
        
        XorCipherModel.XorCipher();
        
        XorCipherView.SetMessage("FileEncrypted", FilePath);
        XorCipherView.DisplayMessage();
    }
}