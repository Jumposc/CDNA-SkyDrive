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

        public static JToken AddJson(JArray jArray, Queue<string> list, JToken addJson)
        {
            if (list.Count == 0)
                return null;
            string name = list.Dequeue();
            if (name == "")
            {
                jArray.Add(addJson);
                return jArray;
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
                            j.Add(addJson);
                            q["data"] = j;
                            return $"[{q}]";
                        }
                        else return null;
                    }
                    JToken o = jArray[i];
                    o["data"] = AddJson(JArray.Parse(data), list, addJson);
                    jArray[i] = o;
                    return jArray;
                }

            }
            return null;
        }
    }
}