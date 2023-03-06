using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ApiGateway.Controllers
{
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
            return Json($"ApiGateway {_configuration["urls"]} Test01");
        }
    }
}
