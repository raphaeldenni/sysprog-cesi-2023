namespace CryptoSoft;

public class XorCipherView
{
    // Properties
    public string? Message { get; set; }
    
    // Constructor
    public XorCipherView()
    {
        
    }
    
    // Methods
    public void DisplayMessage()
    {
        Console.WriteLine(Message);
    }
}