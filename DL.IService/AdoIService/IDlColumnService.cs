using DL.Domain.Dto.AdminDto.AdoDto;
using DL.Domain.Models.SysModels;
using DL.Domain.PublicModels;
using DL.IService.SysIService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DL.IService.AdoIService
{

    public interface ISysColumnService: IBaseService<SysColumn>
	{
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="parm">CmsColumn</param>
        /// <returns></returns>
        List<SysColumn> RecursiveModule(List<SysColumn> list);

        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <param name="parm">T</param>
        /// <returns></returns>
        Task<ApiResult<string>> UpdateAsync(SysColumn parm);

        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <param name="parm">T</param>
        /// <returns></returns>
        Task<ApiResult<string>> AddAsync(SysColumn parm);

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="p">父级</param>
        /// <param name="i">当前id</param>
        /// <param name="o">排序方式</param>
        /// <returns></returns>
        Task<ApiResult<string>> ColSort(string p,string i,int o);

        /// <summary>
        /// 栏目Tree
        /// </summary>
        /// <param name="parm">T</param>
        /// <returns></returns>
        Task<ApiResult<List<ColumnTree>>> TreeAsync(int type);
	}
}