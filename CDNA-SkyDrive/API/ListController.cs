using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CDNA_SkyDrive.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListController : ControllerBase
    {
        [HttpPost]
        public string PostList()
        {
            FileStream file = new FileStream("Resources.txt", FileMode.Open);
            MD5 m = MD5.Create();
            byte[] buffer = m.ComputeHash(file);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                stringBuilder.Append(buffer[i].ToString("x2"));
            }
            string s = stringBuilder.ToString();
            return "";
        }
    }
}