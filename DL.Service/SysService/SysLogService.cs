using DL.Domain.Models.SysModels;
using DL.Domain.PublicModels;
using DL.IService.SysIService;
using DL.Utils.Extensions;
using DL.Utils.Helper;
using SqlSugar;
using System;
using System.Threading.Tasks;

namespace DL.Service.SysService
{
    /// <summary>
    /// 系统日志接口实现
    /// </summary>
    public class SysLogService : BaseService<SysLog>, ISysLogService
    {
        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<ApiResult<PageReply<SysLog>>> GetPagesAsync(PageParm parm)
        {
            var res = new ApiResult<PageReply<SysLog>>();
            try
            {
                string beginTime = string.Empty, endTime = string.Empty;
                if (!string.IsNullOrEmpty(parm.time))
                {
                    var timeRes = UtilsHelper.SplitString(parm.time, '-');
                    beginTime = timeRes[0].Trim();
                    endTime = timeRes[1].Trim();
                }
                res.data =await Db.Queryable<SysLog>()
                    .WhereIF(!string.IsNullOrEmpty(parm.key), m => m.Creator.Contains(parm.key))
                    //.WhereIF(!string.IsNullOrEmpty(parm.where), m => m.Layer == parm.where)
                    //.WhereIF(!string.IsNullOrEmpty(parm.time), m => m.Logged >= Convert.ToDateTime(beginTime) && m.Logged <= Convert.ToDateTime(endTime))
                    .OrderBy(m => m.CreateTime, OrderByType.Desc)
                    .Mapper((it, cache) =>
                    {
                        if (!string.IsNullOrEmpty(it.Account))
                        {
                            it.RelName = it.RelName;
                        }
                    })
                    .ToPageAsync(parm.page, parm.limit);
            }
            catch (Exception ex)
            {
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                res.statusCode = (int)ApiEnum.Error;
                //LoggerHelper.Default.ProcessError((int)ApiEnum.Error, ex.Message);
            }
            return res;
        }
    }
}
