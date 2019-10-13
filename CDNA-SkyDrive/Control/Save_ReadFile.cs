using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http.Internal;

namespace CDNA_SkyDrive.Control
{
    public class Save_ReadFile
    {
        public static bool SaveFile(string username, FormFile file)
        {

            return false;
        }

        public static bool ReadFile()
        {
            return false;
        }
        public static byte[] GetHash(FileStream file)
        {
            file.Seek(0, SeekOrigin.Begin);
            SHA1 sha1 = SHA1.Create();
            byte[] hash = sha1.ComputeHash(file);
            file.Seek(0, SeekOrigin.Begin);
            return hash;
        }
    }
}
