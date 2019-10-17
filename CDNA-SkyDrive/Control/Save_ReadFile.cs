using Microsoft.AspNetCore.Http;
using System.IO;
using System.Security.Cryptography;



namespace CDNA_SkyDrive.Control
{
    public class Save_ReadFile
    {
        public static bool SaveFile(string filepath, IFormFile file)
        {
            try
            {
                using (FileStream fileStream = new FileStream(filepath, FileMode.Create))
                    file.CopyTo(fileStream);
            }
            catch { return false; }
            return true;
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