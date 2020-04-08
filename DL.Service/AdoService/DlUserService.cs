using System;
using System.Threading.Tasks;
using DL.Domain.Models.AdoModels;
using DL.Domain.PublicModels;
using DL.IService.AdoIService;
using DL.Utils.Extensions;
using DL.Utils.Helper;
using SqlSugar;

namespace DL.Service.AdoService
{
    public class AdoUserService : BaseService<AdoUser>, IAdoUserService
	{
		public async Task<ApiResult<PageReply<AdoUser>>> GetPagesAsync(PageParmRqst parm)
		{
			var res = new ApiResult<PageReply<AdoUser>>();
			try
			{
				string beginTime = string.Empty, endTime = string.Empty;
				if (!string.IsNullOrEmpty(parm.time))
				{
					var timeRes = UtilsHelper.SplitString(parm.time, '-');
					beginTime = timeRes[0].Trim();
					endTime = timeRes[1].Trim();
				}
				res.data = await Db.Queryable<AdoUser>()
					.WhereIF(!string.IsNullOrEmpty(parm.key), m => m.NickName.Contains(parm.key) || m.LoginAccount.Contains(parm.key) || m.QQ.Contains(parm.key) || m.WX.Contains(parm.key))
					.WhereIF(!string.IsNullOrEmpty(parm.time), m => m.LoginDate >= Convert.ToDateTime(beginTime) && m.LoginDate <= Convert.ToDateTime(endTime))
					.OrderBy(m => m.AddTime, OrderByType.Desc)
					.ToPageAsync(parm.page, parm.limit);
			}
			catch (Exception ex)
			{
				res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
				res.statusCode = (int)ApiEnum.Error;
				//LoggerHelper.Default.ProcessError((int)ApiEnum.Error, ex.Message);
			}
			return res;
		}
	}
}