namespace CryptoSoft;

public class XorCipherModel
{
    // Properties
    private string FilePath { get; }
    private string FileContent { get; }
    private string Key { get; }
    
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
        FileContent = File.ReadAllText(filePath);
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
        if (!File.Exists(FilePath) | FileContent.Length == 0)
        {
            throw new FileIsInvalid();
        }

        if (Key.Length > 32 | Key.Length == 0)
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