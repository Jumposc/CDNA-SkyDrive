using Microsoft.AspNetCore.Http;
using System.IO;
using System.Security.Cryptography;

namespace CDNA_SkyDrive.Control
{
    public class Save_ReadFile
    {
        public static bool SaveFile(string username, IFormFile file)
        {
            return false;
        }

        public static bool AddFile(string username, IFormFile file)
        {
            return false;
        }

        public static bool ReadFile()
        {
            return false;
        }

        public static byte[] GetHash(Stream file)
        {
            file.Seek(0, SeekOrigin.Begin);
            SHA512 sha512 = SHA512.Create();
            byte[] hash = sha512.ComputeHash(file);
            file.Seek(0, SeekOrigin.Begin);
            return hash;
        }
    }
}