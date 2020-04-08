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
               .AddEnvironmentVariables();//读取系统的环境变量信息,覆盖之前的配置信息

            Configuration = configuration;
            Env = env;

            //Log4net配置文件
            ILoggerRepository Repository = LogManager.CreateRepository("Log4net");
            XmlConfigurator.Configure(Repository, new FileInfo(Path.Combine(env.ContentRootPath, "log4net.config")));


        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())

            services.AddMemoryCache()//内存缓存
                 .AddLogging(m =>
                 {
                     m.AddNLog();
                 })
                .AddResponseCompression()
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddSingleton<IMemoryCache>()
                .AddCors(o =>
                {//CORS Cross-Origin-Resource-Sharing 跨域:跨源资源共享（同源策略） 
                    o.AddPolicy("Limit", policy =>
                     {
                         policy.WithOrigins("http://localhost:2020", "http://127.0.0.1:2020")
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                     });

                })
                .AddAuthorization(o =>
                {//授权
                    o.AddPolicy("AdminPolicy", policy => policy.RequireRole("AdminRole").Build());
                })
                .AddAuthentication(o =>
                {
                    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;//使用Cookie作为验证用户的方案
                    o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;//需要用户登录时候执行的方案
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
                {//使用Cookie的方式，配置登录页面,登出页面和没有权限时的跳转页面
                    //o.LoginPath = new PathString("/Home/Login");
                    o.LoginPath = new PathString("/Sys/Menu/Index");
                    o.LogoutPath = new PathString("/Login/Logout");
                    o.AccessDeniedPath = new PathString("/Error");
                    o.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    o.Cookie.HttpOnly = true;//避免XSS攻击
                    o.Cookie.Name = "TokenCookieName";
                    //o.Cookie.Domain = ".cg.com";//Cookie同域共享
                })
                .AddJwtBearer("JwtAuthenticationScheme", o =>
                {
                    var keyByteArray = Encoding.ASCII.GetBytes(GlobalKey.JWTSecretKey);
                    var signingKey = new SymmetricSecurityKey(keyByteArray);
                    var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

                    o.TokenValidationParameters = new TokenValidationParameters
                    {//令牌验证参数
                        IssuerSigningKey = signingKey,//签名秘钥
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = GlobalKey.Issuer,
                        ValidAudience = GlobalKey.Audience,//颁发给谁
                        ValidateLifetime = true,// 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                        ClockSkew = TimeSpan.FromSeconds(300),// 允许的服务器时间偏移量
                        RequireExpirationTime = true,// 是否要求Token的Claims中必须包含Expires
                        ValidateIssuerSigningKey = true,
                    };
                    o.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {//认证失败时调用

                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {// 如果过期，则把<是否过期>添加到，返回头信息中
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddControllersWithViews()
                //.AddJsonOptions(o =>
                //{//微软内置Test.json 序列化中文编码设置 ，不支持 前端请求的参数类型跟后端接受的参数类型对应
                //    o.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                //    o.JsonSerializerOptions.ReadCommentHandling = new System.Text.Json.JsonCommentHandling();//忽略循环引用
                //    o.JsonSerializerOptions.PropertyNamingPolicy = null;// json 返回值的属性使用首字符小写的 CamelCase 属性名风格
                //    //o.JsonSerializerOptions.PropertyNameCaseInsensitive = true;//该值确定反序列化期间属性名称是否使用不区分大小写的比较。默认值为false
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
        /// 添加任何Autofac模块或注册。
        /// 这是在ConfigureServices之后调用的，所以
        /// 在此处注册将覆盖在ConfigureServices中注册的内容。
        /// 在构建主机时必须调用“UseServiceProviderFactory（new AutofacServiceProviderFactory（））”`否则将不会调用此。
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModuleRegister(ApplicationEnvironment.ApplicationBasePath,
               new List<string>()
               { //批量构造函数注入
                    "DL.Service.dll"
               }));

            builder.RegisterBuildCallback(o =>
            {//全局使用Container进行注入判断等
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
