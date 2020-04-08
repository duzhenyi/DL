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

namespace DL.Service.AdoService
{
    /// <summary>
    /// 资源模块
    /// </summary>
    public class AdoAdoDownloadService : BaseService<AdoDownload>, IAdoDownloadService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> AddAsync(AdoDownload model)
        {
            var res = new ApiResult<string>();
            try
            {
                model.ID = Guid.NewGuid().ToString();
                model.CreateTime = DateTime.Now;

                var dbRes = await Db.Insertable(model).ExecuteCommandAsync();
                res.msg = dbRes > 0 ? "添加成功" : "添加失败";
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
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> DelAsync(string ids)
        {
            var res = new ApiResult<string>();
            try
            {
                if (ids == null)
                {
                    return new ApiResult<string>
                    {
                        msg = "数据不存在"
                    };
                }

                var idArry = ids.Trim(',').Split(',');
                var dbRes = await Db.Deleteable<AdoDownload>().In(idArry).ExecuteCommandAsync();
                res.msg = dbRes == idArry.Length ? "删除成功" : "删除失败";
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
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> ModifyAsync(AdoDownload model)
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
        public async Task<ApiResult<AdoDownload>> GetByIDAsync(string id)
        {
            var model = await Db.Queryable<AdoDownload>().SingleAsync(m => m.ID == id);

            var res = new ApiResult<AdoDownload>();
            var pmdel = Db.Queryable<AdoDownload>().OrderBy(m => m.CreateTime, OrderByType.Desc).First();
            res.data = model ?? new AdoDownload() { IsEnable = true };
            return res;
        }

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<PageReply<AdoDownload>>> GetPagesAsync(AdoDownloadPageParm parm)
        {
            var res = new ApiResult<PageReply<AdoDownload>>();
            try
            {

                res.data = await Db.Queryable<AdoDownload>()
                                   .WhereIF(!string.IsNullOrEmpty(parm.title), m => m.Title.Contains(parm.title))
                                   .WhereIF(!string.IsNullOrEmpty(parm.tagId), m => m.TagId == parm.tagId)
                                   .WhereIF(!string.IsNullOrEmpty(parm.sysColumnId), m => m.SysColumnId == parm.sysColumnId)
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
    }
}
