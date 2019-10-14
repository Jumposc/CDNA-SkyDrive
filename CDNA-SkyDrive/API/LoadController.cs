using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CDNA_SkyDrive.Control;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CDNA_SkyDrive.Mode;
using Newtonsoft.Json;

namespace CDNA_SkyDrive.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoadController : ControllerBase
    {
        [HttpPost()]
        [Route("Up")]
        //[DisableFormValueModelBinding]
        //[RequestFormLimits(MultipartBodyLengthLimit = 200 * 1024 * 1024)]
        public async Task<IActionResult> UpLoad()
        {
            return await (Task.Run(() =>
            {
                var files = Request.Form.Files;
                if (files != null)
                {
                    FileStream stream = null;
                    string saveFilePath = null;
                    try
                    {
                        //处理多文件
                        foreach (var file in files)
                        {
                            //saveFilePath = Path.Combine("", "UploadFile", $"{file.FileName}");
                            //using (stream = new FileStream(saveFilePath, FileMode.Create))
                            //    file.CopyToAsync(stream);
                            Save_ReadFile.GetHash(file.OpenReadStream());
                        }
                    }
                    catch
                    {
                        if (stream != null)
                            stream.Close();
                        if (System.IO.File.Exists(saveFilePath))
                            System.IO.File.Delete(saveFilePath);
                    }
                    return Ok();
                }
                else
                {
                    return StatusCode(500);
                }
            }));

        }

        [HttpPost()]
        public async Task<IActionResult> DownLoad()
        {
            var stream = new FileStream("Resources.txt", FileMode.Open);
            stream.Seek(0, SeekOrigin.Begin);
            return await Task.Run(() => { return File(stream, "text/plain", "file.json"); });
        }
    }
}