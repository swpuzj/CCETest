using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CCE.Models;
using CCETest.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CCETest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public IEnumerable<string> Get()
        {
            return Summaries;
        }

        private List<SysUser> SysUsers = new List<SysUser> 
        { 
            new SysUser
            {
                UserName="admin",
                UserId="001",
                PassWord="admin"
            }
        };

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [HttpGet("login/{name}/{password}")]
        public async Task<IActionResult> Login(string name, string password)
        {
            var user = SysUsers.FirstOrDefault(u=>u.UserName==name);
            if (user != null)
            {
                user.AuthenticationType = "MyCookieAuthenticationScheme";// CookieAuthenticationDefaults.AuthenticationScheme;
                var identity = new ClaimsIdentity(user.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserId));
                identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                identity.AddClaim(new Claim(ClaimTypes.Role, "User"));

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = false,
                    IssuedUtc = DateTimeOffset.UtcNow,
                };

                await HttpContext.SignInAsync("MyCookieAuthenticationScheme", new ClaimsPrincipal(identity));
                return Ok("身份验证成功");
            }
            else
            {
                return Ok(500);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("set")]
        public IEnumerable<string> Set(string req)
        {
            var rng = new Random();
            return Summaries;
          
        }
    }
}
