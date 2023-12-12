namespace CryptoSoft;

public class CryptoSoftViewModel
{
    // View and Model
    private CryptoSoftModel? CryptoSoftModel { get; }
    private CryptoSoftView CryptoSoftView { get; }
    
    // Properties
    private string? SourcePath { get; }
    private string? DestPath { get; }
    private string? Key { get; }
    
    // Constructor

    /// <summary>
    /// CryptoSoftViewModel constructor
    /// </summary>
    /// <param name="args"></param>
    public CryptoSoftViewModel(IReadOnlyList<string> args)
    {
        // Set the properties according to the arguments, if any.
        if (args.Count >= 3 )
        {
            SourcePath = args[0];
            DestPath = args[1];
            Key = args[2];
        }
        else
        {
            SourcePath = DestPath = Key = string.Empty;
        }
        
        CryptoSoftModel = new CryptoSoftModel(SourcePath, DestPath, Key);
        CryptoSoftView = new CryptoSoftView();

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