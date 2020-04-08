using DL.Domain.Dto.AdminDto.BuilderDto;
using DL.Domain.PublicModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DL.IService.BuilderIService
{
    public interface IGenerateService
    {
       ApiResult<List<string>> GetTable(DbDto parm);

       List<DataFieldDto> GetField(DbDto parm);
    }
}
