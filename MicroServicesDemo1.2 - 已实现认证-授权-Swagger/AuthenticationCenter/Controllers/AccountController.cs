using AuthenticationCenter.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationCenter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 登入【采用Jwt认证，返回Token】
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Login")]
        public async Task<object> Login(string userName, string pwd)
        {
            //假设这里已成功验证登录有效性。。。。

            //登陆成功后，基于当前用户生成JWT令牌字符串
            JwtUserInfo jwtUserInfo = new JwtUserInfo { Uid = 1, Role = "Admin,Leader" };
            string jwtStr = JwtHandler.IssueJwt(jwtUserInfo);

            return Ok(new { success = true, token = jwtStr });
        }

        /// <summary>
        /// 登出【采用Jwt认证是不支持强制Token失效的，只能客户端删除Token，服务端等待Token超时】
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            return Ok(new { success = true});
        }
    }
}
