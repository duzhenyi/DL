using DL.Domain.Dto.AdminDto.AdoDto;
using DL.Domain.Models.AdoModels;
using DL.Domain.PublicModels;
using DL.IService.SysIService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DL.IService.AdoIService
{
    public interface IAdoClassTypeService : IBaseService<AdoClassType>
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns> 
        Task<ApiResult<PageReply<AdoClassType>>> GetPagesAsync(PageParmRqst parm);

        /// <summary>
        /// 获取树
        /// </summary>
        /// <param name="industryType">所属行业 0 为所有数据</param>
        /// <param name="isTop">是否置顶</param>
        /// <returns></returns>
        Task<ApiResult<List<IndustryTree>>> GetListTreeAsync(int industryType = 0, bool isTop = false);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<ApiResult<string>> AddAsync(AdoClassType parm);
    }
}