namespace CryptoSoft;

public class CryptoSoftView
{
    // Properties
    private string? Message { get; set; }

    private Dictionary<string, string> Messages { get; } 
    
    // Constructor
    
    /// <summary>
    /// CryptoSoftView constructor
    /// </summary>
    public CryptoSoftView()
    {
        // Dictionary containing all the messages
        Messages = new Dictionary<string, string>()
        {
            {"ConfigFileIsInvalid", "Error: Config file is invalid (empty or non-existent)"},
            { "FileIsInvalid", "Error: File located at '{0}' is invalid (empty or non-existent)" },
            { "KeyIsInvalid", "Error: Key is empty or too long" },
            { "FileEncrypted", "Success: File located at '{0}' has been encrypted" }
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
    /// Set the message to display according to the message type
    /// </summary>
    /// <param name="messageType"></param>
    /// <param name="filePath"></param>
    public void SetMessage(string messageType, string filePath = "")
    {
        Message = string.Format(Messages[messageType], filePath);
    }
}