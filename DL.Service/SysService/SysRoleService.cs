using DL.Domain.Dto.AdminDto.SysDto;
using DL.Domain.Models.SysModels;
using DL.Domain.PublicModels;
using DL.IService.SysIService;
using DL.Utils.Extensions;
using DL.Utils.Log.Log4net;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DL.Service.SysService
{
    /// <summary>
    /// 角色功能实现
    /// </summary>
    public class SysRoleService : BaseService<SysRole>, ISysRoleService
    {
        /// <summary>
        /// 添加角色信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> AddAsync(SysRole parm)
        {
            var res = new ApiResult<string>() { data = "1", statusCode = 200 };
            try
            {
                parm.ID = Guid.NewGuid().ToString();
                parm.CreateTime = DateTime.Now;
                await Db.Insertable(parm).ExecuteCommandAsync();
            }
            catch (Exception ex)
            {
                res.statusCode = (int)ApiEnum.Error;
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                Log4netHelper.Error(typeof(string), ex.Message);
            }
            return res;
        }

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
            var sysPermissions = Db.Queryable<SysPermissions>().Where(m => idArry.Contains(m.RoleId)).SingleAsync().Result;
            if (sysPermissions != null)
            {
                return new ApiResult<string>
                {
                    msg = "存在数据被其他信息使用,不能删除！"
                };
            }

            var res = await Db.Deleteable<SysRole>().In(idArry).ExecuteCommandAsync();
            return new ApiResult<string>
            {
                msg = res == idArry.Length ? "删除成功" : "删除失败"
            };
        }


        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> ModifyAsync(SysRole model)
        {
            var res = new ApiResult<string>();
            try
            {
                var dbres = await Db.Updateable(model).ExecuteCommandAsync();
                res.data = dbres > 0 ? "修改成功" : "修改失败";
            }
            catch (Exception ex)
            {
                res.statusCode = (int)ApiEnum.Error;
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                Log4netHelper.Error(typeof(string), ex.Message);
            }
            return res;
        }

        /// <summary>
        /// 获得分页列表
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<PageReply<SysRole>>> GetPagesAsync(PageParm parm)
        {
            var res = new ApiResult<PageReply<SysRole>>();
            try
            {
                res.data = await Db.Queryable<SysRole>()
                         .OrderBy(m => m.Sort)
                         .ToPageAsync(parm.page, parm.limit);
            }
            catch (Exception ex)
            {
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                res.statusCode = (int)ApiEnum.Error;
                Log4netHelper.Error(typeof(string), ex.Message);
            }
            return res;
        }


        /// <summary>
        /// 获得列表
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<PageReply<SysRoleDto>>> GetPagesToRoleAsync(int limit, string deparmentID, string adminID)
        {
            var res = new ApiResult<PageReply<SysRoleDto>>();
            try
            {
                var reslist = await Db.Queryable<SysRole>()
                         // .WhereIF(!string.IsNullOrEmpty(deparmentID), m => m.DepartmentGroup.Contains(deparmentID))
                         .OrderBy(m => m.CreateTime)
                         .Select(it => new SysRoleDto()
                         {
                             ID = it.ID,
                             name = it.Name,
                             sort = it.Sort,
                         })
                         .Mapper((it, cache) =>
                         {
                             var list = cache.Get(g =>
                               {
                                   return Db.Queryable<SysPermissions>().Where(m => m.AdminId == adminID && m.RoleType == 2).ToList();
                               });
                             if (list.Any(m => m.ID == it.ID))
                             {
                                 it.status = true;
                             }
                             else
                             {
                                 it.status = false;
                             }
                         })
                         .ToPageAsync(1, limit);

                var tree = reslist.Items;
                var newList = new List<SysRoleDto>();
                if (tree != null)
                {
                    foreach (var item in tree.Where(m => m.level == 0).ToList())
                    {
                        //查询角色
                        var tempRole = tree.Where(m => m.ParentId == item.ID && m.level == 1).ToList();
                        if (!string.IsNullOrEmpty(deparmentID))
                        {
                            tempRole = tempRole.Where(m => m.DepartmentGroup.Contains(deparmentID)).ToList();
                        }
                        if (tempRole.Count > 0)
                        {
                            newList.Add(item);
                        }
                        foreach (var row in tempRole)
                        {
                            row.name = "　|--" + row.name;
                            newList.Add(row);
                        }
                    }
                }
                //赋值新的数组
                reslist.Items = newList;
                res.data = reslist;
            }
            catch (Exception ex)
            {
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                res.statusCode = (int)ApiEnum.Error;
                Log4netHelper.Error(typeof(string), ex.Message);
            }
            return res;
        }
    }
}
