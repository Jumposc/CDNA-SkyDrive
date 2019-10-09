using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

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

        [HttpPost]
        public string Post([FromForm] string name, [FromForm] string pwds)
        {
            //if (!((name.Length > 6 && name.Length < 20) && (pwds.Length > 6 && pwds.Length < 20)))
            //    return "ERROR";
            //MySqlConnection connection = new MySqlConnection("");
            //MySqlCommand command = new MySqlCommand("", connection);
            //connection.Close();
            return new Random().Next().ToString();
        }
    }
}