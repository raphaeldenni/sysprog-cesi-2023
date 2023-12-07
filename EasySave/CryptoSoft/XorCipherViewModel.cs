namespace CryptoSoft;

public class XorCipherViewModel
{
    // View and Model
    private XorCipherModel XorCipherModel { get; set; }
    private XorCipherView XorCipherView { get; set; }
    
    // Properties
    private string File { get; set; }
    private string Key { get; set; }
    
    // Constructor
    
    /// <summary>
    /// XorCipherViewModel constructor
    /// </summary>
    /// <param name="file"></param>
    /// <param name="key"></param>
    public XorCipherViewModel(string file, string key)
    {
        XorCipherModel = new XorCipherModel(file, key);
        XorCipherView = new XorCipherView("en");
         
        File = file;
        Key = key;
        
        // Check if the file(s) located at the given path exist(s) and if the key is empty or not.
        if (!System.IO.File.Exists(File))
        {
            XorCipherView.SetMessage("FileNotFound", File);
            XorCipherView.DisplayMessage();
            return;
        }
        
        if (System.IO.File.ReadAllText(File).Length == 0)
        {
            XorCipherView.SetMessage("FileIsEmpty", File);
            XorCipherView.DisplayMessage();
            return;
        }
        
        if (Key.Length == 0)
        {
            XorCipherView.SetMessage("KeyIsEmpty", File);
            XorCipherView.DisplayMessage();
            return;
        }
        
        XorCipherModel.XorCipher();
        
        XorCipherView.SetMessage("FileEncrypted", File);
        XorCipherView.DisplayMessage();
    }
}