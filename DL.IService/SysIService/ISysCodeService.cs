using DL.Domain.Dto.AdminDto.SysDto;
using DL.Domain.Models.SysModels;
using DL.Domain.PublicModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DL.IService.SysIService
{
    /// <summary>
    /// 字典值接口
    /// </summary>
    public interface ISysCodeService : IBaseService<SysCode>
    {
        /// <summary>
        /// 获得列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<PageReply<SysCode>>> GetPagesAsync(PageParm request);

        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<SysCode>> GetByIDAsync(string id);

        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<string>> AddAsync(SysCode parm);


        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<string>> ModifyAsync(SysCode parm);

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<string>> ModifyStatusAsync(SysCode parm);

        Task<ApiResult<List<ItemObjDto>>> GetListAsync(string parentIDId);
    }
}
