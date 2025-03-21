using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptoFileTool
{
    public class XorEncryption : IEncryptionStrategy
    {
        public void EncryptFile(string inputFile, string outputFile, string password)
        {
            ProcessFile(inputFile, outputFile, password);
        }

        public void DecryptFile(string inputFile, string outputFile, string password)
        {
            ProcessFile(inputFile, outputFile, password);
        }

        private void ProcessFile(string inputFile, string outputFile, string password)
        {
            if (!File.Exists(inputFile))
                throw new FileNotFoundException($"File not found: {inputFile}");

            byte[] fileBytes = File.ReadAllBytes(inputFile);
            byte[] keyBytes = Encoding.UTF8.GetBytes(password);
            if (keyBytes.Length == 0)
                throw new CryptographicException("Password cannot be empty.");

            byte[] result = new byte[fileBytes.Length];
            for (int i = 0; i < fileBytes.Length; i++)
            {
                result[i] = (byte)(fileBytes[i] ^ keyBytes[i % keyBytes.Length]);
            }
            File.WriteAllBytes(outputFile, result);
        }
    }
}