using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.IO;
using Newtonsoft.Json;
using System.Data;

namespace CDNA_SkyDrive.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "OK";
        }

        [HttpPost()]
        public string Post()
        {
            StreamReader read = new StreamReader(HttpContext.Request.Body);
            string a = read.ReadToEnd();
            User user = JsonConvert.DeserializeObject<User>(a);
            if (!string.IsNullOrEmpty(user.name) | !string.IsNullOrWhiteSpace(user.name) | !string.IsNullOrEmpty(user.pwds) | !string.IsNullOrWhiteSpace(user.pwds))
            {
                MySqlConnection connection = new MySqlConnection("server=192.168.20.2;port=3306;user=root;password=123456; database=testbase;");
                connection.Open();
                MySqlCommand command = new MySqlCommand($"SELECT UserName FROM testbase.UserTable where  UserName='{user.name}'and PassWord='{user.pwds}';", connection);
                MySqlDataReader data = command.ExecuteReader();
                while (data.Read())
                    if (data[0] != null)
                    {
                        if (data[0].Equals(user.name))
                        {
                            return JsonConvert.SerializeObject(new Return() { Token = new Random().Next().ToString(), Message = "OK" }); ;
                        }
                    }
                connection.Close();
                return new Random().Next().ToString();
            }
            return "ERROR";
        }
    }
    public class User
    {
        public string name;
        public string pwds;
    }
    public class Return
    {
        public string Token;
        public string Message;
    }
}