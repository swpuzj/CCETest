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
    public class HomeController : Controller
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing1", "Bracing2", "Chilly3", "Cool4", "Mild5", "Warm6", "Balmy7", "Hot", "Sweltering", "Scorching"
        };

        private readonly LogHelper _logger=LogHelper.CreateLogger<HomeController>();

        public HomeController()
        {
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public IEnumerable<string> Get()
        {
            _logger.Info("aaaa");
            _logger.Info(Summaries.Count().ToString());
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
            var user = SysUsers.FirstOrDefault(u=>u.UserName==name&&u.PassWord.Equals(password));
            if (user != null)
            {
                user.AuthenticationType = "authentication";// CookieAuthenticationDefaults.AuthenticationScheme;
                var identity = new ClaimsIdentity(user.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserId));
                identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                identity.AddClaim(new Claim(ClaimTypes.Role, "User"));


                DateTimeOffset tokenExp = GetExpDateTime(1800);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = false,
                    IssuedUtc = DateTimeOffset.UtcNow,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
                };

                await HttpContext.SignInAsync("MyAuthentication", new ClaimsPrincipal(identity), authProperties);
                _logger.Info("验证ok");
                return Ok("身份验证成功");
            }
            else
            {
                return Ok(500);
            }
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("signout")]
        public async Task<ActionResult> LoginOut()
        {
            await HttpContext.SignOutAsync("MyAuthentication");
            return Ok("退出成功");
        }

        private DateTimeOffset GetExpDateTime(int exp)
        {
            return new DateTimeOffset(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                .AddSeconds(exp)
                .ToLocalTime();
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("test")]
        public async Task<string> Test()
        {
            _logger.Info("获取数据成功。");
            return "角色:Admin，获取数据成功";
        }
    }
}
