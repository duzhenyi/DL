using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using DL.IService.SysIService;
using DL.Domain.PublicModels;
using DL.Domain.Models.SysModels;
using DL.Utils.Extensions;
using DL.Utils.Helper;
using DL.Domain.Dto.AdminDto.SysDto;
using DL.Utils.Log.Nlog;

namespace DL.Service.SysService
{
    /// <summary>
    /// 角色关联菜单的实现
    /// </summary>
    public class SysPermissionsService : DbContext, ISysPermissionsService
    {
        /// <summary>
        /// 用户授权角色
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> ToRoleAsync(SysPermissions parm, bool status)
        {
            var res = new ApiResult<string>
            {
                statusCode = 200,
                data = "1"
            };
            try
            {
                if (status)
                {
                    //授权
                    var dbres = await Db.Insertable<SysPermissions>(new SysPermissions()
                    {
                        ID = parm.ID,
                        AdminId = parm.AdminId,
                        MenuId = parm.MenuId,
                        RoleType = parm.RoleType
                    }).ExecuteCommandAsync();

                    if (dbres == 0)
                    {
                        res.statusCode = (int)ApiEnum.Error;
                        res.msg = "插入数据失败~";
                    }
                }
                else
                {
                    //取消授权
                    if (parm.RoleType == 2)
                    {
                        await Db.Deleteable<SysPermissions>()
                                .Where(m => m.AdminId == parm.AdminId && m.ID == parm.ID && m.RoleType == 2)
                                .ExecuteCommandAsync();
                    }
                    if (parm.RoleType == 3)
                    {
                        //角色-菜单-按钮功能
                        await Db.Deleteable<SysPermissions>()
                                .ExecuteCommandAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                res.statusCode = (int)ApiEnum.Error;
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                //LoggerHelper.Default.ProcessError((int)ApiEnum.Error, ex.Message);
            }
            return res;
        }

        /// <summary>
        /// 根据角色ID查询授权的菜单
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public async Task<ApiResult<List<SysPermissions>>> GetListAsync(string roleID)
        {
            var res = new ApiResult<List<SysPermissions>>
            {
                statusCode = 200
            };
            try
            {
                res.data = await Db.Queryable<SysPermissions>()
                        .WhereIF(!string.IsNullOrEmpty(roleID), m => m.ID == roleID).ToListAsync();
            }
            catch (Exception ex)
            {
                res.statusCode = (int)ApiEnum.Error;
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
            }
            return res;
        }

        /// <summary>
        /// 保存授权菜单
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> SaveAsync(SysPermissions parm)
        {
            var res = new ApiResult<string>();

            try
            {
                var newSysPermissions = new List<SysPermissions>();
                var arry = UtilsHelper.SplitString(parm.MenuId, ',');
                foreach (var item in arry)
                {
                    newSysPermissions.Add(new SysPermissions()
                    {
                        ID = Guid.NewGuid().ToString(),
                        CreateTime = DateTime.Now,
                        IsEnable = true,
                        MenuId = item,
                        RoleType = parm.RoleType,
                        RoleId = parm.RoleId
                    });
                }
                var result = await Db.Ado.UseTranAsync(() =>
                {
                    //1. 删除该角色下配置的所有菜单信息
                    Db.Deleteable<SysPermissions>()
                      .Where(m => m.RoleId == parm.RoleId && m.RoleType == parm.RoleType)
                      .ExecuteCommand();

                    //2. 添加该角色下配置的新的菜单信息
                    if (newSysPermissions.Count > 0)
                    {
                        Db.Insertable<SysPermissions>(newSysPermissions).ExecuteCommand();
                    }
                });

                if (!result.IsSuccess)
                {
                    res.statusCode = (int)ApiEnum.Error;
                    res.msg = "插入数据失败~";
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
        /// 菜单授权-菜单功能
        /// role=角色
        /// funID=按钮的编号
        /// status=取消还是授权
        /// </summary>
        /// <returns></returns>
        public ApiResult<string> RoleMenuToFunAsync(SysPermissionsParm parm)
        {
            var res = new ApiResult<string>() { statusCode = (int)ApiEnum.Error };
            try
            {
                //根据角色和菜单查询内容
                var model = Db.Queryable<SysPermissions>().Single(m => m.ID == parm.role
                && m.MenuId == parm.menu && m.RoleType == 1);
                if (model == null)
                {
                    res.msg = "您还没有授权当前菜单功能模块";
                    return res;
                }

                Db.Updateable<SysPermissions>()
                    .Where(m => m.ID == parm.role
                && m.MenuId == parm.menu && m.RoleType == 1).ExecuteCommand();
                res.statusCode = (int)ApiEnum.Status;
            }
            catch (Exception ex)
            {
                res.msg = ex.Message;
                //  LoggerHelper.Default.ProcessError((int)ApiEnum.Error, ex.Message);
            }
            return res;
        }

        /// <summary>
        /// 权限管理-保存角色和授权菜单以及功能
        /// </summary>
        /// <returns></returns>
        public ApiResult<string> SaveAuthorization(List<SysMenuDto> list, string roleID)
        {
            var res = new ApiResult<string>() { statusCode = (int)ApiEnum.Error };
            try
            {
                if (list.Count == 0 && string.IsNullOrEmpty(roleID))
                {
                    res.msg = ApiEnum.ParameterError.GetEnumText();
                    return res;
                }
                //查询有有数据的菜单
                var newList = new List<SysMenuDto>();
                foreach (var item in list)
                {
                    if (item.isChecked)
                    {
                        newList.Add(item);
                    }
                }
                if (newList.Count == 0)
                {
                    res.msg = ApiEnum.ParameterError.GetEnumText();
                    return res;
                }
                //构建新的保存数组
                var dbList = new List<SysPermissions>();
                foreach (var item in newList)
                {
                    dbList.Add(new SysPermissions()
                    {
                        ID = roleID,
                        MenuId = item.ID,
                        RoleType = 1
                    });
                }

                var result = Db.Ado.UseTran(async () =>
                {
                    //根据角色删除已有的，添加新的
                    await Db.Deleteable<SysPermissions>().Where(m => m.ID == roleID && m.RoleType == 1).ExecuteCommandAsync();
                    //增加新的
                    await Db.Insertable(dbList).ExecuteCommandAsync();
                });
                if (!result.IsSuccess)
                {
                    res.msg = result.ErrorMessage;
                }
                else
                {
                    res.statusCode = (int)ApiEnum.Status;
                }
            }
            catch (Exception ex)
            {
                res.msg = ex.Message;
                //  LoggerHelper.Default.ProcessError((int)ApiEnum.Error, ex.Message);
            }
            return res;
        }
    }
}
