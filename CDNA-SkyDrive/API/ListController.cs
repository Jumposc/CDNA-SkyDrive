using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CDNA_SkyDrive.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListController : ControllerBase
    {
        [HttpPost]
        public string PostList()
        {
            //此处查数据库用户拥有文件
            return "";
        }
    }
}