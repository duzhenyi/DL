using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 
using DL.Domain.Models.SysModels;
using DL.Domain.PublicModels;
using DL.IService.SysIService;
using DL.Utils.Extensions;
using DL.Utils.Log.Nlog;
using DL.Utils.Helper;
using SqlSugar;
using DL.Domain.Dto.AdminDto.SysModelsDto;
using DL.Domain.Dto.AdminDto.SysDto;
using DL.Domain.Models.AdoModels;

namespace DL.Service.SysService
{
    /// <summary>
    /// 栏目管理
    /// </summary>
    public class SysSysColumnService : BaseService<SysColumn>, ISysColumnService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> AddAsync(SysColumn model)
        {
            model.ID = Guid.NewGuid().ToString();
            model.CreateTime = DateTime.Now;
            if (string.IsNullOrEmpty(model.ParentID))
            {
                model.Layer = 1;
            }
            else
            {
                var pmodel = Db.Queryable<SysColumn>().SingleAsync(m => m.ID == model.ParentID).Result;
                model.Layer = pmodel.Layer + 1;
                model.ParentTitle = pmodel.Title;
            }
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
             
            var articleModel = Db.Queryable<AdoArticle>().Where(m => idArry.Contains(m.SysColumnId)).SingleAsync().Result;
            if (articleModel != null)
            {
                return new ApiResult<string>
                {
                    msg = "有文章[" + articleModel.Title + "]正在使用该数据,不能删除！"
                };
            }

            var res = await Db.Deleteable<SysColumn>().In(idArry).ExecuteCommandAsync();
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
        public async Task<ApiResult<string>> ModifyAsync(SysColumn model)
        {
            if (!string.IsNullOrEmpty(model.ParentID))
            {//说明有父级  根据父级，查询对应的模型

                var pmodel = SysColumnDb.GetById(model.ParentID);
                model.Layer = model.Layer + 1;
                model.ParentTitle = pmodel.Title;
            }
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
        public async Task<ApiResult<SysColumn>> GetByIDAsync(string id)
        {
            var model = await Db.Queryable<SysColumn>().SingleAsync(m => m.ID == id);

            var res = new ApiResult<SysColumn>();
            var pmdel = Db.Queryable<SysColumn>().OrderBy(m => m.CreateTime, OrderByType.Desc).First();
            res.data = model ?? new SysColumn() { IsEnable = true };
            return res;
        }

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<PageReply<SysColumn>>> GetPagesAsync(SysColumnPageParm parm)
        {
            var res = new ApiResult<PageReply<SysColumn>>();
            try
            {
                
                res.data = await Db.Queryable<SysColumn>()
                                   .WhereIF(!string.IsNullOrEmpty(parm.title), m => m.Title.Contains(parm.title))
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
        /// 查询Tree
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<List<TreeDto>>> GetListTreeAsync()
        {
            var list = await Db.Queryable<SysColumn>().ToListAsync();
            var treeList = new List<TreeDto>();
            foreach (var item in list.Where(m => m.Layer == 1).OrderBy(m => m.Sort))
            {
                //获得子级
                var children = RecursionChild(list, new List<TreeDto>(), item.ID);
                treeList.Add(new TreeDto()
                {
                    id = item.ID,
                    title = item.Title,
                    spread = children.Count > 0,
                    children = children.Count == 0 ? null : children
                });
            }
            var res = new ApiResult<List<TreeDto>>
            {
                statusCode = 200,
                data = treeList
            };
            return res;
        }

        /// <summary>
        /// 递归
        /// </summary>
        /// <param name="sourceList">原数据</param>
        /// <param name="list">新集合</param>
        /// <param name="pid">父节点</param>
        /// <returns></returns>
        List<TreeDto> RecursionChild(List<SysColumn> sourceList, List<TreeDto> list, string pid)
        {
            foreach (var row in sourceList.Where(m => m.ParentID == pid).OrderBy(m => m.Sort))
            {
                var res = RecursionChild(sourceList, new List<TreeDto>(), row.ID);
                list.Add(new TreeDto()
                {
                    id = row.ID,
                    title = row.Title,
                    spread = res.Count > 0,
                    children = res.Count > 0 ? res : null
                });
            }
            return list;
        }

    }
}
