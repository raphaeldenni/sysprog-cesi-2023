namespace CryptoSoft;

public class XorCipherModel
{
    // Properties
    private string? FilePath { get; }
    private string? FileContent { get; set; }
    private string? Key { get; }
    
    // Exceptions
    public class FileIsInvalid : Exception
    {
    }
    
    public class KeyIsInvalid : Exception
    {
    }
    
    // Constructor
    
    /// <summary>
    /// Constructor for the XorCipherModel class.
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="key"></param>
    public XorCipherModel(string filePath, string key)
    {
        FilePath = filePath;
        FileContent = !string.IsNullOrEmpty(FilePath) && File.Exists(FilePath) 
            ? File.ReadAllText(FilePath) 
            : string.Empty;
        
        Key = key;
    }
    
    // Methods
    
    /// <summary>
    /// Encrypts/Decrypts the file content using the XOR cipher algorithm with the given key.
    /// </summary>
    /// <returns>String that contains the encrypted file content.</returns>
    public void EncryptFile()
    {
        // Check if the file exists and if it is empty. Also check if the key is valid.
        if (string.IsNullOrEmpty(FilePath) || !File.Exists(FilePath) || string.IsNullOrEmpty(FileContent))
        {
            throw new FileIsInvalid();
        }

        if (string.IsNullOrEmpty(Key) || Key.Length > 32)
        {
            throw new KeyIsInvalid();
        }

        var encryptedFileContent = string.Empty;
        var keyIndex = 0;
        
        // XOR each character of the file content with the corresponding character of the key.
        // If the key is shorter than the file content, the key will be repeated. A modulo operation is used to achieve this.
        foreach (var character in FileContent)
        {
            encryptedFileContent += (char) (character ^ Key[keyIndex]);
            keyIndex = (keyIndex + 1) % Key.Length;
        }
        
        File.WriteAllText(FilePath, encryptedFileContent);
    }
}