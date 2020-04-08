using DL.Domain.Dto.AdminDto.SysDto;
using DL.Domain.Models.SysModels;
using DL.Domain.PublicModels;
using System.Threading.Tasks;

namespace DL.IService.SysIService
{
    /// <summary>
    /// 管理员接口
    /// </summary>
    public interface ISysAdminService : IBaseService<SysAdmin>
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<ApiResult<SysAdminMenuDto>> LoginAsync(SysAdminLogin parm);

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<PageReply<SysAdminDto>>> GetPagesAsync(PageParm parm, string organizeId);

        /// <summary>
        /// 获取实体信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResult<SysAdminDto>> GetModelAsync(string id);

        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<string>> AddAsync(SysAdmin parm);

        /// <summary>
        /// 删除一条或多条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<string>> DeleteAsync(string parm);

        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<string>> ModifyAsync(SysAdmin parm);
    }
}
