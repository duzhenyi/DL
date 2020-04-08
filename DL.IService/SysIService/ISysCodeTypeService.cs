using DL.Domain.Dto.AdminDto.SysDto;
using DL.Domain.Models.SysModels;
using DL.Domain.PublicModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DL.IService.SysIService
{
    /// <summary>
    /// 字典分类接口
    /// </summary>
    public interface ISysCodeTypeService : IBaseService<SysCodeType>
    {

        /// <summary>
        /// 获得树列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<SysCodeTypeTree>>> GetListTreeAsync();

        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<SysCodeTypeDto>> GetByIDAsync(string pid);

        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<string>> AddAsync(SysCodeType model);


        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<string>> ModifyAsync(SysCodeType model);
    }
}
