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
        /// ���ö���ʵ��
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// ���캯��ע�����ö���
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// ����ע��������ʵ��
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
        /// ����Http����Ĺܵ�
        /// </summary>
        /// <param name="app">Ӧ�ó���������</param>
        /// <param name="env">Web��������</param>
        /// <param name="lifetime">����Ӧ�ó������������</param>
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

            //app.UseAuthentication();//��֤

            app.UseAuthorization();//��Ȩ

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
        }
    }
}
