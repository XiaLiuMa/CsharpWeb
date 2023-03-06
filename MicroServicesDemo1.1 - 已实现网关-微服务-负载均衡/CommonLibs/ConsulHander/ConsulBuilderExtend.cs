using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsulHander
{
    /// <summary>
    /// IApplicationBuilder的Consul扩展
    /// </summary>
    public static class ConsulBuilderExtend
    {
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime, ConsulOption option)
        {
            var consulClient = new ConsulClient(x => x.Address = new Uri(option.ConsulAddress));
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务器启动5秒后注册
                Interval = TimeSpan.FromSeconds(30),//每30s检测一次（健康检查间隔时间）
                HTTP = option.HealthCheckUrl,//本服务健康检查地址
                Timeout = TimeSpan.FromSeconds(20),
            };
            var registerAgent = new AgentServiceRegistration()
            {
                Check = httpCheck,
                Checks = new[] { httpCheck },
                ID = option.ServiceId,//一定要指定服务ID，否则每次都会创建一个新的服务节点
                Name = option.ServiceName,
                Address = option.ServiceIP,
                Port = option.ServicePort,
                Tags = new[] { $"urlprefix-/{option.ServiceName}" }//添加 urlprefix-/servicename 格式的tag标签，以便Fabio识别
            };
            consulClient.Agent.ServiceRegister(registerAgent).Wait();//服务启动时注册，使用Consul API进行注册（HttpClient发起）
            lifetime.ApplicationStopped.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registerAgent.ID).Wait();//服务器停止时取消注册
            });
            return app;
        }
    }
}
