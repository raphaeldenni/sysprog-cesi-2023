namespace CryptoSoft;

public class XorCipherViewModel
{
    // View and Model
    private XorCipherModel XorCipherModel { get; set; }
    private XorCipherView XorCipherView { get; set; }
    
    // Properties
    private string SourcePath { get; set; }
    private string Key { get; set; }
    
    // Constructor
    
    /// <summary>
    /// XorCipherViewModel constructor
    /// </summary>
    /// <param name="sourcePath"></param>
    /// <param name="key"></param>
    public XorCipherViewModel(string sourcePath, string key)
    {
        XorCipherModel = new XorCipherModel(sourcePath, key);
        XorCipherView = new XorCipherView();
        
        SourcePath = sourcePath;
        Key = key;
        
        // Check if the file(s) located at the given path exist(s) and if the key is empty or not.
        if (Key.Length == 0 && (!File.Exists(SourcePath) || !Directory.Exists(SourcePath)))
        {
            XorCipherView.Message = $"File(s) located at {SourcePath} not found and/or key is empty";
            return;
        }
        
        XorCipherModel.XorCipher();
        
        XorCipherView.Message = $"File located at {SourcePath} has been encrypted";
        XorCipherView.DisplayMessage();
    }
}