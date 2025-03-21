using System.Security.Cryptography;

namespace CryptoFileTool
{
    public static class Utility
    {
        public static byte[] GenerateRandomBytes(int length)
        {
            byte[] bytes = new byte[length];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }
            return bytes;
        }
    }
}