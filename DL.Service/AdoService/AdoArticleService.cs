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
    /// 文章管理
    /// </summary>
    public class AdoAdoArticleService : BaseService<AdoArticle>, IAdoArticleService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> AddAsync(AdoArticle model)
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
            var res = await Db.Deleteable<AdoArticle>().In(idArry).ExecuteCommandAsync();
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
        public async Task<ApiResult<string>> ModifyAsync(AdoArticle model)
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
        public async Task<ApiResult<AdoArticle>> GetByIDAsync(string id)
        {
            var model = await Db.Queryable<AdoArticle>().SingleAsync(m => m.ID == id);

            var res = new ApiResult<AdoArticle>();
            var pmdel = Db.Queryable<AdoArticle>().OrderBy(m => m.CreateTime, OrderByType.Desc).First();
            res.data = model ?? new AdoArticle() { IsEnable = true };
            return res;
        }

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<PageReply<AdoArticle>>> GetPagesAsync(AdoArticlePageParm parm)
        {
            var res = new ApiResult<PageReply<AdoArticle>>();
            try
            {

                res.data = await Db.Queryable<AdoArticle>()
                                   .WhereIF(!string.IsNullOrEmpty(parm.sysColumnId), m => m.SysColumnId == parm.sysColumnId)
                                   .WhereIF(!string.IsNullOrEmpty(parm.tagId), m => m.TagId == parm.tagId)
                                   .WhereIF(parm.attrcheck == 0, m => m.IsTop) 
                                   .WhereIF(parm.attrcheck == 1, m => m.IsHot) 
                                   .WhereIF(parm.attrcheck == 2, m => m.IsScroll) 
                                   .WhereIF(parm.attrcheck == 3, m => m.IsSlide) 
                                   .WhereIF(parm.attrcheck == 4, m => m.IsComment) 
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
