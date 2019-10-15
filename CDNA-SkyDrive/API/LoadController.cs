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
                            if ((fileID = SQLControl.Select($"SELECT * FROM testbase.HashTable where Hash = @hash;", hashParameter)) != -1)
                            {
                                string ID = token.Split("-")[0];
                                ID = ID.Substring(0, ID.Length - 10);
                                string name = SQLControl.Select($"SELECT * FROM testbase.UserTable where  ID={ID};").Rows[0][1].ToString();
                                JObject filestr = JObject.Parse(SQLControl.Select($"SELECT * FROM testbase.UserFileTable where UserName='{name}';").Rows[0][1].ToString());
                                filestr.Add(file.FileName, fileID);
                                if (SQLControl.Insert($"insert testbase.UserFileTable (File) value ('{filestr.ToString()}');") != 0)
                                { }
                            }
                            else
                            {
                                string filename = DateTime.Now.ToString("yyyyMMddhhmmss");
                                System.IO.File.Exists(FilePath + filename);
                                MySqlParameter blobParameter = new MySqlParameter("@hash", MySqlDbType.TinyBlob);
                                blobParameter.Value = hash;
                                if (SQLControl.Insert($"insert testbase.HashTable value (0,@hash,'{FilePath + filename}');", blobParameter) != 0)
                                {

                                }
                            }

                        }
                    }
                    catch (Exception e)
                    {
                        if (stream != null)
                            stream.Close();
                        if (System.IO.File.Exists(saveFilePath))
                            System.IO.File.Delete(saveFilePath);
                    }
                }
                else
                {
                }
                return Ok();
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