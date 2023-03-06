using ConsulHander;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway
{
    #region MyRegion
    ///// <summary>
    ///// 1、Ocelot 提供网关功能
    ///// 2、Ocelot.Provider.Polly 提供服务治理
    ///// 3、Ocelot.Provider.Consul 提供服务发现
    ///// 4、Ocelot.Cache.CacheManager 提供缓存功能。
    ///// </summary>
    //public class Startup
    //{
    //    /// <summary>
    //    /// 配置对象实例
    //    /// </summary>
    //    public IConfiguration Configuration { get; }

    //    /// <summary>
    //    /// 构造函数注入配置实例
    //    /// </summary>
    //    /// <param name="configuration"></param>
    //    public Startup(IConfiguration configuration)
    //    {
    //        Configuration = configuration;
    //    }

    //    /// <summary>
    //    /// 配置注入服务容器实例
    //    /// </summary>
    //    /// <param name="services"></param>
    //    public void ConfigureServices(IServiceCollection services)
    //    {
    //        services.AddControllers();

    //        services.AddSwaggerGen(c =>
    //        {
    //            c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiGateway", Version = "v1" });
    //        });

    //        //services.AddOcelot(Configuration).AddConsul();
    //        services.AddOcelot();// 注入Ocelot服务
    //    }

    //    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    //    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    //    {
    //        if (env.IsDevelopment())
    //        {
    //            app.UseDeveloperExceptionPage();
    //            app.UseSwagger();
    //            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiGateway v1"));
    //        }

    //        app.UseHttpsRedirection();

    //        app.UseRouting();

    //        //app.UseAuthorization();

    //        app.UseEndpoints(endpoints =>
    //        {
    //            endpoints.MapControllers();
    //        });

    //        app.UseOcelot().Wait();
    //    }
    //} 
    #endregion

    /// <summary>
    /// 1、Ocelot 提供网关功能
    /// 2、Ocelot.Provider.Polly 提供服务治理
    /// 3、Ocelot.Provider.Consul 提供服务发现
    /// 4、Ocelot.Cache.CacheManager 提供缓存功能。
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 配置对象实例
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 构造函数注入配置实例
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 配置注入服务容器实例
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //Ocelot和Consul的注入
            services.AddOcelot(new ConfigurationBuilder().AddJsonFile("ocelot.json").Build()).AddConsul();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            app.UseOcelot().Wait();//使用Ocelot
        }
    }
}
