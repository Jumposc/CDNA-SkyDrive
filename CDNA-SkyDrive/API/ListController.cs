using CDNA_SkyDrive.Control;
using CDNA_SkyDrive.Mode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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