using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CDNA_SkyDrive.Control
{
    public class Resources
    {
        /// <summary>
        ///获取静态资源值
        /// </summary>
        /// <param name="Key">Key值</param>
        /// <returns>资源内容</returns>
        public static string GetResources(string Key)
        {
            JObject Json = JObject.Parse(new StreamReader(new FileStream("Resources.txt", FileMode.Open)).ReadToEnd());
            return Json[Key].ToString();
        }
    }
}
