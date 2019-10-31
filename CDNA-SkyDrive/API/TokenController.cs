using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CDNA_SkyDrive.Control;
using CDNA_SkyDrive.Mode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CDNA_SkyDrive.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        [HttpGet]
        public IActionResult CheckToken()
        {
            if (Token.CheckToken(Request.Cookies["Token"]))
                return Ok(JsonConvert.SerializeObject(new ReturnMode() { Data = "Token验证正确", Message = "OK" }));
            else
                return BadRequest(JsonConvert.SerializeObject(new ReturnMode() { Data = "Token错误", Message = "Error" }));
        }
    }
}