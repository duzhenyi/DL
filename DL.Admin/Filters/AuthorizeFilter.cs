using DL.Admin.Models;
using DL.Domain.Dto.AdminDto.SysDto;
using DL.Domain.PublicModels;
using DL.Utils.Auth;
using DL.Utils.Auth.Jwt;
using DL.Utils.Cache.MemoryCache;
using DL.Utils.Log.Log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DL.Admin.Filters
{
    public class AuthorizeFilter : ActionFilterAttribute
    {
        #region 字段和属性

        /// <summary>
        /// 模块别名，可配置更改
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// 权限动作
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 权限访问控制器参数
        /// </summary>
        private string Sign { get; set; }

        /// <summary>
        /// 是否保存日志
        /// </summary>
        public bool IsLog { get; set; } = true;

        private string ActionArguments { get; set; }
        private Stopwatch Stopwatch { get; set; }

        #endregion

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            if (IsLog)
            {
                ActionArguments = JsonConvert.SerializeObject(context.ActionArguments);
                Stopwatch = new Stopwatch();
                Stopwatch.Start();
            }
            var userID = "";
            //检测是否包含'Authorization'请求头，如果不包含则直接放行
            if (context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                var tokenHeader = context.HttpContext.Request.Headers["Authorization"];
                tokenHeader = tokenHeader.ToString().Substring("Bearer ".Length).Trim();

                TokenModel tm = JwtHelper.SerializeToken(tokenHeader);
                userID = tm.UserID;
            }
            //获得权限
            //List<SysMenuDto> menu = MemoryCacheHelper.Get<List<SysMenuDto>>(KeyModel.AdminMenu + "_" + userID);
            //if (menu == null)
            //{
            //    ContextReturn(context, "登录已过期，请退出重新登录！");
            //    return;
            //}
            //如果是超管，不做权限控制处理
            if (Controller != "admin")
            {
                //if (string.IsNullOrEmpty(Controller))
                //{
                //    ContextReturn(context, "您没有操作权限，请联系系统管理员！");
                //    return;
                //}

                ////判断是否包含权限模块
                //var menuModel = menu.Find(m => m.nameCode == Controller);
                //if (!menu.Any(m => m.nameCode == Controller) || menuModel.btnFun == null)
                //{
                //    ContextReturn(context, "您没有操作权限，请联系系统管理员！");
                //    return;
                //}
                ////判断模块下面的权限是否包含功能
                //if (!menuModel.btnFun.Any(m => m.codeType == Action))
                //{
                //    ContextReturn(context, "您没有操作权限，请联系系统管理员！");
                //    return;
                //}
            }
            base.OnActionExecuting(context);
        }
        /// <summary>
        /// 返回API的信息
        /// </summary>
        /// <param name="context"></param>
        private void ContextReturn(ActionExecutingContext context, string msg)
        {
            var res = new ApiResult<string>() { statusCode = (int)ApiEnum.Unauthorized, msg = msg };
            context.HttpContext.Response.ContentType = "application/json;charset=utf-8";
            context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(res));
            context.Result = new EmptyResult();
        }


        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            if (IsLog)
            {
                Stopwatch.Stop();

                string url = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
                string method = context.HttpContext.Request.Method;

                string qs = ActionArguments;

                var user = "";
                //检测是否包含'Authorization'请求头，如果不包含则直接放行
                if (context.HttpContext.Request.Headers.ContainsKey("Authorization"))
                {
                    var tokenHeader = context.HttpContext.Request.Headers["Authorization"];
                    tokenHeader = tokenHeader.ToString().Substring("Bearer ".Length).Trim();

                    TokenModel tm = JwtHelper.SerializeToken(tokenHeader);
                    user = tm.UserName;
                }

                var str = $"\n 方法：{Controller}：{Action} \n " +
                    $"地址：{url} \n " +
                    $"方式：{method} \n " +
                    $"参数：{qs}\n " +
                    $"用户：{user}\n " +
                    $"耗时：{Stopwatch.Elapsed.TotalMilliseconds} 毫秒";
                Log4netHelper.Info(typeof(string), str);
            }

        }
    }
}
