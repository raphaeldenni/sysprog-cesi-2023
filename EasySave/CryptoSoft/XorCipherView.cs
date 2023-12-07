namespace CryptoSoft;

public class XorCipherView
{
    // Properties
    public string? Message { get; set; }
    public string Lang { get; }

    private Dictionary<string, Dictionary<string, string>> Messages { get; } 
    
    // Constructor
    
    /// <summary>
    /// XorCipherView constructor
    /// </summary>
    /// <param name="lang"></param>
    public XorCipherView(string lang)
    {
        Lang = lang;
        
        // Dictionary containing all the messages in all the languages
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
    
    /// <summary>
    /// Display a message with the given Message property
    /// </summary>
    public void DisplayMessage()
    {
        Console.WriteLine(Message);
    }
    
    /// <summary>
    /// Set the message to display according to the language and the message type
    /// </summary>
    /// <param name="messageType"></param>
    /// <param name="filePath"></param>
    public void SetMessage(string messageType, string filePath)
    {
        Message = string.Format(Messages[Lang][messageType], filePath);
    }
}