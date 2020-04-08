using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using DL.IService.SysIService;
using DL.Domain.PublicModels;
using DL.Utils.Security;
using DL.Domain.Models.SysModels;
using DL.Utils.Extensions;
using DL.Utils.Helper;
using DL.Domain.Dto.AdminDto.SysDto;
using DL.Utils.Log.Nlog;

namespace DL.Service.SysService
{
    public class SysAdminService : BaseService<SysAdmin>, ISysAdminService
    {
        #region  用户登录和授权菜单查询
        /// <summary>
        /// 用户登录实现
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<SysAdminMenuDto>> LoginAsync(SysAdminLogin parm)
        {
            var res = new ApiResult<SysAdminMenuDto>();
            try
            {
                var adminModel = new SysAdminMenuDto();
                parm.password = DES3Encrypt.EncryptString(parm.password);
                var model = await Db.Queryable<SysAdmin>().Where(m => m.Account == parm.loginname && m.IsEnable).FirstAsync();
                if (model == null)
                {
                    res.msg = "账号错误";
                    return res;
                }
                if (!model.Pwd.Equals(parm.password))
                {
                    res.msg = "密码错误";
                    return res;
                }
                if (!model.IsEnable)
                {
                    res.msg = "登录账号被冻结，请联系管理员";
                    return res;
                }
                adminModel.menu = GetMenuByAdmin(model.RoleId);
                if (adminModel == null)
                {
                    res.msg = "当前账号没有授权功能模块，无法登录~";
                    return res;
                }
                //修改登录时间
                model.LoginTime = DateTime.Now;
                model.LastLoginTime = model.LoginTime;
                model.LoginCount = model.LoginCount + 1;
                SysAdminDb.Update(model);

                res.statusCode = (int)ApiEnum.Status;
                adminModel.admin = model;
                res.data = adminModel;
            }
            catch (Exception ex)
            {
                res.msg = ex.Message;
                NLogHelper.Error(ex.Message);
            }
            return res;
        }
        /// <summary>
        /// 根据登录账号，返回菜单信息
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        private List<SysMenuDto> GetMenuByAdmin(string roleId)
        {
            var res = new List<SysMenuDto>();
            try
            {
                //根据角色查询菜单，并查询到菜单涉及的功能
                var query = Db.Queryable<SysMenu, SysPermissions>((m, p) => new object[]{
                    JoinType.Left,m.ID==p.MenuId
                })
                .Where((m, p) => p.RoleId == roleId && p.RoleType == 1 && m.IsEnable && p.IsEnable)
                .OrderBy((m, p) => m.Sort)
                .Select((m, p) => new SysMenuDto()
                {
                    ID = m.ID,
                    parentID = m.ParentId,
                    parentName = m.ParentName,
                    name = m.Name,
                    nameCode = m.EnCode,
                    type = m.Type,
                    url = m.Url,
                    icon = m.Icon,
                    iconColor = m.IconColor,
                    isDeskTop = m.IsDeskTop,
                    sort = m.Sort
                });
                res = query.ToList();
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex.Message);
                res = null;
            }
            return res;
        }
        #endregion

        /// <summary>
        /// 添加账号相关信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> AddAsync(SysAdmin parm)
        {
            var res = new ApiResult<string>();
            try
            {
                var isExist = await Db.Queryable<SysAdmin>().AnyAsync(m => m.Account == parm.Account);
                if (isExist)
                {
                    res.msg = "用户名已存在，请更换!";
                    return res;
                }
                parm.Pwd = DES3Encrypt.EncryptString(parm.Pwd);
                parm.ID = Guid.NewGuid().ToString();
                parm.CreateTime = DateTime.Now;
                if (string.IsNullOrEmpty(parm.HeadPic))
                {
                    parm.HeadPic = "/themes/img/avatar.jpg";
                }

                var dbRes = SysAdminDb.Insert(parm);
                if (dbRes)
                {
                    res.msg = "添加成功";
                }
                else
                {
                    res.msg = "添加失败";
                }

            }
            catch (Exception ex)
            {
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                NLogHelper.Error(ex.Message);
            }
            return await Task.Run(() => res);
        }

        /// <summary>
        /// 删除账号相关信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> DeleteAsync(string parm)
        {
            var list = UtilsHelper.StrToListString(parm);
            var isok = await Db.Deleteable<SysAdmin>().Where(m => list.Contains(m.ID)).ExecuteCommandAsync();
            var res = new ApiResult<string>
            {
                statusCode = isok > 0 ? 200 : 500,
                data = isok > 0 ? "1" : "0",
                msg = isok > 0 ? "删除成功~" : "删除失败~"
            };
            return res;
        }


        /// <summary>
        /// 获得列表
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<PageReply<SysAdminDto>>> GetPagesAsync(PageParm parm, string organizeId)
        {
            var res = new ApiResult<PageReply<SysAdminDto>>();
            try
            {
                res.data = await Db.Queryable<SysAdmin, SysRole, SysOrganize>((a, r, o) => new object[]
                  {
                    JoinType.Left, a.RoleId == r.ID,
                    JoinType.Left, a.OrganizeId == o.ID
                  })
                  .WhereIF(!string.IsNullOrEmpty(organizeId), a => a.OrganizeId == organizeId)
                  .OrderBy(a => a.CreateTime)
                  .Select((a, r, o) => new SysAdminDto
                  {
                      ID = a.ID,
                      Account = a.Account,
                      CreateTime = a.CreateTime,
                      Creator = a.Creator,
                      Email = a.Email,
                      HeadPic = a.HeadPic,
                      IDCard = a.IDCard,
                      IsEnable = a.IsEnable,
                      LastLoginTime = a.LastLoginTime,
                      LoginCount = a.LoginCount,
                      LoginTime = a.LoginTime,
                      Mobile = a.Mobile,
                      OrganizeId = a.OrganizeId,
                      OrganizeName = o.Name,
                      Pwd = a.Pwd,
                      RelName = a.RelName,
                      Remark = a.Remark,
                      RoleId = a.RoleId,
                      RoleName = r.Name,
                      Sex = a.Sex,
                  })
                  .ToPageAsync(parm.page, parm.limit);
            }
            catch (Exception ex)
            {
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                NLogHelper.Error(ex.Message);
            }
            return res;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> ModifyAsync(SysAdmin parm)
        {
            var res = new ApiResult<string>();
            try
            {
                //修改，判断用户是否和其它的重复
                var isExisteName = await Db.Queryable<SysAdmin>().AnyAsync(m => m.Account == parm.Account && m.ID != parm.ID);
                if (isExisteName)
                {
                    res.msg = "用户名已存在，请更换";
                    return await Task.Run(() => res);
                }
                parm.Pwd = DES3Encrypt.EncryptString(parm.Pwd);
                var dbres = await Db.Updateable(parm).ExecuteCommandAsync();
                if (dbres > 0)
                {
                    res.msg = "更新成功！";
                }
                else
                {
                    res.msg = "更新失败！";
                }

            }
            catch (Exception ex)
            {
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                NLogHelper.Error(ex.Message);
            }
            return res;
        }

        /// <summary>
        /// 获取一个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResult<SysAdminDto>> GetModelAsync(string id)
        {
            var res = new ApiResult<SysAdminDto>();
            try
            {

                res.data = await Db.Queryable<SysAdmin, SysOrganize>((admin, organize) => new object[]
                                    {
                                       JoinType.Left,admin.OrganizeId==organize.ID
                                    })
                                   .Where((admin, organize) => admin.ID == id)
                                   .Select((admin, organize) => new SysAdminDto
                                   {
                                       ID = admin.ID,
                                       Account = admin.Account,
                                       CreateTime = admin.CreateTime,
                                       Creator = admin.Creator,
                                       Email = admin.Email,
                                       HeadPic = admin.HeadPic,
                                       IDCard = admin.IDCard,
                                       IsEnable = admin.IsEnable,
                                       LastLoginTime = admin.LastLoginTime,
                                       LoginCount = admin.LoginCount,
                                       LoginTime = admin.LoginTime,
                                       Mobile = admin.Mobile,
                                       OrganizeId = admin.OrganizeId,
                                       OrganizeName = organize.Name,
                                       Pwd = admin.Pwd,
                                       RelName = admin.RelName,
                                       Remark = admin.Remark,
                                       RoleId = admin.RoleId,
                                       Sex = admin.Sex
                                   })
                                   .SingleAsync();
            }
            catch (Exception ex)
            {
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                NLogHelper.Error(ex.Message);
            }
            return res;
        }

    }
}
