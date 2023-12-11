using System.Text;

namespace CryptoSoft;

public class CryptoSoftModel
{
    // Properties
    private string SourcePath { get; }
    private string DestPath { get; }
    private byte[] Key { get; }
    private string[] ExtensionsToEncrypt { get; }
    
    // Exceptions
    public class FileIsInvalid : Exception
    {
    }
    
    public class KeyIsInvalid : Exception
    {
    }
    
    // Constructor

    /// <summary>
    /// Constructor for the CryptoSoftModel class.
    /// </summary>
    /// <param name="sourcePath"></param>
    /// <param name="destPath"></param>
    /// <param name="key"></param>
    /// <param name="extensionsToEncrypt"></param>
    public CryptoSoftModel(string sourcePath, string destPath, string key, string[] extensionsToEncrypt)
    {
        SourcePath = sourcePath;
        DestPath = destPath;
        Key = Encoding.UTF8.GetBytes(key);
        ExtensionsToEncrypt = extensionsToEncrypt;
    }
    
    // Methods
    
    /// <summary>
    /// Save the file located at SourcePath to DestPath, then encrypt it.
    /// </summary>
    /// <exception cref="FileIsInvalid"></exception>
    /// <exception cref="KeyIsInvalid"></exception>
    public void CreateEncryptedFile()
    {
        // Check if the file exists.
        if (string.IsNullOrEmpty(SourcePath) || !File.Exists(SourcePath))
        {
            throw new FileIsInvalid();
        }
        
        File.Copy(SourcePath, DestPath, true);
        
        // Check if the file extension is in the list of extensions to encrypt. If not, the file is not encrypted.
        var fileExtension = Path.GetExtension(SourcePath);
        
        if (ExtensionsToEncrypt.Contains(fileExtension))
        {
            EncryptFile();
        }
    }
    
    /// <summary>
    /// Encrypts/Decrypts the file content using the XOR cipher algorithm with the given key.
    /// </summary>
    private void EncryptFile()
    {
        // Check if the key is empty or too short.
        if (Key == null || Key.Length < 8)
        {
            throw new KeyIsInvalid();
        }
        
        using var fileStream = new FileStream(DestPath, FileMode.Open, FileAccess.ReadWrite);
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