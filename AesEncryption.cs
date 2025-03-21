using System.IO;
using System.Security.Cryptography;

namespace CryptoFileTool
{
    public class AesEncryption : IEncryptionStrategy
    {
        public void EncryptFile(string inputFile, string outputFile, string password)
        {
            if (!File.Exists(inputFile))
                throw new FileNotFoundException($"File for encryption not found: {inputFile}");

            byte[] salt = Utility.GenerateRandomBytes(32);
            using (FileStream fsCrypt = new FileStream(outputFile, FileMode.Create))
            {
                fsCrypt.Write(salt, 0, salt.Length);
                using (Aes aes = Aes.Create())
                {
                    var key = new Rfc2898DeriveBytes(password, salt, 50000);
                    aes.Key = key.GetBytes(aes.KeySize / 8);
                    aes.IV = key.GetBytes(aes.BlockSize / 8);
                    aes.Mode = CipherMode.CBC;
                    using (CryptoStream cs = new CryptoStream(fsCrypt, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    using (FileStream fsIn = new FileStream(inputFile, FileMode.Open))
                    {
                        fsIn.CopyTo(cs);
                    }
                }
            }
        }

        public void DecryptFile(string inputFile, string outputFile, string password)
        {
            if (!File.Exists(inputFile))
                throw new FileNotFoundException($"File for decryption not found: {inputFile}");

            byte[] salt = new byte[32];
            using (FileStream fsCrypt = new FileStream(inputFile, FileMode.Open))
            {
                if (fsCrypt.Read(salt, 0, salt.Length) != salt.Length)
                    throw new CryptographicException("Unable to read salt from file.");

                using (Aes aes = Aes.Create())
                {
                    var key = new Rfc2898DeriveBytes(password, salt, 50000);
                    aes.Key = key.GetBytes(aes.KeySize / 8);
                    aes.IV = key.GetBytes(aes.BlockSize / 8);
                    aes.Mode = CipherMode.CBC;
                    using (CryptoStream cs = new CryptoStream(fsCrypt, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    using (FileStream fsOut = new FileStream(outputFile, FileMode.Create))
                    {
                        cs.CopyTo(fsOut);
                    }
                }
            }
        }
    }
}
