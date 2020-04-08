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
    /// 高校内容
    /// </summary>
    public class AdoAdoSchoolContentService : BaseService<AdoSchoolContent>, IAdoSchoolContentService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> AddAsync(AdoSchoolContent model)
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
            if (ids == null)
            {
                return new ApiResult<string>
                {
                    msg = "数据不存在"
                };
            }

            var idArry = ids.Trim(',').Split(',');
            var res = await Db.Deleteable<AdoSchoolContent>().In(idArry).ExecuteCommandAsync();
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
        public async Task<ApiResult<string>> ModifyAsync(AdoSchoolContent model)
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
        public async Task<ApiResult<AdoSchoolContent>> GetByIDAsync(string id)
        {
            var model = await Db.Queryable<AdoSchoolContent>().SingleAsync(m => m.ID == id);

            var res = new ApiResult<AdoSchoolContent>();
            var pmdel = Db.Queryable<AdoSchoolContent>().OrderBy(m => m.CreateTime, OrderByType.Desc).First();
            res.data = model ?? new AdoSchoolContent() { IsEnable = true };
            return res;
        }

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<PageReply<AdoSchoolContent>>> GetPagesAsync(AdoSchoolContentPageParm parm)
        {
            var res = new ApiResult<PageReply<AdoSchoolContent>>();
            try
            {

                res.data = await Db.Queryable<AdoSchoolContent>()
                                   .WhereIF(!string.IsNullOrEmpty(parm.sysColumnId), m => m.SysColumnId == parm.sysColumnId)
                                   .WhereIF(!string.IsNullOrEmpty(parm.schoolId), m => m.SchoolId == parm.schoolId)
                                   .WhereIF(!string.IsNullOrEmpty(parm.tagId), m => m.TagId == parm.tagId)
                                   .WhereIF(!string.IsNullOrEmpty(parm.title), m => m.Title.Contains(parm.title))
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
