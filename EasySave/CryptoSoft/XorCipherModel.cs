using System.Text;

namespace CryptoSoft;

public class XorCipherModel
{
    // Properties
    private string? FilePath { get; }
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
        Key = Encoding.UTF8.GetBytes(key);
    }
    
    // Methods
    
    /// <summary>
    /// Encrypts/Decrypts the file content using the XOR cipher algorithm with the given key.
    /// </summary>
    public void EncryptFile()
    {
        // Check if the file exists.
        if (string.IsNullOrEmpty(FilePath) || !File.Exists(FilePath))
        {
            throw new FileIsInvalid();
        }
        
        // Check if the key is empty or too short.
        if (Key == null || Key.Length < 8)
        {
            throw new KeyIsInvalid();
        }
        
        using var fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite);
        var keyIndex = 0;
        
        // XOR each byte of the file content with the corresponding byte character of the key.
        // If the key is shorter than the file content, the key will be repeated. A modulo operation is used to achieve this.
        for (var i = 0; i < fileStream.Length; i++)
        {
            var fileByte = fileStream.ReadByte();
            var encryptedByte = (byte)(fileByte ^ Key[keyIndex]);
            
            fileStream.Position = i;
            fileStream.WriteByte(encryptedByte);
            
            keyIndex = (keyIndex + 1) % Key.Length;
        }
    }
}