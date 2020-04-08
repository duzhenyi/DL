using DL.Domain.Dto.AdminDto.SysDto;
using DL.Domain.Models.SysModels;
using DL.Domain.PublicModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DL.IService.SysIService
{
    /// <summary>
    /// 系统菜单业务接口
    /// </summary>
    public interface ISysMenuService : IBaseService<SysMenu>
    {
        /// <summary>
        /// 获得菜单列表，提供给权限管理，根据角色查询所有菜单
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<SysMenuDto>>> GetMenuByRole(string role);

        /// <summary>
        /// 获取授权的树菜单列表
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<ApiResult<List<SysMenuRoleTreeDto>>>  GetMenuRoleTreeAsync(string roleId);

        /// <summary>
        /// 获取树菜单
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<TreeDto>>> GetTreeAsync();


        /// <summary>
        /// 获得树表格列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<SysMenuTreeTableDto>>> GetTreeTableAsync();

        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<SysMenu>> GetByIDAsync(string parm);

        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<string>> AddAsync(SysMenu model);

        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<string>> ModifyAsync(SysMenu model);

        /// <summary>
        /// 修改置顶
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <param name="isDeskTop">是否置顶</param>
        /// <returns></returns>
        Task<ApiResult<string>> ModifyAsync(string id, bool isDeskTop);

    }
}
