using AuthenticationCenter.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationCenter.Controllers
{
    /// <summary>
    /// 健康管理控制器
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class HealthController : Controller
    {
        private IConfiguration _configuration;

        public HealthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Consul健康检测
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ConsulHealth")]
        public IActionResult ConsulHealth()
        {
            Console.WriteLine($"This is HealhController {_configuration["urls"]} Invoke");
            return Ok();
        }

        /// <summary>
        /// 测试1
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Test01")]
        public IActionResult Test01()
        {
            return Json($"AuthenticationService {_configuration["urls"]} Test01");
        }

        /// <summary>
        /// 测试2
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Test02")]
        [Authorize(policy: "Admin")]
        public IActionResult Test02()
        {
            return Json($"AuthenticationService {_configuration["urls"]} Test02");
        }

        /// <summary>
        /// 测试03
        /// </summary>
        /// <param name="uid">学生账户</param>
        /// <returns>测试实体</returns>
        [HttpGet]
        [Route("Test03")]
        public TestMod Test03(string uid)
        {
            return new TestMod();
        }
    }
}
