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
    public class RegisterController : ControllerBase
    {
        [HttpPost()]
        public string Register()
        {
            string Json = "";
            string a = new StreamReader(HttpContext.Request.Body).ReadToEnd();
            SingInMode user = JsonConvert.DeserializeObject<SingInMode>(a);
            if (!string.IsNullOrEmpty(user.Name) | !string.IsNullOrWhiteSpace(user.Name) | !string.IsNullOrEmpty(user.Pwds) | !string.IsNullOrWhiteSpace(user.Pwds))
            {
                if (0 != SQLControl.Execute($"insert CDNABASE.UserTable value (0 ,'{user.Name}','{user.Pwds}');"))
                {
                    if (0 != SQLControl.Execute($"insert CDNABASE.UserDataTable value ('{user.Name}','{user.PhoneNumber}','InitialImage.jpg');"))
                        if (0 != SQLControl.Execute($"insert CDNABASE.UserFileTable value ('{user.Name}','{Token.s}',1);"))
                            Json = JsonConvert.SerializeObject(new ReturnMode() { Data = "注册成功", Message = "OK" });
                        else Json = JsonConvert.SerializeObject(new ReturnMode() { Data = "注册失败，服务器错误", Message = "OK" });
                    else Json = JsonConvert.SerializeObject(new ReturnMode() { Data = "注册失败，服务器错误", Message = "OK" });
                }
                else Json = JsonConvert.SerializeObject(new ReturnMode() { Data = "注册失败，或有重复用户名", Message = "Error" });
            }
            else Json = JsonConvert.SerializeObject(new ReturnMode() { Data = "空！", Message = "Error" });
            return Json;
        }
    }
}