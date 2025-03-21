namespace CryptoFileTool
{
    public static class EncryptionFactory
    {
        public static IEncryptionStrategy GetEncryptionStrategy(string choice)
        {
            switch (choice)
            {
                case "1":
                    return new AesEncryption();
                case "2":
                    return new XorEncryption();
                default:
                    return null;
            }
        }
    }
}