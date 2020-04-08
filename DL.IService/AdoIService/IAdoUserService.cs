using DL.Domain.Models.AdoModels;
using DL.Domain.PublicModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using DL.IService;
using DL.Domain.Dto.AdminDto.AdoModelsDto;
using DL.Domain.Dto.SiteDto;

namespace DL.IService.AdoIService
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public interface IAdoUserService : IBaseService<AdoUser>
    {
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<string>> AddAsync(AdoUser model);

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
        Task<ApiResult<string>> ModifyAsync(AdoUser model);

        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<AdoUser>> GetByIDAsync(string parm);

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<PageReply<AdoUser>>> GetPagesAsync(AdoUserPageParm pageParm);


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="adoUserLogin"></param>
        /// <returns></returns>
        Task<ApiResult<AdoUser>> LoginAsync(AdoUserLogin adoUserLogin);
    }
}
