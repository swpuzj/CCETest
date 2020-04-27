using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CCE.Common;
using CCE.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CCETest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : Controller
    {
       
        private readonly LogHelper _logger=LogHelper.CreateLogger<TaskController>();


        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<string> Add(int a,int b)
        {
            _logger.Info("aaaa");
            return $"a+b={a + b}";
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("get")]
        public async Task<string> Get()
        {
            _logger.Info("获取数据成功。");
            return "角色:Admin，获取数据成功";
        }
    }
}
