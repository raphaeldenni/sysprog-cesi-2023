namespace CryptoSoft;

public class XorCipherView
{
    // Properties
    private string? Message { get; set; }
    private string Lang { get; }

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
                    { "FileIsInvalid", "Error: File located at '{0}' is invalid (empty or non-existent)" },
                    { "KeyIsInvalid", "Error: Key is empty or too long" },
                    { "FileEncrypted", "Success: File located at '{0}' has been encrypted" }
                }
            },
            {
                "fr", new Dictionary<string, string>()
                {
                    { "FileIsInvalid", "Erreur: Le fichier situé à '{0}' est invalide (vide ou inexistant)" },
                    { "KeyIsInvalid", "Erreur: La clé est vide ou trop longue" },
                    { "FileEncrypted", "Succès: Le fichier situé à '{0}' a été chiffré" }
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