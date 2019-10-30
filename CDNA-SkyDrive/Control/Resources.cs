using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading;

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
            FileStream file;
            try
            {
                file = new FileStream("Resources.txt", FileMode.Open);
            }
            catch
            {
                Thread.Sleep(10);
                return GetResources(Key);
            }
            JObject Json = JObject.Parse(new StreamReader(file).ReadToEnd());
            file.Close();
            return Json[Key].ToString();
        }
    }
}