using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace CryptoFileTool.Api {
    [ApiController]
    [Route("api/[controller]")]
    public class EncryptionController : ControllerBase
    {
        private readonly string decodeDir = @"C:\Users\user\RiderProjects\CryptoFileTool\CryptoFileTool\decode";
        private readonly string encodeDir = @"C:\Users\user\RiderProjects\CryptoFileTool\CryptoFileTool\encode";
        
        [HttpPost("encrypt")]
        public IActionResult Encrypt([FromQuery] string method, [FromQuery] string filename, [FromQuery] string password) {
            if (string.IsNullOrWhiteSpace(method) || string.IsNullOrWhiteSpace(filename) || string.IsNullOrWhiteSpace(password))
                return BadRequest("All parameters are required.");

            IEncryptionStrategy strategy = EncryptionFactory.GetEncryptionStrategy(method);
            if (strategy == null)
                return BadRequest("Invalid encryption method selected.");

            string inputFile = Path.Combine(decodeDir, filename);
            if (!System.IO.File.Exists(inputFile))
                return NotFound("The file for encryption was not found in the decode folder.");

            string targetFile = Path.Combine(encodeDir, filename);
            try {
                if (System.IO.File.Exists(targetFile)) {
                    string newFileName = Path.GetFileNameWithoutExtension(filename) +
                                         "_" + DateTime.Now.ToString("yyyyMMddHHmmss") +
                                         Path.GetExtension(filename);
                    targetFile = Path.Combine(encodeDir, newFileName);
                }

                strategy.EncryptFile(inputFile, targetFile, password);
                System.IO.File.Delete(inputFile);
                return Ok($"File {filename} has been encrypted and moved to the encode folder.");
            }
            catch (Exception ex) {
                return StatusCode(500, $"Error encrypting the file: {ex.Message}");
            }
        }

        [HttpPost("decrypt")]
        public IActionResult Decrypt([FromQuery] string method, [FromQuery] string filename, [FromQuery] string password) {
            if (string.IsNullOrWhiteSpace(method) || string.IsNullOrWhiteSpace(filename) || string.IsNullOrWhiteSpace(password))
                return BadRequest("All parameters are required.");

            IEncryptionStrategy strategy = EncryptionFactory.GetEncryptionStrategy(method);
            if (strategy == null)
                return BadRequest("Invalid encryption method selected.");

            string inputFile = Path.Combine(encodeDir, filename);
            if (!System.IO.File.Exists(inputFile))
                return NotFound("The file for decryption was not found in the encode folder.");

            string targetFile = Path.Combine(decodeDir, filename);
            try {
                if (System.IO.File.Exists(targetFile)) {
                    string newFileName = Path.GetFileNameWithoutExtension(filename) +
                                         "_" + DateTime.Now.ToString("yyyyMMddHHmmss") +
                                         Path.GetExtension(filename);
                    targetFile = Path.Combine(decodeDir, newFileName);
                }

                strategy.DecryptFile(inputFile, targetFile, password);
                System.IO.File.Delete(inputFile);
                return Ok($"File {filename} has been decrypted and moved to the decode folder.");
            }
            catch (Exception ex) {
                return StatusCode(500, $"Error decrypting the file: {ex.Message}");
            }
        }
    }
}
