using DL.Domain.Models.AdoModels;
using DL.Domain.PublicModels;
using DL.IService.SysIService;
using System.Threading.Tasks;

namespace DL.IService.AdoIService
{
    public interface IAdoUserService: IBaseService<AdoUser>
	{
		/// <summary>
		/// 获取数据
		/// </summary>
		/// <param name="parm"></param>
		/// <returns></returns> 
		Task<ApiResult<PageReply<AdoUser>>> GetPagesAsync(PageParmRqst parm);

	}
}