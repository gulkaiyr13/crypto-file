using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CryptoFileTool
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0].Equals("api", StringComparison.OrdinalIgnoreCase))
            {
                RunWebApi(args);
            }
            else
            {
                RunConsoleApp();
            }
        }

        static void RunConsoleApp()
        {
            Console.WriteLine("=== CryptoFileTool (Console Application) ===");
            Console.WriteLine("Choose an operation:");
            Console.WriteLine("1 - Encryption (from decode to encode)");
            Console.WriteLine("2 - Decryption (from encode to decode)");
            Console.Write("Enter the operation number: ");
            string operation = Console.ReadLine();

            if (operation != "1" && operation != "2")
            {
                Console.WriteLine("Error: Invalid operation selected.");
                return;
            }

            Console.WriteLine("Choose the encryption method:");
            Console.WriteLine("1 - AES");
            Console.WriteLine("2 - XOR");
            Console.Write("Enter the method number: ");
            string methodChoice = Console.ReadLine();

            IEncryptionStrategy strategy = EncryptionFactory.GetEncryptionStrategy(methodChoice);
            if (strategy == null)
            {
                Console.WriteLine("Error: Invalid encryption method selected.");
                return;
            }

            Console.Write("Enter the password for encryption/decryption: ");
            string password = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Error: Password cannot be empty.");
                return;
            }

            string sourceDir, targetDir;
            if (operation == "1")
            {
                sourceDir = @"C:\Users\user\RiderProjects\CryptoFileTool\CryptoFileTool\decode";
                targetDir = @"C:\Users\user\RiderProjects\CryptoFileTool\CryptoFileTool\encode";
            }
            else
            {
                sourceDir = @"C:\Users\user\RiderProjects\CryptoFileTool\CryptoFileTool\encode";
                targetDir = @"C:\Users\user\RiderProjects\CryptoFileTool\CryptoFileTool\decode";
            }

            if (!Directory.Exists(sourceDir))
            {
                Console.WriteLine($"Error: The source directory does not exist: {sourceDir}");
                return;
            }
            if (!Directory.Exists(targetDir))
            {
                Console.WriteLine($"Error: The target directory does not exist: {targetDir}");
                return;
            }

            string[] files = Directory.GetFiles(sourceDir);
            if (files.Length == 0)
            {
                Console.WriteLine("No files to process in directory " + sourceDir);
            }
            else
            {
                foreach (string file in files)
                {
                    try
                    {
                        string fileName = Path.GetFileName(file);
                        string targetFile = Path.Combine(targetDir, fileName);

                        if (File.Exists(targetFile))
                        {
                            string newFileName = Path.GetFileNameWithoutExtension(fileName) +
                                                 "_" + DateTime.Now.ToString("yyyyMMddHHmmss") +
                                                 Path.GetExtension(fileName);
                            targetFile = Path.Combine(targetDir, newFileName);
                        }

                        if (operation == "1")
                        {
                            strategy.EncryptFile(file, targetFile, password);
                        }
                        else
                        {
                            strategy.DecryptFile(file, targetFile, password);
                        }
                        File.Delete(file);
                        Console.WriteLine($"Processed file: {fileName}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing file {Path.GetFileName(file)}: {ex.Message}");
                    }
                }
            }
            Console.WriteLine("Operation completed. Press any key to exit...");
            Console.ReadKey();
        }

        static void RunWebApi(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            var app = builder.Build();
            app.MapControllers();
            app.Run();
        }
    }
}
