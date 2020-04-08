using DL.IService.SysIService;
using DL.Domain.Models.SysModels;
using DL.Domain.Dto.AdminDto.SysDto;
using DL.Domain.PublicModels;
using System.Collections.Generic;
using System;
using DL.Utils.Extensions;
using System.Threading.Tasks;

namespace DL.Service.SysService
{
    public class SysImgService : BaseService<SysImage>, ISysImgService
    {
        public async Task<ApiResult<PageReply<SysImage>>> GetList(SysImgPageParmDto parm)
        {
            var res = new ApiResult<PageReply<SysImage>>();
            try
            {
                res.data = await Db.Queryable<SysImage>()
                        .WhereIF(!string.IsNullOrEmpty(parm.typeId), m => m.SysImgTypeId == parm.typeId)
                        .WhereIF(!string.IsNullOrEmpty(parm.key), m => m.ImgBig.Contains(parm.key))
                        .OrderBy(m => m.CreateTime, SqlSugar.OrderByType.Desc)
                        .ToPageAsync(parm.page, parm.limit);

            }
            catch (Exception ex)
            {
                res.msg = ApiEnum.Error.GetEnumText() + ex.Message;
                res.statusCode = (int)ApiEnum.Error;
            }
            return res;
        }
    }
}
