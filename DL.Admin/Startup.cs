using Autofac;
using DL.Utils.Auth;
using DL.Utils.Autofac;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NLog.Extensions.Logging;

namespace DL.Admin
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
               .AddEnvironmentVariables();//��ȡϵͳ�Ļ���������Ϣ,����֮ǰ��������Ϣ

            Configuration = configuration;
            Env = env;

            //Log4net�����ļ�
            ILoggerRepository Repository = LogManager.CreateRepository("Log4net");
            XmlConfigurator.Configure(Repository, new FileInfo(Path.Combine(env.ContentRootPath, "log4net.config")));


        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())

            services.AddMemoryCache()//�ڴ滺��
                 .AddLogging(m =>
                 {
                     m.AddNLog();
                 })
                .AddResponseCompression()
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddSingleton<IMemoryCache>()
                .AddCors(o =>
                {//CORS Cross-Origin-Resource-Sharing ����:��Դ��Դ����ͬԴ���ԣ� 
                    o.AddPolicy("Limit", policy =>
                     {
                         policy.WithOrigins("http://localhost:2020", "http://127.0.0.1:2020")
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                     });

                })
                .AddAuthorization(o =>
                {//��Ȩ
                    o.AddPolicy("AdminPolicy", policy => policy.RequireRole("AdminRole").Build());
                })
                .AddAuthentication(o =>
                {
                    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;//ʹ��Cookie��Ϊ��֤�û��ķ���
                    o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;//��Ҫ�û���¼ʱ��ִ�еķ���
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
                {//ʹ��Cookie�ķ�ʽ�����õ�¼ҳ��,�ǳ�ҳ���û��Ȩ��ʱ����תҳ��
                    //o.LoginPath = new PathString("/Home/Login");
                    o.LoginPath = new PathString("/Sys/Menu/Index");
                    o.LogoutPath = new PathString("/Login/Logout");
                    o.AccessDeniedPath = new PathString("/Error");
                    o.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    o.Cookie.HttpOnly = true;//����XSS����
                    o.Cookie.Name = "TokenCookieName";
                    //o.Cookie.Domain = ".cg.com";//Cookieͬ����
                })
                .AddJwtBearer("JwtAuthenticationScheme", o =>
                {
                    var keyByteArray = Encoding.ASCII.GetBytes(GlobalKey.JWTSecretKey);
                    var signingKey = new SymmetricSecurityKey(keyByteArray);
                    var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

                    o.TokenValidationParameters = new TokenValidationParameters
                    {//������֤����
                        IssuerSigningKey = signingKey,//ǩ����Կ
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = GlobalKey.Issuer,
                        ValidAudience = GlobalKey.Audience,//�䷢��˭
                        ValidateLifetime = true,// �Ƿ���֤Token��Ч�ڣ�ʹ�õ�ǰʱ����Token��Claims�е�NotBefore��Expires�Ա�
                        ClockSkew = TimeSpan.FromSeconds(300),// ����ķ�����ʱ��ƫ����
                        RequireExpirationTime = true,// �Ƿ�Ҫ��Token��Claims�б������Expires
                        ValidateIssuerSigningKey = true,
                    };
                    o.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {//��֤ʧ��ʱ����

                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {// ������ڣ����<�Ƿ����>��ӵ�������ͷ��Ϣ��
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddControllersWithViews()
                //.AddJsonOptions(o =>
                //{//΢������Test.json ���л����ı������� ����֧�� ǰ������Ĳ������͸���˽��ܵĲ������Ͷ�Ӧ
                //    o.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                //    o.JsonSerializerOptions.ReadCommentHandling = new System.Text.Json.JsonCommentHandling();//����ѭ������
                //    o.JsonSerializerOptions.PropertyNamingPolicy = null;// json ����ֵ������ʹ�����ַ�Сд�� CamelCase ���������
                //    //o.JsonSerializerOptions.PropertyNameCaseInsensitive = true;//��ֵȷ�������л��ڼ����������Ƿ�ʹ�ò����ִ�Сд�ıȽϡ�Ĭ��ֵΪfalse
                //})
                .AddNewtonsoftJson(o =>
                {
                    o.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    o.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

                    //o.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    //o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                .AddControllersAsServices().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

        }

        /// <summary>
        /// ����κ�Autofacģ���ע�ᡣ
        /// ������ConfigureServices֮����õģ�����
        /// �ڴ˴�ע�Ὣ������ConfigureServices��ע������ݡ�
        /// �ڹ�������ʱ������á�UseServiceProviderFactory��new AutofacServiceProviderFactory��������`���򽫲�����ôˡ�
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModuleRegister(ApplicationEnvironment.ApplicationBasePath,
               new List<string>()
               { //�������캯��ע��
                    "DL.Service.dll"
               }));

            builder.RegisterBuildCallback(o =>
            {//ȫ��ʹ��Container����ע���жϵ�
                AutofacHelper.Container = o.BeginLifetimeScope();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHost host)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }


            app.UseCors("Limit")
               .UseResponseCompression()
               .UseCookiePolicy()
               .UseStaticFiles()
               .UseRouting()
               .UseAuthentication()
               .UseAuthorization()
               .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Login}/{action=Index}/{id?}");

                    endpoints.MapAreaControllerRoute(
                       name: "areas", "areas",
                       pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                    //endpoints.MapRazorPages();
                });
        }
    }
}
