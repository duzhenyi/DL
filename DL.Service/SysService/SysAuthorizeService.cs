using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
using Newtonsoft.Json;
using DL.Domain.PublicModels;
using DL.Domain.Dto.AdminDto;
using DL.IService.SysIService;
using DL.Domain.Models.SysModels;
using DL.Utils.Extensions;
using DL.Domain.Dto.AdminDto.SysDto;
using DL.Utils.Log.Log4net;

namespace DL.Service.SysService
{
    /// <summary>
    /// 根据登录账号，获得相应权限服务实现
    /// </summary>
    public class SysAuthorizeService : DbContext, ISysAuthorizeService
    {
        /// <summary>
        /// 根据登录账号，获得相应权限
        /// </summary>
        /// <returns></returns>
        public ApiResult<List<SysMenuDto>> GetAuthorizeAsync(string admin)
        {
            var res = new ApiResult<List<SysMenuDto>>() { statusCode = (int)ApiEnum.Error };
            try
            {
                //根据用户查询角色列表， 一个用户对应多个角色
                var roleList = SysPermissionsDb.GetList(m => m.ID == admin && m.RoleType == 2).Select(m => m.ID).ToList();
                //根据角色查询菜单，并查询到菜单涉及的功能
                var query = Db.Queryable<SysMenu, SysPermissions>((sm, sp) => new object[]{
                    JoinType.Left,sm.ID==sp.MenuId
                })
                .Where((sm, sp) => roleList.Contains(sp.ID) && sp.RoleType == 1 && sm.IsEnable )
                .OrderBy((sm, sp) => sm.Sort)
                .Select((sm, sp) => new SysMenuDto()
                {
                    ID = sm.ID,
                    parentID = sm.ParentId,
                    parentName = sm.ParentName,
                    name = sm.Name,
                   // nameCode = sm.EnCode, 
                    type = sm.Type,
                    url = sm.Url,
                    icon = sm.Icon,
                    sort = sm.Sort, 
                })
                .Mapper((it, cache) =>
                {
                    var codeList = cache.Get(list =>
                      {
                          return Db.Queryable<SysCode>().Where(m => m.CodeTypeId == "a88fa4d3-3658-4449-8f4a-7f438964d716")
                          .Select(m => new SysCodeDto()
                          {
                              ID = m.ID,
                              name = m.Name,
                              codeType = m.CodeTypeId
                          })
                          .ToList();
                      });

                });
                res.data = query.ToList();
                res.statusCode = (int)ApiEnum.Status;
            }
            catch (Exception ex)
            {
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                //LoggerHelper.Default.ProcessError((int)ApiEnum.Error, ex.Message);
            }
            return res;
        }

        /// <summary>
        /// 根据菜单，获得当前菜单的所有功能权限
        /// </summary>
        /// <returns></returns>
        public ApiResult<List<SysCodeDto>> GetCodeByMenu(string role, string menu)
        {
            var res = new ApiResult<List<SysCodeDto>>() { statusCode = (int)ApiEnum.Error };
            try
            {
                //获得角色权限ID-List
                var menuModel = SysMenuDb.GetSingle(m => m.ID == menu);
                if (menuModel == null)
                {
                    return new ApiResult<List<SysCodeDto>>();
                }
                //查询授权菜单里面的按钮功能
                var btnFunModel = SysPermissionsDb.GetSingle(m => m.ID == role && m.MenuId == menu && m.IsEnable);
                if (btnFunModel == null)
                {
                    return new ApiResult<List<SysCodeDto>>();
                }
                res.statusCode = (int)ApiEnum.Status;
            }
            catch (Exception ex)
            {
                res.msg = ex.Message;
                Log4netHelper.Error(typeof(string), ex.Message);
            }
            return res;
        }
    }
}
