using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
//using DL.IService.SysIService;
using DL.Utils.Security;
using DL.Domain.PublicModels;
using DL.Utils.Cache.MemoryCache;
using DL.Admin.Models;
using DL.Domain.Dto.AdminDto.SysDto;
using DL.Utils.AppConfig;
using DL.Utils.Cache.RedisCache;
using DL.Utils.Auth.Jwt;
using DL.Utils.Auth;
using DL.Domain.Models.SysModels;
using SqlSugar;
using DL.IService.SysIService;

namespace DLSchool.Admin.Controllers
{
    public class LoginController : Controller
    {
        private readonly ISysAdminService _adminService;
        private readonly ISysLogService _logService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginController(ISysAdminService adminService,
         ISysLogService logService,
         IHttpContextAccessor httpContextAccessor)
        {
            _adminService = adminService;
            _logService = logService;
            _httpContextAccessor = httpContextAccessor;
        }

        #region 登录

        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            var auth = HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (auth.Status.ToString() != "Faulted")
            {
                RedirectToPage("Index");
            }
            ViewBag.RsaKey = RSAEncrypt.GetKey();
            //获得公钥和私钥
            MemoryCacheHelper.Set(KeyModel.LoginKey, ViewBag.RsaKey);

            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResult<string>> Login([FromBody]SysAdminLogin parm)
        {
            var apiRes = new ApiResult<string>() { statusCode = (int)ApiEnum.HttpRequestError };
            var token = "";
            try
            {
                #region 1. 从缓存获取公钥私钥解密，再解密密码

                //获得公钥私钥，解密
                var rsaKey = MemoryCacheHelper.Get<List<string>>(KeyModel.LoginKey);
                if (rsaKey == null)
                {
                    apiRes.msg = "登录失败，请刷新浏览器再次登录";
                    return apiRes;
                }
                //Ras解密密码
                var ras = new RSAEncrypt(rsaKey[0], rsaKey[1]);
                parm.password = ras.Decrypt(parm.password);

                #endregion

                #region 2. 判断用户登录次数限制以及过期时间

                //获得用户登录限制次数
                var configLoginCount = Convert.ToInt32(Appsettings.Configuration[KeyModel.LoginCount]);
                //获得登录次数和过期时间
                SysAdminLoginConfig loginConfig = MemoryCacheHelper.Get<SysAdminLoginConfig>(KeyModel.LoginCount) ?? new SysAdminLoginConfig();
                if (loginConfig.Count != 0 && loginConfig.DelayMinute != null)
                {
                    //说明存在过期时间，需要判断
                    if (DateTime.Now <= loginConfig.DelayMinute)
                    {
                        apiRes.msg = "您的登录以超过设定次数，请稍后再次登录~";
                        return apiRes;
                    }
                    else
                    {
                        //已经过了登录的预设时间，重置登录配置参数
                        loginConfig.Count = 0;
                        loginConfig.DelayMinute = null;
                    }
                }
                #endregion

                #region 3. 从数据库查询该用户

                //查询登录结果
                var dbres = _adminService.LoginAsync(parm).Result;
                if (dbres.statusCode != 200)
                {
                    //增加登录次数
                    loginConfig.Count += 1;
                    //登录的次数大于配置的次数，则提示过期时间
                    if (loginConfig.Count == configLoginCount)
                    {
                        var configDelayMinute = Convert.ToInt32(Appsettings.Configuration[KeyModel.LogindElayMinute]);
                        //记录过期时间
                        loginConfig.DelayMinute = DateTime.Now.AddMinutes(configDelayMinute);
                        apiRes.msg = "登录次数超过" + configLoginCount + "次，请" + configDelayMinute + "分钟后再次登录";
                        return apiRes;
                    }
                    //记录登录次数，保存到session
                    MemoryCacheHelper.Set(KeyModel.LoginCount, loginConfig);
                    //提示用户错误和登录次数信息
                    apiRes.msg = dbres.msg + "　　您还剩余" + (configLoginCount - loginConfig.Count) + "登录次数";
                    return apiRes;
                }

                #endregion

                #region 4. 设置Identity User信息

                var user = dbres.data.admin;
                var identity = new ClaimsPrincipal(
                 new ClaimsIdentity(new[]
                     {
                              new Claim(ClaimTypes.Sid,user.ID),
                              new Claim(ClaimTypes.Role,user.RoleId),
                              new Claim(ClaimTypes.Thumbprint,user.HeadPic),
                              new Claim(ClaimTypes.Name,user.RelName),
                              new Claim(ClaimTypes.WindowsAccountName,user.Account),
                              new Claim(ClaimTypes.UserData,user.LastLoginTime.ToString())
                     }, CookieAuthenticationDefaults.AuthenticationScheme)
                );
                if (Appsettings.Configuration[KeyModel.LoginSaveUser] == "Session")
                {//如果保存用户类型是Session，则默认设置cookie退出浏览器 清空，并且保存用户信息
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, identity, new AuthenticationProperties
                    {
                        AllowRefresh = false
                    });
                }
                else
                {
                    //根据配置保存浏览器用户信息，小时单位
                    var hours = int.Parse(Appsettings.Configuration[KeyModel.LoginCookieExpires]);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, identity, new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddHours(hours),
                        IsPersistent = true,
                        AllowRefresh = false
                    });
                }
                #endregion

                #region 5. 保存权限信息到缓存
                if (dbres.data.menu != null)
                {
                    var menuSaveType = Appsettings.Configuration[KeyModel.LoginAuthorize];
                    if (menuSaveType == "Redis")
                    {
                        RedisCacheHelper.Set(KeyModel.AdminMenu + "_" + dbres.data.admin.ID, dbres.data.menu);
                    }
                    else
                    {
                        MemoryCacheHelper.Set(KeyModel.AdminMenu + "_" + dbres.data.admin.ID, dbres.data.menu);
                    }
                }
                #endregion

                #region 6. 生成token信息，并且返回给前端

                token = JwtHelper.IssueToken(new TokenModel()
                {
                    UserID = user.ID,
                    UserName = user.RelName,
                    UserAccount = user.Account,
                    Role = "AdminPolicy",
                    ProjectName = "DL.Admin"
                });
                MemoryCacheHelper.Del<string>(KeyModel.LoginKey);
                MemoryCacheHelper.Del<string>(KeyModel.LoginCount);

                #endregion

                #region 7. 保存日志

                var agent = HttpContext.Request.Headers["User-Agent"];
                var log = new SysLog()
                {
                    ID = Guid.NewGuid().ToString(),
                    CreateTime = DateTime.Now,
                    Layer = 1,
                    Message = "登录",
                    Url = "/Login/Login",
                    IP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                    Account = parm.loginname,
                    Browser = agent.ToString()
                };
                await _logService.AddAsync(log);

                #endregion
            }
            catch (Exception ex)
            {
                apiRes.msg = ex.Message;
                apiRes.statusCode = (int)ApiEnum.Error;

                #region 保存日志
                var agent = HttpContext.Request.Headers["User-Agent"];
                var log = new SysLog()
                {
                    ID = Guid.NewGuid().ToString(),
                    CreateTime = DateTime.Now,
                    Layer = 4,
                    Message = "登录失败！" + ex.Message,
                    Exception = ex.Message,
                    Url = "/Login/Login",
                    IP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                    Account = parm.loginname,
                    Browser = agent.ToString()
                };
                await _logService.AddAsync(log);
                #endregion
            }

            apiRes.statusCode = (int)ApiEnum.Status;
            apiRes.data = token;
            return apiRes;
        }


        /// <summary>
        /// 管理员退出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ApiResult<string> LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return new ApiResult<string>() { data = "/Login/Index" };
        }
        #endregion


    }
}
