using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CDNA_SkyDrive.Control;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CDNA_SkyDrive.Mode;
using Newtonsoft.Json.Linq;
using System.Data;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace CDNA_SkyDrive.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoadController : ControllerBase
    {
        const string FilePath = "UpLoadFile/";
        [HttpPost()]
        [Route("Up")]
        //[DisableFormValueModelBinding]
        //[RequestFormLimits(MultipartBodyLengthLimit = 200 * 1024 * 1024)]
        public async Task<IActionResult> UpLoad()
        {
            return await (Task.Run(() =>
            {
                int Code = 200;
                string restr = "Ok";
                string token = Request.Cookies["Token"];
                //string[] p = new StreamReader(Request.Body).ReadToEnd().Split('/');
                string[] p = "./A/".Split('/');
                Queue<string> pathlist = null;
                var files = Request.Form.Files;
                if (Token.CheckToken(token) && files != null)
                {
                    Stream stream = null;
                    string saveFilePath = null;
                    try
                    {
                        foreach (var file in files)
                        {
                            stream = file.OpenReadStream();
                            byte[] hash = Save_ReadFile.GetHash(stream);

                            MySqlParameter hashParameter = new MySqlParameter("@hash", MySqlDbType.TinyBlob);
                            hashParameter.Value = hash;
                            int fileID = 0;
                            //检查Hash表里是否有这个文件
                            if (-1 == (fileID = SQLControl.Select($"SELECT * FROM testbase.HashTable where Hash = @hash;", hashParameter)))
                            {//没有就加进去
                                string filename = DateTime.Now.ToString("yyyyMMddhhmmss");
                                System.IO.File.Exists(FilePath + filename);
                                MySqlParameter blobParameter = new MySqlParameter("@hash", MySqlDbType.TinyBlob);
                                blobParameter.Value = hash;
                                if (0 == SQLControl.Execute($"insert testbase.HashTable value (0,@hash,'{FilePath + filename}');", blobParameter))
                                    throw new NewSqlException();
                                fileID = SQLControl.Select($"SELECT * FROM testbase.HashTable where Hash = @hash;", blobParameter);
                            }
                            //把Hash绑定到用户文件列表上
                            string ID = token.Split("-")[0];
                            ID = ID.Substring(0, ID.Length - 10);

                            DataTable table;
                            if ((table = SQLControl.Select($"SELECT * FROM testbase.UserTable where  ID={ID};")) == null)
                                throw new NewSqlException();
                            string name = table.Rows[0][1].ToString();
                            if ((table = SQLControl.Select($"SELECT * FROM testbase.UserFileTable where UserName='{name}';")) == null)
                                throw new NewSqlException();

                            JArray filestr = JArray.Parse(table.Rows[0][1].ToString());
                            pathlist = new Queue<string>(p);
                            JToken fir = JToken.Parse("{time:\"2019 - 10 - 15\",name: \"A\",type: \"dir\",data:[]}");
                            JToken nowdir = Dir.AddDir(filestr, pathlist, fir);
                            //JObject filedata = new JObject();
                            //filedata["FileID"] = ID;
                            //filedata["Time"] = DateTime.Now.ToString();
                            filestr[file.FileName] = fileID;
                            if (0 != SQLControl.Execute($"UPDATE testbase.UserFileTable SET (File='{filestr.ToString()}')where UserName='{name}';"))
                            {
                            }
                        }
                    }
                    catch (IOException)
                    {
                        Code = 500;
                        restr = JsonConvert.SerializeObject(new ReturnMode() { Data = null, Message = "服务器保存错误！" });
                        if (stream != null)
                            stream.Close();
                        if (System.IO.File.Exists(saveFilePath))
                            System.IO.File.Delete(saveFilePath);
                    }
                    catch (NewSqlException)
                    {
                        return StatusCode(500, JsonConvert.SerializeObject(new ReturnMode() { Data = "数据库错误", Message = "Error" }));
                    }
                }
                else
                {
                }
                return StatusCode(Code, restr);
            }));

        }

        [HttpPost()]
        public async Task<IActionResult> DownLoad()
        {
            var stream = new FileStream("Resources.txt", FileMode.Open);
            stream.Seek(0, SeekOrigin.Begin);
            return await Task.Run(() => { return File(stream, "text/plain", "file.json"); });
        }
    }
}