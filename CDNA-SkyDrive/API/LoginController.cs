using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CDNA_SkyDrive.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "OK";
        }

        [HttpPost]
        public async Task<string> Post([FromForm] string name, [FromForm] string pwds)
        {
            return "token";
        }
    }
}