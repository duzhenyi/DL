using DL.Domain.Models.AdoModels;
using DL.Domain.PublicModels;
using DL.IService.SysIService;

namespace DL.IService.AdoIService
{

    public interface IAdoTagsService: IBaseService<AdoTags>
	{
        PageReply<AdoTags> GetList(PageParmRqst parm);
    }
}