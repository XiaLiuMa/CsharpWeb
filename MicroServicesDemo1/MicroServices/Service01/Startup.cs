using ConsulHander;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Service01.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service01
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
            #region 添加认证
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>{
                //读取配置文件
                var audienceConfig = Configuration.GetSection("Audience");
                var symmetricKeyAsBase64 = audienceConfig["Secret"];
                var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
                var signingKey = new SymmetricSecurityKey(keyByteArray);

                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuer = true,
                    ValidIssuer = audienceConfig["Issuer"],//发行人
                    ValidateAudience = true,
                    ValidAudience = audienceConfig["Audience"],//订阅人
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,//缓冲时间，可以直接设置为0
                    RequireExpirationTime = true,
                };
            });
            #endregion

            #region 添加授权策略
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());//单独角色
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));//或的关系
                options.AddPolicy("SystemAndAdmin", policy => policy.RequireRole("Admin").RequireRole("System"));//且的关系
            });
            #endregion

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Service01", Version = "v1", Description = "微服务01" });

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
                var sawgger_xml = Path.Combine(basePath, "Service01.xml");
                c.IncludeXmlComments(sawgger_xml, true);
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
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Service01 v1"));

            app.UseHttpsRedirection();

            //app.UseErrorHandling();//错误助手

            app.UseRouting();//路由：在哪？
            app.UseAuthentication();//认证：是谁？
            app.UseAuthorization();//授权：能干什么？

            #region 引入Consul中间件
            var consulOption = new ConsulOption()
            {
                ServiceName = Configuration["ConsulOption:ServiceName"],
                ServiceIP = Configuration["ConsulOption:ServiceIP"],
                ServicePort =Int32.Parse( Configuration["ConsulOption:ServicePort"]),
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
