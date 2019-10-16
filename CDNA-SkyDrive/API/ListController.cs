using CDNA_SkyDrive.Control;
using CDNA_SkyDrive.Mode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;
using System.IO;

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
            string[] p = new StreamReader(Request.Body).ReadToEnd().Split('/');
            Queue<string> path = new Queue<string>();
            foreach (string i in p)
                path.Enqueue(i);
            string a = Request.Cookies["Token"];
            if (Token.CheckToken(a))
            {
                string ID = a.Split("-")[0];
                ID = ID.Substring(0, ID.Length - 10);
                DataTable table;
                if ((table = SQLControl.Select($"SELECT * FROM testbase.UserTable where  ID = {ID};")) == null)
                    return StatusCode(500, JsonConvert.SerializeObject(new ReturnMode() { Data = "数据库错误", Message = "Error" }));
                string name = table.Rows[0][1].ToString();
                table = null;
                if ((table = SQLControl.Select($"SELECT * FROM testbase.UserFileTable where UserName = '{name}';")) == null)
                    return StatusCode(500, JsonConvert.SerializeObject(new ReturnMode() { Data = "数据库错误", Message = "Error" }));
                JToken file = JToken.Parse(table.Rows[0][1].ToString());
                JToken nowdir = Dir.Intodir(file, path);
                Json = JsonConvert.SerializeObject(new ReturnMode() { Data = nowdir.ToString(), Message = "OK" });
                return Ok(Json);
            }
            else
                return BadRequest(JsonConvert.SerializeObject(new ReturnMode() { Data = "Token错误", Message = "Error" }));
        }
    }
}