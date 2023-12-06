namespace CryptoSoft
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: CryptoSoft.exe <file_path> <key>");
                return;
            }
            
            var filePath = args[0];
            
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File located at {filePath} not found");
                return;
            }
            
            var fileContent = File.ReadAllText(filePath);
            
            if (fileContent.Length == 0)
            {
                Console.WriteLine($"File located at {filePath} is empty");
                return;
            }
            
            var key = args[1];
            
            if (key.Length == 0)
            {
                Console.WriteLine("Key cannot be empty");
                return;
            }
            
            var encryptedFileContent = XorCipher(fileContent, key);
            
            File.WriteAllText(filePath, encryptedFileContent);
            
            Console.WriteLine($"File located at {filePath} xored with key {key}");
            
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static string XorCipher(string fileContent, string key)
        {
            var encryptedFileContent = String.Empty;
            var keyIndex = 0;
            
            foreach (var character in fileContent)
            {
                encryptedFileContent += (char) (character ^ key[keyIndex]);
                keyIndex = (keyIndex + 1) % key.Length;
            }
            
            return encryptedFileContent;
        }
    }
}

