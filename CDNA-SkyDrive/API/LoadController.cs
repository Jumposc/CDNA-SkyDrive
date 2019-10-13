using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CDNA_SkyDrive.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoadController : ControllerBase
    {
        [Route("Up")]
        public IActionResult UpLoad()
        {
            var files = Request.Form.Files;
            if (files != null)
            {
                try
                {
                    //处理多文件
                    foreach (var file in files)
                    {
                        var saveFilePath = Path.Combine("", "UploadFile", $"{file.FileName}");
                        using (var stream = new FileStream(saveFilePath, FileMode.Create))
                        {
                            file.CopyToAsync(stream);
                        }
                    }
                }
                catch { }
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpPost()]
        public IActionResult DownLoad()
        {
            var stream = new FileStream("Resources.txt", FileMode.Open);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "text/plain", "file.json");
        }
    }
}