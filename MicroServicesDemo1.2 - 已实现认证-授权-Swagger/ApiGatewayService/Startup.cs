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
    ///// 1��Ocelot �ṩ���ع���
    ///// 2��Ocelot.Provider.Polly �ṩ��������
    ///// 3��Ocelot.Provider.Consul �ṩ������
    ///// 4��Ocelot.Cache.CacheManager �ṩ���湦�ܡ�
    ///// </summary>
    //public class Startup
    //{
    //    /// <summary>
    //    /// ���ö���ʵ��
    //    /// </summary>
    //    public IConfiguration Configuration { get; }

    //    /// <summary>
    //    /// ���캯��ע������ʵ��
    //    /// </summary>
    //    /// <param name="configuration"></param>
    //    public Startup(IConfiguration configuration)
    //    {
    //        Configuration = configuration;
    //    }

    //    /// <summary>
    //    /// ����ע���������ʵ��
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
    //        services.AddOcelot();// ע��Ocelot����
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
    /// 1��Ocelot �ṩ���ع���
    /// 2��Ocelot.Provider.Polly �ṩ��������
    /// 3��Ocelot.Provider.Consul �ṩ������
    /// 4��Ocelot.Cache.CacheManager �ṩ���湦�ܡ�
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// ���ö���ʵ��
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// ���캯��ע������ʵ��
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// ����ע���������ʵ��
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //Ocelot��Consul��ע��
            services.AddOcelot(new ConfigurationBuilder().AddJsonFile("ocelot.json").Build()).AddConsul();

            #region ���Swagger֧��
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiGatewayService", Version = "v1", Description = "API���ط���" });

                #region ����Swagger֧���ֶ�¼��Token����(1������С����2����header�����token���ݵ���̨��)
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) ֱ�����¿�������Bearer {token}��ע��ո�\"",
                    Name = "Authorization",//jwtĬ�ϵĲ�������
                    In = ParameterLocation.Header,//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
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

            app.UseAuthentication();//��֤

            app.UseAuthorization();//��Ȩ

            #region ���ڲ�����Ȩ
            //app.AddAuthorization(options =>
            //{
            //    //��Ա�����֤
            //    options.AddPolicy("MemberOnly", p =>
            //    {
            //        //p.RequireClaim("MemberCardCode");
            //        p.RequireClaim("Name");//�������ĳ��Claim��
            //    });

            //    //�������ĳ��Claim��
            //    options.AddPolicy("User", policy => policy
            //        .RequireAssertion(context => context.User.HasClaim(c => (c.Type == "EmployeeNumber" || c.Type == "Role")))
            //    );
            //    //�ۺϿ���
            //    options.AddPolicy("Employee", policy => policy
            //            .RequireRole("Admin")//��ɫ
            //            .RequireUserName("Alice")//�����֤
            //            .RequireClaim("EmployeeNumber")//�������ĳ��Claim��
            //            .Combine(commonPolicy));//�ϲ���������
            //    //�Զ������
            //    options.AddPolicy("Over18", p => p.Requirements.Add(new MinimumAgeRequirement(18)));
            //    //��Handler��֤
            //    options.AddPolicy("Anbu", p => p.Requirements.Add(new AnBuEnterRequirement()));
            //}); 
            #endregion

            #region ����Consul�м��
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

            app.UseOcelot().Wait();//ʹ��Ocelot
        }
    }
}
