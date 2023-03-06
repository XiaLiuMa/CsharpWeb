using AuthenticationCenter.Core;
using ConsulHander;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationCenter
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
            #region ������֤
            services.AddAuthentication(x =>
            {
                // ��ϸ��������� ��ͼ�д������ʾ����Ǹ�
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(o =>
            {

                //��ȡ�����ļ�
                var audienceConfig = Configuration.GetSection("Audience");
                var symmetricKeyAsBase64 = audienceConfig["Secret"];
                var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
                var signingKey = new SymmetricSecurityKey(keyByteArray);

                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuer = true,
                    ValidIssuer = audienceConfig["Issuer"],//������
                    ValidateAudience = true,
                    ValidAudience = audienceConfig["Audience"],//������
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,//����ǻ������ʱ�䣬Ҳ����˵����ʹ���������˹���ʱ�䣬����ҲҪ���ǽ�ȥ������ʱ��+���壬Ĭ�Ϻ�����7���ӣ������ֱ������Ϊ0
                    RequireExpirationTime = true,
                };
            });
            #endregion

            #region ������Ȩ����
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());//������ɫ
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));//��Ĺ�ϵ
                options.AddPolicy("SystemAndAdmin", policy => policy.RequireRole("Admin").RequireRole("System"));//�ҵĹ�ϵ
            });
            #endregion

            services.AddControllersWithViews();

            #region ����Swagger֧��
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthenticationCenter", Version = "v1", Description = "��Ȩ����" });

                #region ����Swagger֧���ֶ�¼��Token����(1������С����2����header������token���ݵ���̨��)
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
                var sawgger_xml = Path.Combine(basePath, "AuthenticationCenter.xml");
                c.IncludeXmlComments(sawgger_xml, true);
            });
            #endregion
        }

        /// <summary>
        /// ����Http�����Ĺܵ�
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthenticationCenter v1"));
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();//Ĭ��ֵΪ30�죬see https://aka.ms/aspnetcore-hsts.
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseErrorHandling();//��������

            app.UseRouting();//·�ɣ����ģ�
            app.UseAuthentication();//��֤����˭��
            app.UseAuthorization();//��Ȩ���ܸ�ʲô��

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
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}