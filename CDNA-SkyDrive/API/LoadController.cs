﻿using System;
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
using System.Text;

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
                                if (!Save_ReadFile.SaveFile(FilePath + filename, file))
                                    throw new IOException();
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
                            Queue<string> pathlist = new Queue<string>(p);
                            JObject jo = new JObject();
                            Encoding encoding = Encoding.Default;
                            jo.Add("time", DateTime.Now.ToString("yyyy-MM-dd"));
                            string s = Encoding.UTF8.GetString(Encoding.Default.GetBytes(file.FileName));
                            jo.Add("name", Encoding.UTF8.GetString(Encoding.Default.GetBytes(file.FileName)));
                            jo.Add("type", "file");
                            jo.Add("data", fileID);
                            JToken newdir = Dir.AddJson(filestr, pathlist, JToken.Parse(jo.ToString()));

                            if (0 == SQLControl.Execute($"testbase.UserFileTable SET File='' where UserName='{name}';") && 0 == SQLControl.Execute($"UPDATE testbase.UserFileTable SET File='{newdir.ToString()}' where UserName='{name}';"))
                                throw new NewSqlException();
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
                return Ok(JsonConvert.SerializeObject(new ReturnMode() { Data = "保存完成！", Message = "OK" }));
            }));

        }

        [HttpPost()]
        [Route("Down")]
        public IActionResult DownLoad()
        {
            string token = Request.Cookies["Token"];
            //string[] p = Request.Headers["Path"].ToString().Split('/');
            string[] p = "./ALL01UMD.sav".Split('/');
            if (Token.CheckToken(token))
            {
                string ID = token.Split("-")[0];
                ID = ID.Substring(0, ID.Length - 10);
                DataTable table;
                if ((table = SQLControl.Select($"SELECT * FROM testbase.UserTable where  ID={ID};")) == null)
                    return StatusCode(500, JsonConvert.SerializeObject(new ReturnMode() { Data = "数据库错误", Message = "Error" }));
                string name = table.Rows[0][1].ToString();
                if ((table = SQLControl.Select($"SELECT * FROM testbase.UserFileTable where UserName='{name}';")) == null)
                    return StatusCode(500, JsonConvert.SerializeObject(new ReturnMode() { Data = "数据库错误", Message = "Error" }));
                JToken file = JToken.Parse(table.Rows[0][1].ToString());
                Queue<string> pathlist = new Queue<string>(p);
                JToken nowdir = Dir.Intodir(file, pathlist);
                if ((table = SQLControl.Select($"SELECT * FROM testbase.HashTable where ID={nowdir["data"]};")) == null)
                    return StatusCode(500, JsonConvert.SerializeObject(new ReturnMode() { Data = "数据库错误", Message = "Error" }));
                string filepath = table.Rows[0][2].ToString();
                return File(new FileStream(filepath, FileMode.Open), "application/octet-stream", p[p.Length - 1]);
            }
            else
                return BadRequest(JsonConvert.SerializeObject(new ReturnMode() { Data = "Token错误", Message = "Error" }));
        }
    }
}