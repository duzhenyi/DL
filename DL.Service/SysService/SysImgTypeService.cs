using DL.IService.SysIService;
using DL.Domain.Models.SysModels;
using DL.Domain.Dto.AdminDto.SysDto;
using DL.Domain.PublicModels;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DL.Service.SysService
{
    public class SysImgTypeService : BaseService<SysImgType>, ISysImgTypeService
    {
        /// <summary>
        /// 添加部门信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> AddAsync(SysImgType model)
        {
            
            model.ID = Guid.NewGuid().ToString();
            model.CreateTime = DateTime.Now;
            
            if (string.IsNullOrEmpty(model.ParentId))
            {
                model.Layer = 1;
            }
            else
            {
                var pmodel = Db.Queryable<SysImgType>().SingleAsync(m => m.ID == model.ParentId).Result;
                model.Layer = pmodel.Layer + 1;
                model.ParentName = pmodel.Name;
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
            var imgModel = Db.Queryable<SysImage>().Where(m => idArry.Contains(m.SysImgTypeId)).SingleAsync().Result;
            if (imgModel != null)
            {
                return new ApiResult<string>
                {
                    msg = "有图片正在使用该数据,不能删除！"
                };
            }

            var res = await Db.Deleteable<SysImgType>().In(idArry).ExecuteCommandAsync();
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
        public async Task<ApiResult<string>> ModifyAsync(SysImgType model)
        {
            if (!string.IsNullOrEmpty(model.ParentId))
            {//说明有父级  根据父级，查询对应的模型

                var pmodel = SysOrganizeDb.GetById(model.ParentId);
                model.Layer = model.Layer + 1;
                model.ParentName = pmodel.Name;
            }
            var dbres = await Db.Updateable(model).ExecuteCommandAsync();
            var res = new ApiResult<string>
            {
                msg = dbres > 0 ? "修改成功" : "修改失败"
            };
            return res;
        }


        /// <summary>
        /// 查询Tree
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<List<TreeDto>>> GetListTreeAsync()
        {
            var list = await Db.Queryable<SysImgType>().ToListAsync();
            var treeList = new List<TreeDto>();
            foreach (var item in list.Where(m => m.Layer == 1))
            {
                //获得子级
                var children = RecursionOrganize(list, new List<TreeDto>(), item.ID);
                treeList.Add(new TreeDto()
                {
                    id = item.ID,
                    title = item.Name,
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
        List<TreeDto> RecursionOrganize(List<SysImgType> sourceList, List<TreeDto> list, string pid)
        {
            foreach (var row in sourceList.Where(m => m.ParentId == pid))
            {
                var res = RecursionOrganize(sourceList, new List<TreeDto>(), row.ID);
                list.Add(new TreeDto()
                {
                    id = row.ID,
                    title = row.Name,
                    spread = res.Count > 0,
                    children = res.Count > 0 ? res : null
                });
            }
            return list;
        }

    }
}
