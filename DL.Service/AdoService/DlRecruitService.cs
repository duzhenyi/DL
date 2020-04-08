using DL.Domain.Dto.AdminDto.AdoDto;
using DL.Domain.Models.AdoModels;
using DL.Domain.Models.SysModels;
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
    public class AdoRecruitService : BaseService<AdoRecruit>, IAdoRecruitService
    {
        public async Task<ApiResult<PageReply<AdoRecruit>>> GetPageAsync(QueryJobParams queryJobParams)
        {
            var res = new ApiResult<PageReply<AdoRecruit>>();
            try
            {
                res.data = await Db.Queryable<AdoRecruit>()
                    .Where(m => (m.Audit ==1 && m.IsTimeLimit && m.BeginTime < DateTime.Now && m.EndTime > DateTime.Now) || (m.Audit==1 && !m.IsTimeLimit))
                    .WhereIF(!string.IsNullOrEmpty(queryJobParams.key) , m => queryJobParams.key.Contains(m.Title) || m.Tag.Contains(queryJobParams.key))
                    .WhereIF(!string.IsNullOrEmpty(queryJobParams.workArea) && queryJobParams.workArea != "不限", m => m.WorkArea.Contains(queryJobParams.workArea))
                    .WhereIF(!string.IsNullOrEmpty(queryJobParams.industry) && queryJobParams.industry != "不限", m => m.Industry.Contains(queryJobParams.industry))
                    .WhereIF(!string.IsNullOrEmpty(queryJobParams.workType) && queryJobParams.workType != "不限", m => queryJobParams.workType == m.WorkType)
                    .WhereIF(!string.IsNullOrEmpty(queryJobParams.settlementAmount) && queryJobParams.settlementAmount != "不限", m => queryJobParams.settlementAmount == m.SettlementAmount)
                    .WhereIF(!string.IsNullOrEmpty(queryJobParams.workMoney) && queryJobParams.workMoney != "不限", m => queryJobParams.workMoney == m.Money)
                    .WhereIF(queryJobParams.comprehensive == "1", m => m.AddTime.Date == DateTime.Now.Date)
                    .WhereIF(queryJobParams.comprehensive == "2", m => m.AddTime.Date == DateTime.Now.AddDays(-1).Date)
                    .WhereIF(queryJobParams.comprehensive == "3", m => m.AddTime.AddDays(0 - Convert.ToInt16(DateTime.Now.DayOfWeek)) == DateTime.Now.AddDays(0 - Convert.ToInt16(DateTime.Now.DayOfWeek)))
                    .WhereIF(queryJobParams.comprehensive == "4", m => m.IsTop)
                    .OrderByIF(queryJobParams.comprehensive == "0", m => m.DayHits, OrderByType.Desc)
                    .ToPageAsync(queryJobParams.pageIndex, queryJobParams.limit);
            }
            catch (Exception ex)
            {
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                res.statusCode = (int)ApiEnum.Error;
                //LoggerHelper.Default.ProcessError((int)ApiEnum.Error, ex.Message);
            }
            return res;
        }

        private bool CheckMoney(List<string> remunerations, string money)
        {
            var list = Db.Queryable<SysCode>().Where(m => m.Guid == "258371e8-a5cb-49fe-86c4-dbf52501737f" && m.Status).ToList();
            foreach (var item in list)
            {
                var arry = item.Name.Split('~');
                if (arry[0] == money)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<ApiResult<PageReply<AdoRecruit>>> GetPagesAsync(PageParmRqst parm)
        {
            var res = new ApiResult<PageReply<AdoRecruit>>();
            try
            {
                string beginTime = string.Empty, endTime = string.Empty;
                if (!string.IsNullOrEmpty(parm.time))
                {
                    var timeRes = UtilsHelper.SplitString(parm.time, '-');
                    beginTime = timeRes[0].Trim();
                    endTime = timeRes[1].Trim();
                }
                res.data = await Db.Queryable<AdoRecruit>()
                    .WhereIF(parm.audit == 0, m => m.Audit==0)
                    .WhereIF(parm.audit == 1, m => m.Audit == 1)
                    .WhereIF(!string.IsNullOrEmpty(parm.key), m => m.Title.Contains(parm.key))
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

        /// <summary>
        /// 审核数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> VerifyAsync(string ids, string desc, string sysAdminId)
        {
            var res = new ApiResult<string>();
            var list = new List<AdoRecruit>();
            var arry = ids.Split(',');
            for (int i = 0; i < arry.Length; i++)
            {
                if (!string.IsNullOrEmpty(arry[i]))
                {
                    var entity = await Db.Queryable<AdoRecruit>().FirstAsync(m => m.Guid == arry[i]);
                    if (entity != null )
                    {
                        entity.SysAdminId = sysAdminId;
                        entity.AuditDesc = desc;
                        entity.AuditTime = DateTime.Now;
                        list.Add(entity);
                    }
                }
            }

            await UpdateAsync(list);
            return res;
        }
    }
}