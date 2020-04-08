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
    /// 广告模块
    /// </summary>
    public class AdoAdoAdvService : BaseService<AdoAdv>, IAdoAdvService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> AddAsync(AdoAdv model)
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
            var res = await Db.Deleteable<AdoAdv>().In(idArry).ExecuteCommandAsync();
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
        public async Task<ApiResult<string>> ModifyAsync(AdoAdv model)
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
        public async Task<ApiResult<AdoAdv>> GetByIDAsync(string id)
        {
            var model = await Db.Queryable<AdoAdv>().SingleAsync(m => m.ID == id);

            var res = new ApiResult<AdoAdv>();
            var pmdel = Db.Queryable<AdoAdv>().OrderBy(m => m.CreateTime, OrderByType.Desc).First();
            res.data = model ?? new AdoAdv() { IsEnable = true };
            return res;
        }

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<PageReply<AdoAdv>>> GetPagesAsync(AdoAdvPageParm parm)
        {
            var res = new ApiResult<PageReply<AdoAdv>>();
            try
            {
                
                res.data = await Db.Queryable<AdoAdv>()
                                   .WhereIF(!string.IsNullOrEmpty(parm.title), m => m.Title.Contains(parm.title))
.WhereIF(!string.IsNullOrEmpty(parm.advLocationId), m => m.AdvLocationId.Contains(parm.advLocationId))
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
