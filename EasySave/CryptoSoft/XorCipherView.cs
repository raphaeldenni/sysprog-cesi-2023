namespace CryptoSoft;

public class XorCipherView
{
    // Properties
    public string? Message { get; set; }
    public string Lang { get; }

    private Dictionary<string, Dictionary<string, string>> Messages { get; } 
    
    // Constructor
    public XorCipherView(string lang)
    {
        Lang = lang;
        
        Messages = new Dictionary<string, Dictionary<string, string>>()
        {
            {
                "en", new Dictionary<string, string>()
                {
                    { "FileNotFound", "File(s) located at {0} not found" },
                    { "FileIsEmpty", "File located at {0} is empty" },
                    { "KeyIsEmpty", "Key is empty" },
                    { "FileEncrypted", "File located at {0} has been encrypted" }
                }
            },
            {
                "fr", new Dictionary<string, string>()
                {
                    { "FileNotFound", "Fichier(s) situé(s) à {0} introuvable(s)" },
                    { "FileIsEmpty", "Fichier situé à {0} est vide" },
                    { "KeyIsEmpty", "Clé est vide" },
                    { "FileEncrypted", "Fichier situé à {0} a été chiffré" }
                }
            }
        };
    }

    // Methods
    public void DisplayMessage()
    {
        Console.WriteLine(Message);
    }
    
    public void SetMessage(string messageType, string filePath)
    {
        Message = string.Format(Messages[Lang][messageType], filePath);
    }
}