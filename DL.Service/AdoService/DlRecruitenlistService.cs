using DL.Domain.Dto.AdminDto.AdoDto;
using DL.Domain.Models.AdoModels;
using DL.Domain.PublicModels;
using DL.IService.AdoIService;
using DL.Utils.Extensions;
using DL.Utils.Helper;
using DL.Utils.Log.Log4net;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DL.Service.AdoService
{
    public class AdoRecruitenlistService : BaseService<AdoRecruitenlist>, IAdoRecruitenlistService
    {
        public async Task<ApiResult<List<MyAdoRecruitenlistDto>>> GetMyRecruitListAsync(string userGuid)
        {
            var res = new ApiResult<List<MyAdoRecruitenlistDto>>();
            try
            {
                res.data = await Db.Queryable<AdoRecruitenlist, AdoRecruit>((l, r) => new object[]
                {
                    JoinType.Left,l.RecruitGuid == r.Guid
                })
                .Where(l => l.UserGuid == userGuid) 
                .Select((l,r) => new MyAdoRecruitenlistDto()
                {
                    Guid =l.Guid,
                    RecruitGuid = l.RecruitGuid,
                    AddTime =l.AddTime,
                    UserGuid =l.UserGuid,
                    Name =l.Name,
                    Tel= l.Tel,
                    Title =r.Title
                }).OrderBy(l => l.AddTime, OrderByType.Desc).ToListAsync();

            }
            catch (Exception ex)
            {
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                res.statusCode = (int)ApiEnum.Error;
                Log4netHelper.Error(typeof(string), ex.Message);
            }
            return res;
        }

        public async Task<ApiResult<PageReply<AdoRecruitenlist>>> GetPagesAsync(PageParmRqst parm)
        {
            var res = new ApiResult<PageReply<AdoRecruitenlist>>();
            try
            {
                string beginTime = string.Empty, endTime = string.Empty;
                if (!string.IsNullOrEmpty(parm.time))
                {
                    var timeRes = UtilsHelper.SplitString(parm.time, '-');
                    beginTime = timeRes[0].Trim();
                    endTime = timeRes[1].Trim();
                }
                res.data = await Db.Queryable<AdoRecruitenlist>()
                    .Where(m => m.RecruitGuid == parm.guid)
                    .WhereIF(!string.IsNullOrEmpty(parm.key), m => m.Name.Contains(parm.key))
                    .WhereIF(!string.IsNullOrEmpty(parm.time), m => m.AddTime >= Convert.ToDateTime(beginTime) && m.AddTime <= Convert.ToDateTime(endTime))
                    .OrderBy(m => m.AddTime, OrderByType.Desc)
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