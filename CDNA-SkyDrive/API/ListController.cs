using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using CDNA_SkyDrive.Control;
using MySql.Data.MySqlClient;
using System.Data;
using CDNA_SkyDrive.Mode;

namespace CDNA_SkyDrive.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListController : ControllerBase
    {
        [HttpPost]
        public IActionResult PostList()
        {
            string Json = "";
            string a = new StreamReader(HttpContext.Request.Body).ReadToEnd();
            if (Token.CheckToken(a))
            {
                string ID = a.Split("-")[0];
                ID = ID.Substring(0, ID.Length - 10);
                string name = SQLControl.Select($"SELECT * FROM testbase.UserTable where  ID={ID};").Rows[0][1].ToString();
                string file = SQLControl.Select($"SELECT * FROM testbase.UserFileTable where UserName='{name}';").Rows[0][1].ToString();
                Json = JsonConvert.SerializeObject(new ReturnMode() { Data = file, Message = "OK" });
                return Ok(Json);
            }
            else
                return BadRequest(JsonConvert.SerializeObject(new ReturnMode() { Data = "Token错误", Message = "Error" }));
        }
    }
}