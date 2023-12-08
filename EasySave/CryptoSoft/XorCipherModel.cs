using System.Text;

namespace CryptoSoft;

public class XorCipherModel
{
    // Properties
    private string? FilePath { get; }
    private byte[]? FileContent { get; }
    private byte[]? Key { get; }
    
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
        
        // File content and key are stored as byte arrays to be able to use the XOR operator on any type of file.
        FileContent = !string.IsNullOrEmpty(FilePath) && File.Exists(FilePath) 
            ? File.ReadAllBytes(FilePath) 
            : null as byte[];

        Key = Encoding.UTF8.GetBytes(key);
    }
    
    // Methods
    
    /// <summary>
    /// Encrypts/Decrypts the file content using the XOR cipher algorithm with the given key.
    /// </summary>
    public void EncryptFile()
    {
        // Check if the file exists and if it is empty.
        if (string.IsNullOrEmpty(FilePath) || !File.Exists(FilePath) || FileContent == null)
        {
            throw new FileIsInvalid();
        }
        
        // Check if the key is empty or too short.
        if (Key == null || Key.Length < 8)
        {
            throw new KeyIsInvalid();
        }
        
        var encryptedFileContent = new byte[FileContent.Length];
        var keyIndex = 0;
        
        // XOR each byte of the file content with the corresponding byte character of the key.
        // If the key is shorter than the file content, the key will be repeated. A modulo operation is used to achieve this.
        for (var i = 0; i < FileContent.Length; i++)
        {
            encryptedFileContent[i] = (byte) (FileContent[i] ^ Key[keyIndex]);
            keyIndex = (keyIndex + 1) % Key.Length;
        }
        
        File.WriteAllBytes(FilePath, encryptedFileContent);
    }
}