using CDNA_SkyDrive.Control;
using CDNA_SkyDrive.Mode;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;

namespace CDNA_SkyDrive.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddDirController : ControllerBase
    {
        [HttpPost()]
        public IActionResult AddDir()
        {
            string token = Request.Cookies["Token"];
            string[] p = Request.Headers["Path"].ToString().Split('/');
            string dirname = Request.Headers["DirName"].ToString();
            string name = "";
            JArray filestr = null;
            Queue<string> q = new Queue<string>(p);
            DataTable table = null;
            try
            {
                if (Token.CheckToken(token))
                {
                    string ID = token.Split("-")[0];
                    ID = ID.Substring(0, ID.Length - 10);
                    if ((table = SQLControl.Select($"SELECT * FROM CDNABASE.UserTable where  ID={ID};")) == null)
                        throw new NewSqlException();
                    name = table.Rows[0][1].ToString();
                    if ((table = SQLControl.Select($"SELECT * FROM CDNABASE.UserFileTable where UserName='{name}';")) == null)
                        throw new NewSqlException();
                    do
                    {
                        table = null;
                        if ((table = SQLControl.Select($"SELECT * FROM CDNABASE.UserFileTable where UserName='{name}';")) == null)
                            throw new NewSqlException();
                    } while (int.Parse(table.Rows[0][2].ToString()) != 1);
                    filestr = JArray.Parse(table.Rows[0][1].ToString());
                    JObject job = new JObject();
                    job["time"] = DateTime.Now.ToString("yyyy-MM-dd");
                    job["name"] = dirname;
                    job["type"] = "dir";
                    job["data"] = "[]";
                    JToken newdir = Dir.AddJson(filestr, q, JToken.Parse(job.ToString()));

                    SQLControl.Execute($"UPDATE CDNABASE.UserFileTable SET File='' , State = 0 where UserName='{name}';");
                    SQLControl.Execute($"UPDATE CDNABASE.UserFileTable SET File='{newdir.ToString()}',State = 1 where UserName='{name}';");
                }
                else return BadRequest(JsonConvert.SerializeObject(new ReturnMode() { Data = "Token错误", Message = "Error" }));
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
            return Ok();
        }
    }
}