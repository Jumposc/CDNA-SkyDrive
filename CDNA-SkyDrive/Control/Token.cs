using CDNA_SkyDrive.API;
using System;
using System.Globalization;

namespace CDNA_SkyDrive.Control
{
    public class Token
    {
        public static string s = "[{\"time\": \"2019-10-15\", \"name\": \".\", \"type\": \"dir\", \"data\": []}]";

        public static string GetToken(string s)
        {
            string token = s + DateTime.Now.ToString("yyyyMMddHH");
            token += "-" + AES.EncodeAES(token);
            return token;
        }

        public static bool CheckToken(string s)
        {
            string[] str = s.Split('-');
            string aaa = AES.DecodeAES(str[1]);
            if (str[0] == AES.DecodeAES(str[1]).Replace("\0", ""))
            {
                DateTime tokentime = DateTime.ParseExact(str[0].Substring(str[0].Length - 10), "yyyyMMddHH", CultureInfo.InvariantCulture);
                if (tokentime.AddDays(1) > DateTime.Now)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}