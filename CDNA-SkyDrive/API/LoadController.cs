using CDNA_SkyDrive.Control;
using CDNA_SkyDrive.Mode;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;

namespace CDNA_SkyDrive.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoadController : ControllerBase
    {
        private const string FilePath = "/var/CDNA-SkyDrive/File";
        static Dictionary<string, CDNAFileStream> streamtable = new Dictionary<string, CDNAFileStream>();

        [HttpPost()]
        [Route("Up")]
        //[DisableFormValueModelBinding]
        //[RequestFormLimits(MultipartBodyLengthLimit = 200 * 1024 * 1024)]
        public IActionResult UpLoad()
        {
            string token = Request.Cookies["Token"];
            string[] p = Request.Headers["Path"].ToString().Split('/');
            //string[] p = "./A/".Split('/');
            var files = Request.Form.Files;
            if (Token.CheckToken(token) && files != null)
            {
                Stream stream = null;
                string saveFilePath = null;
                JArray filestr = null;
                string name = null;
                try
                {
                    foreach (var file in files)
                    {
                        string ID = token.Split("-")[0];
                        ID = ID.Substring(0, ID.Length - 10);
                        DataTable table;
                        if ((table = SQLControl.Select($"SELECT * FROM CDNABASE.UserTable where  ID={ID};")) == null)
                            throw new NewSqlException();
                        name = table.Rows[0][1].ToString();

                        filestr = null;
                        stream = file.OpenReadStream();
                        byte[] hash = Save_ReadFile.GetHash(stream);

                        MySqlParameter hashParameter = new MySqlParameter("@hash", MySqlDbType.TinyBlob);
                        hashParameter.Value = hash;
                        int fileID = 0;
                        //检查Hash表里是否有这个文件
                        if (-1 == (fileID = SQLControl.Select($"SELECT * FROM CDNABASE.HashTable where Hash = @hash;", hashParameter)))
                        {//没有就加进去
                            string filename = DateTime.Now.ToString("yyyyMMddhhmmss");
                            while (System.IO.File.Exists(FilePath + filename))
                            { filename = DateTime.Now.ToString("yyyyMMddhhmmss") + new Random().Next(10); }
                            if (!Save_ReadFile.SaveFile(FilePath + filename, file))
                                throw new IOException();
                            MySqlParameter blobParameter = new MySqlParameter("@hash", MySqlDbType.TinyBlob);
                            blobParameter.Value = hash;
                            if (0 == SQLControl.Execute($"insert CDNABASE.HashTable value (0,@hash,'{FilePath + filename}');", blobParameter))
                                throw new NewSqlException();
                            fileID = SQLControl.Select($"SELECT * FROM CDNABASE.HashTable where Hash = @hash;", blobParameter);
                        }
                        //把Hash绑定到用户文件列表上
                        do
                        {
                            table = null;
                            if ((table = SQLControl.Select($"SELECT * FROM CDNABASE.UserFileTable where UserName='{name}';")) == null)
                                throw new NewSqlException();
                        } while (int.Parse(table.Rows[0][2].ToString()) != 1);
                        filestr = JArray.Parse(table.Rows[0][1].ToString());
                        Queue<string> pathlist = new Queue<string>(p);
                        JObject jo = new JObject();
                        Encoding encoding = Encoding.Default;
                        jo.Add("time", DateTime.Now.ToString("yyyy-MM-dd"));
                        jo.Add("name", file.FileName);
                        jo.Add("type", "file");
                        jo.Add("size", file.Length);
                        jo.Add("data", fileID);
                        if (Dir.SelectFileName(filestr, pathlist, file.FileName))
                            return BadRequest(JsonConvert.SerializeObject(new ReturnMode() { Data = "重名", Message = "Error" }));
                        pathlist = new Queue<string>(p);
                        JToken newdir = Dir.AddJson(filestr, pathlist, JToken.Parse(jo.ToString()));

                        SQLControl.Execute($"UPDATE CDNABASE.UserFileTable SET File='' , State = 0 where UserName='{name}';");
                        SQLControl.Execute($"UPDATE CDNABASE.UserFileTable SET File='{newdir.ToString()}',State = 1 where UserName='{name}';");
                    }
                }
                catch (IOException)
                {
                    if (stream != null)
                        stream.Close();
                    if (System.IO.File.Exists(saveFilePath))
                        System.IO.File.Delete(saveFilePath);
                    return StatusCode(500, JsonConvert.SerializeObject(new ReturnMode() { Data = "服务器保存错误", Message = "Error" }));
                }
                catch (NewSqlException)
                {
                    return StatusCode(500, JsonConvert.SerializeObject(new ReturnMode() { Data = "数据库错误", Message = "Error" }));
                }
                catch (MySqlException)
                {
                    if (filestr != null)
                        SQLControl.Execute($"UPDATE CDNABASE.UserFileTable SET File='{filestr.ToString()}' , State = 1 where UserName='{name}';");
                }
            }
            else
                return BadRequest(JsonConvert.SerializeObject(new ReturnMode() { Data = "Token错误", Message = "Error" }));
            return Ok(JsonConvert.SerializeObject(new ReturnMode() { Data = "保存完成！", Message = "OK" }));
        }

        [HttpPost()]
        [Route("Down")]
        public IActionResult DownLoad()
        {
            string token = Request.Cookies["Token"];
            string[] p = Request.Headers["Path"].ToString().Split('/');
            //string[] p = "./ALL01UMD.sav".Split('/');
            if (Token.CheckToken(token))
            {
                string ID = token.Split("-")[0];
                ID = ID.Substring(0, ID.Length - 10);
                DataTable table;
                if ((table = SQLControl.Select($"SELECT * FROM CDNABASE.UserTable where  ID={ID};")) == null)
                    return StatusCode(500, JsonConvert.SerializeObject(new ReturnMode() { Data = "数据库错误", Message = "Error" }));
                string name = table.Rows[0][1].ToString();
                if ((table = SQLControl.Select($"SELECT * FROM CDNABASE.UserFileTable where UserName='{name}';")) == null)
                    return StatusCode(500, JsonConvert.SerializeObject(new ReturnMode() { Data = "数据库错误", Message = "Error" }));
                JToken file = JToken.Parse(table.Rows[0][1].ToString());
                Queue<string> pathlist = new Queue<string>(p);
                JToken nowdir = Dir.Intodir(file, pathlist);
                if (nowdir == null)
                    return BadRequest(JsonConvert.SerializeObject(new ReturnMode() { Data = "文件路径错误", Message = "Error" }));
                if ((table = SQLControl.Select($"SELECT * FROM CDNABASE.HashTable where ID={nowdir["data"]};")) == null)
                    return StatusCode(500, JsonConvert.SerializeObject(new ReturnMode() { Data = "数据库错误", Message = "Error" }));
                string filepath = table.Rows[0][2].ToString();
                bool k = false;
                CDNAFileStream filestream = null;
                foreach (string key in streamtable.Keys)
                    if (key == filepath)
                    {
                        k = true;
                        filestream = streamtable[key];
                        break;
                    }
                if (!k)
                {
                    filestream = new CDNAFileStream(filepath, FileMode.Open);
                    streamtable.Add(filepath, filestream);
                }
                long length = filestream.Length;
                byte[] data = new byte[length];
                for (long i = 0; i < filestream.Length; i++)
                {
                    while (!filestream.ISREAD) ;
                    filestream.ISREAD = false;
                    long nowpo = filestream.Position;
                    filestream.Seek(i, SeekOrigin.Begin);
                    data[i] = (byte)filestream.ReadByte();
                    filestream.Seek(nowpo, SeekOrigin.Begin);
                    filestream.ISREAD = true;
                }
                return File(data, "application/octet-stream", p[p.Length - 1]);
            }
            else
                return BadRequest(JsonConvert.SerializeObject(new ReturnMode() { Data = "Token错误", Message = "Error" }));
        }
    }
}