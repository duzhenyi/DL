using DL.Domain.Dto.AdminDto.SysDto;
using DL.Domain.Models.SysModels;
using DL.Domain.PublicModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DL.IService.SysIService
{
    public interface ISysImgService : IBaseService<SysImage>
    {
        Task<ApiResult<PageReply<SysImage>>> GetList(SysImgPageParmDto parm);
    }
}
