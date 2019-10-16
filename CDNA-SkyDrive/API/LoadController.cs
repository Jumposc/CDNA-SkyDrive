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
                string token = Request.Cookies["Token"];
                string[] p = Request.Headers["Path"].ToString().Split('/');
                //string[] p = "./A/".Split('/');
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
                            JObject jo = new JObject();
                            jo.Add("time", DateTime.Now.ToString("yyyy-MM-dd"));
                            jo.Add("name", file.FileName);
                            jo.Add("type", "file");
                            jo.Add("data", fileID);
                            JToken newdir = Dir.AddJson(filestr, pathlist, JToken.Parse(jo.ToString()));

                            if (0 == SQLControl.Execute($"UPDATE testbase.UserFileTable SET File='{newdir.ToString()}' where UserName='{name}';"))
                                throw new NewSqlException();
                            return Ok(JsonConvert.SerializeObject(new ReturnMode() { Data = "保存完成！", Message = "OK" }));
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
                }
                else
                    return BadRequest(JsonConvert.SerializeObject(new ReturnMode() { Data = "Token错误", Message = "Error" }));
                return Ok("程序运行完成");
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