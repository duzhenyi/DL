using System.Collections.Generic;
using System.Threading.Tasks;
using DL.Domain.Dto.AdminDto.SysDto;
using DL.Domain.Models.SysModels;
using DL.Domain.PublicModels;

namespace DL.IService.SysIService
{
    /// <summary>
    /// 角色业务接口
    /// </summary>
    public interface ISysRoleService:IBaseService<SysRole>
    {

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<ApiResult<string>> DelAsync(string ids);

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<PageReply<SysRole>>> GetPagesAsync(PageParm parm);

        /// <summary>
        /// 查询列表，并获得权限值状态
        /// </summary>
        /// <param name="deparmentID">所属部门</param>
        /// <param name="adminID">用户的唯一编号</param>
        /// <returns></returns>
        Task<ApiResult<PageReply<SysRoleDto>>> GetPagesToRoleAsync(int limit,string deparmentID, string adminID);


        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<string>> AddAsync(SysRole parm);


        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<string>> ModifyAsync(SysRole parm);
    }
}
