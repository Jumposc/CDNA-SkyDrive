using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CDNA_SkyDrive.Mode;
using Newtonsoft.Json;
using System.IO;
using MySql.Data.MySqlClient;
using CDNA_SkyDrive.Control;

namespace CDNA_SkyDrive.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        [HttpPost()]
        public string Register()
        {
            string Json = "";
            string a = new StreamReader(HttpContext.Request.Body).ReadToEnd();
            UserMode user = JsonConvert.DeserializeObject<UserMode>(a);
            if (!string.IsNullOrEmpty(user.Name) | !string.IsNullOrWhiteSpace(user.Name) | !string.IsNullOrEmpty(user.Pwds) | !string.IsNullOrWhiteSpace(user.Pwds))
            {
                MySqlConnection connection = new MySqlConnection(Resources.GetResources("ConnectionString"));
                connection.Open();
                MySqlCommand command = new MySqlCommand($"insert testbase.UserTable value (0,'{user.Name}','{user.Pwds}');", connection);
                if (command.ExecuteNonQuery() != 0)
                    Json = JsonConvert.SerializeObject(new ReturnMode() { Data = "注册成功", Message = "OK" });
                else
                    Json = JsonConvert.SerializeObject(new ReturnMode() { Data = "注册失败，或有重复用户名", Message = "Error" });
                connection.Close();
            }
            else
                Json = JsonConvert.SerializeObject(new ReturnMode() { Data = "空！", Message = "Error" });
            return Json;
        }
    }
}