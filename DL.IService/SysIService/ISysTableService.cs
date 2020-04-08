using DL.Domain.Models.SysModels;
using System.Threading.Tasks;

namespace DL.IService.SysIService
{
    public interface ISysTableService : IBaseService<SysTable>
    {
        Task<string> GetTableTree();

        string CreateEntityModel(SysTable  sysTable);

        bool SaveEidt(SysTable sysTable);

        string CreateServices(string tableName, string nameSpace, string foldername, bool webController, bool apiController);

        string CreatePage(SysTable sysTable);

        string CreateVuePage(SysTable sysTable, string vuePath);

        object LoadTable(int parentId, string tableName, string columnCNName, string nameSpace, string foldername, string tableId);

        Task<bool> DelTree(int table_Id);
    }
}
