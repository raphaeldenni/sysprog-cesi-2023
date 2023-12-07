namespace CryptoSoft;

public class XorCipherModel
{
    // Properties
    private string FilePath { get; set; }
    private string FileContent { get; set; }
    private string Key { get; set; }
    
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
    public void XorCipher()
    {
        var encryptedFileContent = String.Empty;
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