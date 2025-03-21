# CryptoFileTool

CryptoFileTool is a dual-mode application that supports file encryption and decryption using different algorithms (currently AES and XOR). The project is designed to work both as a console application and as a REST API, making it flexible for local use as well as remote access.

## Features

- **Encryption/Decryption Algorithms:**  
  - **AES:** Uses salt and PBKDF2-based key derivation for secure encryption.![image](https://github.com/user-attachments/assets/96df9b86-daae-4464-844e-4034b7ab610f)

  - **XOR:** A simple method for demonstration purposes (not secure for production). ![image](https://github.com/user-attachments/assets/77a51b2c-b658-4bd2-8daf-6f2210b8a084)


- **Dual-mode Operation:**  
  - **Console Mode:** Processes files automatically by moving them from the `decode` folder to the `encode` folder (and vice versa).
  - ![image](https://github.com/user-attachments/assets/1fdc20a9-96e1-4a0c-9e7d-90180dbedb63)

  - **REST API Mode:** Exposes endpoints for encryption and decryption via HTTP.
  - ![image](https://github.com/user-attachments/assets/b7698be8-a371-40cc-8e48-63e1843cf5e4)


- **Automatic File Handling:**  
  Files are automatically moved to the target folder after processing. If a file with the same name exists, a timestamp suffix is added to ensure uniqueness.

- **Validation and Error Handling:**  
  The application performs input validation and handles errors gracefully.

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (or later)
- Git (for cloning and version control)

## Installation

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/gulkaiyr13/crypto-file.git
   cd crypto-file
   ```
2. **Build the Project:**
Use the .NET CLI to restore and build the solution:

 ```bash
 dotnet restore
 dotnet build
```
**Console Mode**
By default, running the application without arguments launches the console mode.
Run the Application:
 ```bash
 dotnet run
```

**REST API Mode**
To run the application as a REST API, pass the api argument when starting the application.
Run the API:
 ```bash
 dotnet run --api
```

Encryption Endpoint:
POST http://localhost:5000/api/encryption/encrypt
Query Parameters:

method: Encryption method (1 for AES, 2 for XOR)
filename: Name of the file (located in the decode folder)
password: Password for encryption

Example:
![image](https://github.com/user-attachments/assets/8f8413be-e3c9-46c8-a0d3-6f93c3dc87ba)

Decryption Endpoint:
POST http://localhost:5000/api/encryption/decrypt

***Contributing***
Contributions are welcome! Please feel free to fork the repository, submit pull requests, or open issues for any improvements or bug fixes.
