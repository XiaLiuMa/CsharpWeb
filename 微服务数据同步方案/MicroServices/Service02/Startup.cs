using ConsulHander;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service02
{
    public class Startup
    {
        /// <summary>
        /// 配置对象实例
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 构造函数注入配置对象
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 配置注入容器的实例
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Service02", Version = "v1" });
            });
        }

        /// <summary>
        /// 配置Http处理的管道
        /// </summary>
        /// <param name="app">应用程序生成器</param>
        /// <param name="env">Web宿主环境</param>
        /// <param name="lifetime">宿主应用程序的生命周期</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Service02 v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthentication();//认证

            app.UseAuthorization();//授权

            #region 引入Consul中间件
            var consulOption = new ConsulOption()
            {
                ServiceName = Configuration["ConsulOption:ServiceName"],
                ServiceIP = Configuration["ConsulOption:ServiceIP"],
                ServicePort = Int32.Parse(Configuration["ConsulOption:ServicePort"]),
                HealthCheckUrl = Configuration["ConsulOption:HealthCheckUrl"],
                ConsulAddress = Configuration["ConsulOption:ConsulAddress"]
            };
            app.RegisterConsul(lifetime, consulOption);
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
