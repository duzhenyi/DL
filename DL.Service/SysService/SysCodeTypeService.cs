using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlSugar;
using DL.IService.SysIService;
using DL.Domain.Models.SysModels;
using DL.Domain.PublicModels;
using DL.Domain.Dto.AdminDto.SysDto;

namespace DL.Service.SysService
{
    /// <summary>
    /// 字典分类
    /// </summary>
    public class SysCodeTypeService : BaseService<SysCodeType>, ISysCodeTypeService
    {
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> AddAsync(SysCodeType model)
        {
            model.ID = Guid.NewGuid().ToString();
            model.CreateTime = DateTime.Now;
            model.IsEnable = true;

            if (string.IsNullOrEmpty(model.ParentId))
            {
                model.Layer = 1;
            }
            else
            {
                var pmodel = Db.Queryable<SysCodeType>().SingleAsync(m => m.ID == model.ParentId).Result;
                model.Layer = pmodel.Layer + 1;
            }

            var res = await Db.Insertable(model).ExecuteCommandAsync();
            return new ApiResult<string>
            {
                msg = res > 0 ? "添加成功" : "添加失败"
            };
        }


        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<SysCodeTypeDto>> GetByIDAsync(string pid)
        {
            var model = await Db.Queryable<SysCodeType>().SingleAsync(m => m.ID == pid);
            var res = new ApiResult<SysCodeTypeDto>
            {
                data = model != null ? new SysCodeTypeDto()
                {
                    id = model.ID,
                    title = model.Name,
                    parent = model.ParentId,
                    sort = model.Sort
                } : null
            };
            if (model != null) return await Task.Run(() => res);
            var pmdel = Db.Queryable<SysCodeType>().OrderBy(m => m.Sort, OrderByType.Desc).First();
            res.data = new SysCodeTypeDto() { sort = pmdel?.Sort + 1 ?? 1 };
            return res;
        }


        /// <summary>
        /// 获得树实现
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<List<SysCodeTypeTree>>> GetListTreeAsync()
        {
            var list = await Db.Queryable<SysCodeType>().ToListAsync();
            var treeList = new List<SysCodeTypeTree>();
            foreach (var item in list.Where(m => m.Layer == 1).OrderBy(m => m.Sort))
            {
                //获得子级
                var children = new List<SysCodeTypeTree>();
                foreach (var row in list.Where(m => m.ParentId == item.ID).OrderBy(m => m.Sort))
                {
                    children.Add(new SysCodeTypeTree()
                    {
                        id = row.ID,
                        title = row.Name,
                        children = null
                    });
                }
                treeList.Add(new SysCodeTypeTree()
                {
                    id = item.ID,
                    title = item.Name,
                    children = children
                });
            }
            var res = new ApiResult<List<SysCodeTypeTree>>
            {
                data = treeList
            };
            return res;
        }

        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<string>> ModifyAsync(SysCodeType model)
        {
            if (!string.IsNullOrEmpty(model.ParentId))
            {
                var pmodel = SysCodeTypeDb.GetById(model.ParentId);
                model.Layer = pmodel.Layer + 1;
            }
            var res = await Db.Updateable(model).ExecuteCommandAsync();
            return new ApiResult<string>
            {
                msg = res > 0 ? "修改成功" : "修改失败"
            };
        }
    }
}
