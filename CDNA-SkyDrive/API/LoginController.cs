using CDNA_SkyDrive.Control;
using CDNA_SkyDrive.Mode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.IO;

namespace CDNA_SkyDrive.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost()]
        public string Post()
        {
            string Json = "";
            string a = new StreamReader(HttpContext.Request.Body).ReadToEnd();
            UserMode user = JsonConvert.DeserializeObject<UserMode>(a);
            if (!string.IsNullOrEmpty(user.Name) | !string.IsNullOrWhiteSpace(user.Name) | !string.IsNullOrEmpty(user.Pwds) | !string.IsNullOrWhiteSpace(user.Pwds))
            {
                DataTable s = SQLControl.Select($"SELECT * FROM testbase.UserTable where  UserName='{user.Name}'and PassWord='{user.Pwds}';");
                if (s.Rows.Count != 0)
                    Json = JsonConvert.SerializeObject(new ReturnMode() { Data = Token.GetToken(s.Rows[0][0].ToString()), Message = "OK" });
                else
                    Json = JsonConvert.SerializeObject(new ReturnMode() { Data = "用户名密码错误", Message = "Error" });
            }
            else
                Json = JsonConvert.SerializeObject(new ReturnMode() { Data = "空！", Message = "Error" });
            return Json;
        }
    }
}