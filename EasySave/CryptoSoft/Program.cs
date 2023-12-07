namespace CryptoSoft
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Error: Please provide a file path");
                return;
            }
            
            var filePath = args[0];
            
            _ = new XorCipherViewModel(filePath);
        }
    }
}

