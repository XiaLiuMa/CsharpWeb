using System;

namespace ConsulHander
{
    /// <summary>
    /// Consul配置
    /// </summary>
    public class ConsulOption
    {
        /// <summary>
        /// 服务唯一ID
        /// </summary>
        public string ServiceId { get; private set; } = Guid.NewGuid().ToString("N");
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 服务部署的IP
        /// </summary>
        public string ServiceIP { get; set; }
        /// <summary>
        /// 服务部署的端口
        /// </summary>
        public int ServicePort { get; set; }
        /// <summary>
        /// 健康检查服务地址
        /// </summary>
        public string HealthCheckUrl { get; set; }
        /// <summary>
        /// Consul部署的url地址
        /// </summary>
        public string ConsulAddress { get; set; }
    }
}
