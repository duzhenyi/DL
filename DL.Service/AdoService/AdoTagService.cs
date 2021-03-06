﻿using System;
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
    /// Desc
    /// </summary>
    public class AdoAdoTagService : BaseService<AdoTag>, IAdoTagService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> AddAsync(AdoTag model)
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
            var res = await Db.Deleteable<AdoTag>().In(idArry).ExecuteCommandAsync();
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
        public async Task<ApiResult<string>> ModifyAsync(AdoTag model)
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
        public async Task<ApiResult<AdoTag>> GetByIDAsync(string id)
        {
            var model = await Db.Queryable<AdoTag>().SingleAsync(m => m.ID == id);

            var res = new ApiResult<AdoTag>();
            var pmdel = Db.Queryable<AdoTag>().OrderBy(m => m.CreateTime, OrderByType.Desc).First();
            res.data = model ?? new AdoTag() { IsEnable = true };
            return res;
        }

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<PageReply<AdoTag>>> GetPagesAsync(AdoTagPageParm parm)
        {
            var res = new ApiResult<PageReply<AdoTag>>();
            try
            {
                string beginCreateTimeTime = string.Empty, endCreateTimeTime = string.Empty;
                if (!string.IsNullOrEmpty(parm.createTime))
                {
                    var timeCreateTimeRes = UtilsHelper.SplitString(parm.createTime, '-');
                    beginCreateTimeTime = timeCreateTimeRes[0].Trim();
                    endCreateTimeTime = timeCreateTimeRes[1].Trim();
                }

                res.data = await Db.Queryable<AdoTag>()
                                   .WhereIF(parm.isHot, m => m.IsHot)
                                   .WhereIF(!parm.isHot, m => !m.IsHot)
                                   .WhereIF(parm.isEnable, m => m.IsEnable)
                                   .WhereIF(!parm.isEnable, m => !m.IsEnable)
                                   .WhereIF(!string.IsNullOrEmpty(parm.createTime), m => m.CreateTime >= Convert.ToDateTime(beginCreateTimeTime) && m.CreateTime <= Convert.ToDateTime(endCreateTimeTime))
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
