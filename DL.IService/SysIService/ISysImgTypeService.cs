using DL.Domain.Dto.AdminDto.SysDto;
using DL.Domain.Models.SysModels;
using DL.Domain.PublicModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DL.IService.SysIService
{
    public interface ISysImgTypeService : IBaseService<SysImgType>
    {
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<string>> AddAsync(SysImgType model);

        /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<ApiResult<string>> DelAsync(string ids);

        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<string>> ModifyAsync(SysImgType model);

        /// <summary>
        /// 获得树列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<TreeDto>>> GetListTreeAsync();
    }
}
