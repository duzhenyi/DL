using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlSugar;
using DL.Domain.Models.SysModels;
using DL.IService.SysIService;
using DL.Domain.PublicModels;
using DL.Domain.Dto.AdminDto.SysDto;
using DL.Utils.Extensions;
using DL.Utils.Log.Nlog;

namespace DL.Service.SysService
{
    public class SysMenuService : BaseService<SysMenu>, ISysMenuService
    {
        /// <summary>
        /// 添加菜单信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> AddAsync(SysMenu model)
        {
            var res = new ApiResult<string>();
            //判断别名是否存在，要不一样的
            var count = Db.Queryable<SysMenu>().Where(m => m.EnCode == model.EnCode && m.ParentId == model.ParentId).Count();
            if (count > 0)
            {
                res.statusCode = (int)ApiEnum.Error;
                res.msg = "别名已存在";
                return await Task.Run(() => res);
            }
            model.ID = Guid.NewGuid().ToString();
            model.CreateTime = DateTime.Now;

            if (!string.IsNullOrEmpty(model.ParentId) && model.ParentId != "0")
            { //说明有父级m,据父级，查询对应的模型
                var pmodel = SysMenuDb.GetById(model.ParentId);
                model.ParentName = pmodel.Name;
            }

            var dbres = await Db.Insertable(model).ExecuteCommandAsync();
            if (dbres > 0)
            {
                res.msg = "添加成功";
            }
            else
            {
                res.statusCode = (int)ApiEnum.Error;
                res.msg = "添加失败";
            }
            return res;
        }

        /// <summary>
        /// 根据唯一编号查询菜单信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<SysMenu>> GetByIDAsync(string parm)
        {
            var model = await Db.Queryable<SysMenu>().SingleAsync(m => m.ID == parm);
            var res = new ApiResult<SysMenu>();
            var pmdel = Db.Queryable<SysMenu>().OrderBy(m => m.Sort, OrderByType.Desc).First();
            res.data = model ?? new SysMenu() { Sort = pmdel?.Sort + 1 ?? 1, IsEnable = true };
            return res;
        }


        /// <summary>
        /// 获取树菜单
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<List<TreeDto>>> GetTreeAsync()
        {
            var res = new ApiResult<List<TreeDto>>();
            var treeList = new List<TreeDto>();
            try
            {
                var list = await Db.Queryable<SysMenu>().ToListAsync();
                var plist = list.Where(m => m.Type == 1).OrderBy(m => m.Sort);

                foreach (var m in plist)
                {
                    var children = GetTreeChildMenu(list, new List<TreeDto>(), m.ID);
                    treeList.Add(new TreeDto
                    {
                        id = m.ID,
                        title = m.Name,
                        children = children
                    });
                }
                res.data = treeList;
                return res;
            }
            catch (Exception ex)
            {
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                res.statusCode = (int)ApiEnum.Error;
            }
            return res;
        }
        private List<TreeDto> GetTreeChildMenu(List<SysMenu> sourceList, List<TreeDto> list, string pid)
        {
            var plist = sourceList.Where(m => m.ParentId == pid && m.Type != 3).OrderBy(m => m.Sort);
            foreach (var m in plist)
            {
                var children = GetTreeChildMenu(sourceList, new List<TreeDto>(), m.ID);
                list.Add(new TreeDto
                {
                    id = m.ID,
                    title = m.Name,
                    children = children
                });
            }
            return list;
        }


        /// <summary>
        /// 获得树表格列表
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<List<SysMenuTreeTableDto>>> GetTreeTableAsync()
        {
            var res = new ApiResult<List<SysMenuTreeTableDto>>();
            var treeList = new List<SysMenuTreeTableDto>();
            try
            {
                var list = await Db.Queryable<SysMenu>().ToListAsync();

                var plist = list.Where(m => m.Type == 1).OrderBy(m => m.Sort).ToList();

                foreach (var m in plist)
                {
                    //获得子级
                    var children = TreeTableChildMenu(list, new List<SysMenuTreeTableDto>(), m.ID);
                    treeList.Add(new SysMenuTreeTableDto
                    {
                        id = m.ID,
                        createTime = m.CreateTime,
                        creator = m.Creator,
                        enCode = m.EnCode,
                        icon = m.Icon,
                        iconColor = m.IconColor,
                        isDeskTop = m.IsDeskTop,
                        isEnable = m.IsEnable,
                        name = m.Name,
                        openType = m.OpenType,
                        parentId = m.ParentId,
                        parentName = m.ParentName,
                        remark = m.Remark,
                        sort = m.Sort,
                        type = m.Type,
                        url = m.Url,
                        children = children
                    });
                }
                res.data = treeList;
                return res;
            }
            catch (Exception ex)
            {
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                res.statusCode = (int)ApiEnum.Error;
            }
            return res;
        }
        private List<SysMenuTreeTableDto> TreeTableChildMenu(List<SysMenu> sourceList, List<SysMenuTreeTableDto> list, string pid)
        {
            var plist = sourceList.Where(m => m.ParentId == pid).OrderBy(m => m.Sort);
            foreach (var m in plist)
            {
                var children = TreeTableChildMenu(sourceList, new List<SysMenuTreeTableDto>(), m.ID);

                list.Add(new SysMenuTreeTableDto
                {
                    id = m.ID,
                    createTime = m.CreateTime,
                    creator = m.Creator,
                    enCode = m.EnCode,
                    icon = m.Icon,
                    iconColor = m.IconColor,
                    isDeskTop = m.IsDeskTop,
                    isEnable = m.IsEnable,
                    name = m.Name,
                    openType = m.OpenType,
                    parentId = m.ParentId,
                    parentName = m.ParentName,
                    remark = m.Remark,
                    sort = m.Sort,
                    type = m.Type,
                    url = m.Url,
                    children = children
                });
            }
            return list;
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> ModifyAsync(SysMenu model)
        {
            var res = new ApiResult<string>();
            try
            {
                //判断别名是否存在，要不一样的
                var count = Db.Queryable<SysMenu>().Where(m => m.EnCode == model.EnCode && m.ID!=model.ID && m.ParentId == model.ParentId).Count();
                if (count > 0)
                {
                    res.statusCode = (int)ApiEnum.Error;
                    res.msg = "别名已存在";
                    return await Task.Run(() => res);
                }
                if (!string.IsNullOrEmpty(model.ParentId) && model.ParentId != "0")
                {
                    // 说明有父级  根据父级，查询对应的模型
                    var pmodel = SysMenuDb.GetById(model.ParentId);
                    model.ParentName = pmodel.Name;
                }
                await Db.Updateable(model).ExecuteCommandAsync();
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
        /// 获得菜单列表，提供给权限管理，根据角色查询所有菜单
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<List<SysMenuDto>>> GetMenuByRole(string role)
        {
            var res = new ApiResult<List<SysMenuDto>>();
            try
            {
                res.data = await Db.Queryable<SysMenu>()
                        .OrderBy(m => m.Sort)
                        .Select(m => new SysMenuDto()
                        {
                            ID = m.ID,
                            parentID = m.ParentId,
                            name = m.Name,
                            type = m.Type,
                            icon = m.Icon
                        })
                        .Mapper((it, cache) =>
                        {
                            //根据角色查询已授权的选项
                            var codeList = cache.Get(t =>
                            {
                                return Db.Queryable<SysCode>().Where(m => m.CodeTypeId == "a88fa4d3-3658-4449-8f4a-7f438964d716").ToList();
                            });
                            var menuList = cache.Get(t =>
                            {
                                return Db.Queryable<SysPermissions>().Where(m => m.ID == role && m.RoleType == 1).ToList();
                            });

                            //判断菜单权限里是否包含当前按钮权限
                            var permissionModel = menuList.Find(g => g.MenuId == it.ID && g.ID == role);
                            if (permissionModel != null)
                            {
                                it.isChecked = true;
                            }
                        }).ToListAsync();
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
        /// 修改为置顶桌面
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="isDeskTop"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> ModifyAsync(string ID, bool isDeskTop)
        {
            var res = new ApiResult<string>
            {
                statusCode = 200
            };
            await Db.Updateable<SysMenu>().SetColumns(m => m.IsDeskTop == isDeskTop).Where(m => m.ID == ID).ExecuteCommandAsync();
            return res;
        }


        /// <summary>
        /// 获取授权的树菜单列表
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<ApiResult<List<SysMenuRoleTreeDto>>> GetMenuRoleTreeAsync(string roleId)
        {
            var res = new ApiResult<List<SysMenuRoleTreeDto>>();
            var treeList = new List<SysMenuRoleTreeDto>();
            try
            {
                var list = await Db.Queryable<SysMenu>().ToListAsync();
                var plist = list.Where(m => m.Type == 1).OrderBy(m => m.Sort);

                foreach (var m in plist)
                {
                    //获得子级
                    var children = RoleMenuTreeChildMenu(list, new List<SysMenuRoleTreeDto>(), m.ID, roleId);
                    var model = new SysMenuRoleTreeDto
                    {
                        id = m.ID,
                        title = m.Name,
                        children = children
                    };
                    var isExist = Db.Queryable<SysPermissions>().Where(x => x.MenuId == m.ID && x.RoleId == roleId).Count() > 0 ? true : false;

                    if (children.Count == 0 && isExist)
                    {
                        model.isChecked = true;
                    }
                    treeList.Add(model);
                }
                res.data = treeList;
                return res;
            }
            catch (Exception ex)
            {
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                res.statusCode = (int)ApiEnum.Error;
            }
            return res;
        }
        private List<SysMenuRoleTreeDto> RoleMenuTreeChildMenu(List<SysMenu> sourceList, List<SysMenuRoleTreeDto> list, string pid, string roleId)
        {
            var plist = sourceList.Where(m => m.ParentId == pid).OrderBy(m => m.Sort);
            foreach (var m in plist)
            {
                var children = RoleMenuTreeChildMenu(sourceList, new List<SysMenuRoleTreeDto>(), m.ID, roleId);
                var model = new SysMenuRoleTreeDto
                {
                    id = m.ID,
                    title = m.Name,
                    children = children
                };
                var isExist = Db.Queryable<SysPermissions>().Where(x => x.MenuId == m.ID && x.RoleId == roleId).Count() > 0 ? true : false;

                if (children.Count == 0 && isExist)
                {
                    model.isChecked = true;
                }
                list.Add(model);
            }
            return list;
        }

    }
}
