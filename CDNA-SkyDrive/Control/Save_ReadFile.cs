using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
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

        public static String GetHash(Stream file)
        {
            file.Seek(0, SeekOrigin.Begin);
            SHA512 sha512 = SHA512.Create();
            byte[] hash = sha512.ComputeHash(file);
            file.Seek(0, SeekOrigin.Begin);
            return Encoding.UTF8.GetString(hash);
        }
    }
}
