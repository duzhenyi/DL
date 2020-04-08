using System;
using System.Threading.Tasks;
using SqlSugar;
using System.Collections.Generic;
using DL.Domain.Models.SysModels;
using DL.IService.SysIService;
using DL.Domain.PublicModels;
using DL.Utils.Extensions;
using DL.Domain.Dto.AdminDto.SysDto;
using DL.Utils.Log.Nlog;

namespace DL.Service.SysService
{
    /// <summary>
    /// 字典值实现
    /// </summary>
    public class SysCodeService : BaseService<SysCode>, ISysCodeService
    {
        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<SysCode>> GetByIDAsync(string id)
        {
            var model = await Db.Queryable<SysCode>().SingleAsync(m => m.ID == id);
            var res = new ApiResult<SysCode>
            {
                data = model != null ? model : new SysCode() { }
            };
            if (model == null)
            {
                var pmdel = await Db.Queryable<SysCode>().OrderBy(m => m.Sort, OrderByType.Desc).FirstAsync();
                res.data = new SysCode() { IsEnable = true, Sort = pmdel?.Sort + 1 ?? 1 };
            }
            return res;
        }

        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> AddAsync(SysCode parm)
        {
            var res = new ApiResult<string>();
            try
            {
                //判断是否存在
                var isExt = SysCodeDb.IsAny(m => m.Name == parm.Name && m.CodeTypeId == parm.CodeTypeId);
                if (isExt)
                {
                    res.msg = "该名称已存在";
                }
                else
                {
                    parm.ID = Guid.NewGuid().ToString();
                    parm.CreateTime = DateTime.Now;

                    var dbres = await Db.Insertable(parm).ExecuteCommandAsync();
                    if (dbres == 0)
                    {
                        res.msg = "添加失败";
                    }
                    else
                    {
                        res.msg = "添加成功";
                    }
                }
            }
            catch (Exception ex)
            {
                res.statusCode = (int)ApiEnum.Error;
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                NLogHelper.Error(ex.Message);
            }
            return res;
        }


        /// <summary>
        /// 查询列表，根据条件
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<PageReply<SysCode>>> GetPagesAsync(PageParm parm)
        {
            var res = new ApiResult<PageReply<SysCode>>();

            try
            {
                res.data = await Db.Queryable<SysCode>()
                            .WhereIF(!string.IsNullOrEmpty(parm.pid), m => m.CodeTypeId == parm.pid)
                            .OrderBy(m => m.Sort)
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
        /// 修改一条记录
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<string>> ModifyAsync(SysCode model)
        {
            var res = new ApiResult<string>();
            try
            {
                var dbres = await Db.Updateable(model).ExecuteCommandAsync();
                res.msg = dbres > 0 ? "修改成功" : "修改失败";
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
        /// 修改状态
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> ModifyStatusAsync(SysCode model)
        {
            var isok = await Db.Updateable<SysCode>(model).ExecuteCommandAsync();
            var res = new ApiResult<string>
            {
                success = isok > 0,
                statusCode = isok > 0 ? (int)ApiEnum.Status : (int)ApiEnum.Error,
                data = isok > 0 ? "1" : "0"
            };
            return res;
        }

        public async Task<ApiResult<List<ItemObjDto>>> GetListAsync(string parentIDId)
        {
            var list = await Db.Queryable<SysCode>()
                                .WhereIF(parentIDId != null, m => m.CodeTypeId == parentIDId).OrderBy(m => m.Sort, OrderByType.Desc)
                                .Select(m => new ItemObjDto
                                {
                                    title = m.Name,
                                    value = m.Name
                                }).ToListAsync();

            var res = new ApiResult<List<ItemObjDto>>
            {
                statusCode = 200,
                data = list
            };
            return res;
        }
    }
}
