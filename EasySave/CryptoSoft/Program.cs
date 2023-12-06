namespace CryptoSoft
{
    internal static class Program
    {
        private static void Main(string[] args)
        { 
            var filePath = args[0];
            var key = args[1];
            
            _ = new XorCipherViewModel(filePath, key);
        }
    }
}

