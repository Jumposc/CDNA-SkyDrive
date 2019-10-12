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
        [HttpPost()]
        public IActionResult UpLoad(IList<IFormFile> files)
        {
            if (files != null)
            {
                //处理多文件
                foreach (var file in files)
                {
                    //如果需要对文件处理,可以根据文件扩展名,进行筛选
                    var fileExtensionName = Path.GetExtension(file.FileName).Substring(1);
                    var saveFilePath = Path.Combine("", "UploadFile", $"{DateTime.Now.ToString("yyyyMMddhhmmss")}.{fileExtensionName}");
                    var stream = new FileStream(saveFilePath, FileMode.Create);
                    //asp.net core对异步支持很好,如果使用异步可以使用file.CopyToAsync方法
                    file.CopyTo(stream);
                }
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