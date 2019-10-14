using Newtonsoft.Json.Linq;
using System.IO;

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
            FileStream file = new FileStream("Resources.txt", FileMode.Open);
            JObject Json = JObject.Parse(new StreamReader(file).ReadToEnd());
            file.Close();
            return Json[Key].ToString();
        }
    }
}