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
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayService
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
    //            c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiGatewayService", Version = "v1" });
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
    //            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiGatewayService v1"));
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

            #region 添加Swagger支持
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiGatewayService", Version = "v1", Description = "API网关服务" });

                #region 配置Swagger支持手动录入Token令牌(1、开启小锁；2、在header中添加token传递到后台。)
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意空格）\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });
                #endregion

                var basePath = AppContext.BaseDirectory;
                var sawgger_xml = Path.Combine(basePath, "ApiGatewayService.xml");
                c.IncludeXmlComments(sawgger_xml, true);
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiGatewayService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();//认证

            app.UseAuthorization();//授权

            #region 基于策略授权
            //app.AddAuthorization(options =>
            //{
            //    //会员身份验证
            //    options.AddPolicy("MemberOnly", p =>
            //    {
            //        //p.RequireClaim("MemberCardCode");
            //        p.RequireClaim("Name");//必须包含某个Claim项
            //    });

            //    //必须包含某个Claim项
            //    options.AddPolicy("User", policy => policy
            //        .RequireAssertion(context => context.User.HasClaim(c => (c.Type == "EmployeeNumber" || c.Type == "Role")))
            //    );
            //    //综合控制
            //    options.AddPolicy("Employee", policy => policy
            //            .RequireRole("Admin")//角色
            //            .RequireUserName("Alice")//身份验证
            //            .RequireClaim("EmployeeNumber")//必须包含某个Claim项
            //            .Combine(commonPolicy));//合并其他策略
            //    //自定义策略
            //    options.AddPolicy("Over18", p => p.Requirements.Add(new MinimumAgeRequirement(18)));
            //    //多Handler验证
            //    options.AddPolicy("Anbu", p => p.Requirements.Add(new AnBuEnterRequirement()));
            //}); 
            #endregion

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
