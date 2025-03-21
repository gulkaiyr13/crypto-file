namespace CryptoFileTool
{
    public interface IEncryptionStrategy
    {
        void EncryptFile(string inputFile, string outputFile, string password);
        void DecryptFile(string inputFile, string outputFile, string password);
    }
}