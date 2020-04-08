using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 
using DL.Domain.Models.AdoModels;
using DL.Domain.PublicModels;
using DL.IService.AdoIService;
using DL.Utils.Extensions;
using DL.Utils.Log.Nlog;
using DL.Utils.Helper;
using SqlSugar;
using DL.Domain.Dto.AdminDto.AdoModelsDto;
using DL.Domain.Dto.SiteDto;
using DL.Utils.Security;
using Microsoft.AspNetCore.Http;

namespace DL.Service.AdoService
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class AdoAdoUserService : BaseService<AdoUser>, IAdoUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdoAdoUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> AddAsync(AdoUser model)
        {
            model.ID = Guid.NewGuid().ToString();
            model.CreateTime = DateTime.Now; 

            var res = await Db.Insertable(model).ExecuteCommandAsync();
            return new ApiResult<string>
            {
                msg = res > 0 ? "添加成功" : "添加失败"
            };
        }


        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> DelAsync(string ids)
        {
            if (ids == null)
            {
                return new ApiResult<string>
                {
                    msg = "数据不存在"
                };
            }

            var idArry = ids.Trim(',').Split(','); 
            var res = await Db.Deleteable<AdoUser>().In(idArry).ExecuteCommandAsync();
            return new ApiResult<string>
            {
                msg = res == idArry.Length ? "删除成功" : "删除失败"
            };
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> ModifyAsync(AdoUser model)
        { 
            var dbres = await Db.Updateable(model).ExecuteCommandAsync();
            var res = new ApiResult<string>
            {
                msg = dbres > 0 ? "修改成功" : "修改失败"
            };
            return res;
        }

        /// <summary>
        /// 根据唯一编号查询一条信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<AdoUser>> GetByIDAsync(string id)
        {
            var model = await Db.Queryable<AdoUser>().SingleAsync(m => m.ID == id);

            var res = new ApiResult<AdoUser>();
            var pmdel = Db.Queryable<AdoUser>().OrderBy(m => m.CreateTime, OrderByType.Desc).First();
            res.data = model ?? new AdoUser() { IsEnable = true };
            return res;
        }

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<PageReply<AdoUser>>> GetPagesAsync(AdoUserPageParm parm)
        {
            var res = new ApiResult<PageReply<AdoUser>>();
            try
            {
                
                res.data = await Db.Queryable<AdoUser>()
                                   .WhereIF(!string.IsNullOrEmpty(parm.loginAccount), m => m.LoginAccount.Contains(parm.loginAccount))
                                   .WhereIF(!string.IsNullOrEmpty(parm.nickName), m => m.NickName.Contains(parm.nickName))
                                   .WhereIF(parm.isEnable, m => m.IsEnable)
                                   .WhereIF(!parm.isEnable, m => !m.IsEnable)
                                   .OrderBy(m => m.CreateTime)
                                   .ToPageAsync(parm.page, parm.limit);
            }
            catch (Exception ex)
            {
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                res.statusCode = (int)ApiEnum.Error;
                NLogHelper.Error(ex.Message);
            }
            return res;
        }


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="adoUserLogin"></param>
        /// <returns></returns>
        public async Task<ApiResult<AdoUser>> LoginAsync(AdoUserLogin adoUserLogin)
        {
            var res = new ApiResult<AdoUser>();
            try
            {
                var adminModel = new AdoUser();
                adoUserLogin.password = DES3Encrypt.EncryptString(adoUserLogin.password);
                var model = await Db.Queryable<AdoUser>().Where(m => m.LoginAccount == adoUserLogin.loginname && m.IsEnable).FirstAsync();
                if (model == null)
                {
                    res.msg = "账号错误";
                    return res;
                }
                if (!model.Pwd.Equals(adoUserLogin.password))
                {
                    res.msg = "密码错误";
                    return res;
                }
                if (!model.IsEnable)
                {
                    res.msg = "登录账号被冻结，请联系管理员";
                    return res;
                }
                 
                //修改登录时间
                model.LoginTime = DateTime.Now;
                model.LastLoginTime = model.LoginTime;
                model.LoginCount = model.LoginCount + 1;

                var ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                if (ip.Length < 10)
                {
                    ip = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.FirstOrDefault(address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.ToString();
                }

                model.LoginIp = ip;
                AdoUserDb.Update(model);

                res.statusCode = (int)ApiEnum.Status;
                res.data = adminModel;
            }
            catch (Exception ex)
            {
                res.statusCode = (int)ApiEnum.Error;
                res.msg = ex.Message;
                NLogHelper.Error(ex.Message);
            }
            return res;
        }
    }
}
