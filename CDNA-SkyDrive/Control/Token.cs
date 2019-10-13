using CDNA_SkyDrive.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDNA_SkyDrive.Control
{
    public class Token
    {
        public static string GetToken(string s)
        {
            string token = s + DateTime.Now.ToString("yyyyMMddHH");
            token += "-" + AES.EncodeAES(token);
            return token;
        }
        public static bool CheckToken()
        {
            return false;
        }
    }
}
