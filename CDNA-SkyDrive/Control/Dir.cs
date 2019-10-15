using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CDNA_SkyDrive.Control
{
    public class Dir
    {
        public static JToken Intodir(JToken jToken, Queue<string> list)
        {
            if (list.Count == 0)
                return null;
            string name = list.Dequeue();
            if (name == "")
                return jToken;
            foreach (var item in jToken)
                if (item["type"].ToString() == "dir" && item["name"].ToString() == name)
                    return Intodir(item["data"], list);
            return null;
        }

        public static JToken AddDir(JArray jArray, Queue<string> list, JToken adddir)
        {
            if (list.Count == 0)
                return null;
            string name = list.Dequeue();
            if (name == "")
            {
                JObject q = JObject.Parse(jArray.ToString());
                q["data"].AddAfterSelf(adddir);
                return q;
            }
            for (int i = 0; i < jArray.Count; i++)
            {
                JObject q = JObject.Parse(jArray[i].ToString());
                if (q["type"].ToString() == "dir" && q["name"].ToString() == name)
                {
                    string data = q["data"].ToString();
                    if (data == "[]")
                    {
                        name = list.Dequeue();
                        if (name == "")
                        {
                            JArray j = JArray.Parse(q["data"].ToString());
                            j.Add(adddir);
                            q["data"] = j;
                            return $"[{q}]";
                        }
                        else return null;
                    }
                    data = data.Substring(1, data.Length - 1);
                    data = data.Substring(0, data.Length - 1);
                    JToken o = jArray[i];
                    o["data"] = AddDir(new JArray(data), list, adddir);
                    jArray[i] = o;
                    return jArray;
                }

            }
            return null;
        }
    }
}