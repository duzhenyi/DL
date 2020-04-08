using DL.Domain.Models.SysModels;
using DL.IService.SysIService;
using DL.Utils.Autofac;
using DL.Utils.Extensions;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DL.Service.SysService
{
    public class SysTableService : BaseService<SysTable>, ISysTableService
    {
        public string CreateEntityModel(SysTable sysTable)
        {
            throw new NotImplementedException();
        }

        public string CreatePage(SysTable sysTable)
        {
            throw new NotImplementedException();
        }

        public string CreateServices(string tableName, string nameSpace, string foldername, bool webController, bool apiController)
        {
            throw new NotImplementedException();
        }

        public string CreateVuePage(SysTable sysTable, string vuePath)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DelTree(int table_Id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取生成配置的树开菜单
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetTableTree()
        {
            var treeData = await Db.Queryable<SysTable>()
                .Select(c => new
                {
                    id = c.ID,
                    pId = c.ParentId,
                    parentId = c.ParentId,
                    name = c.ColumnCNName,
                    orderNo = c.OrderNo
                }).OrderBy(c => c.orderNo).ToListAsync();
            
            var treeList = treeData.Select(a => new
            {
                a.id,
                a.pId,
                a.parentId,
                a.name, 
            });

            var projectPath = Directory.GetParent(AutofacHelper.GetService<IHostingEnvironment>().ContentRootPath).ToString();

            string startsWith = projectPath.Substring(0, projectPath.IndexOf('.'));

            return treeList.Count() == 0 ? "[]" : JsonConvert.SerializeObject(treeList) ?? ""; 
        }

        /// <summary>
        /// 设置界面table td单元格的宽度
        /// </summary>
        /// <param name="columns"></param>
        private void SetMaxLength(List<SysTableColumn> columns)
        {
            columns.ForEach(x =>
            {
                if (x.ColumnName == "DateTime")
                {
                    x.ColumnWidth = 150;
                }
                else if (x.ColumnName == "Modifier" || x.ColumnName == "Creator")
                {
                    x.ColumnWidth = 130;
                }
                else if (x.ColumnName == "CreateID" || x.ColumnName == "ModifyID")
                {
                    x.ColumnWidth = 80;
                }
                else if (x.Maxlength > 200)
                {
                    x.ColumnWidth = 220;
                }
                else if (x.Maxlength > 110 && x.Maxlength <= 200)
                {
                    x.ColumnWidth = 180;
                }
                else if (x.Maxlength > 60 && x.Maxlength <= 110)
                {
                    x.ColumnWidth = 120;
                }
                else
                {
                    x.ColumnWidth = 90;
                }
            });
        }


        /// <summary>
        /// 界面加载表的配置信息
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="tableName"></param>
        /// <param name="columnCNName"></param>
        /// <param name="nameSpace"></param>
        /// <param name="foldername"></param>
        /// <param name="table_Id"></param> 
        /// <returns></returns>
        public object LoadTable(int parentId, string tableName, string columnCNName, string nameSpace, string foldername, string tableId)
        { 
            if (string.IsNullOrEmpty(tableName))
                return "";

            tableId = Db.Queryable<SysTable>().Where(m => m.TableName == tableName).Select(m => m.ID).First();
              
            SysTable sysTable  = new SysTable()
            {
                ParentId = parentId,
                ColumnCNName = columnCNName,
                CnName = columnCNName,
                TableName = tableName,
                Namespace = nameSpace,
                FolderName = foldername,
                Enable = 1
            };

            List<SysTableColumn> columns = Db.Ado.SqlQuery<SysTableColumn>(GetStructure(tableName));
            SetMaxLength(columns);

            //base.Add<SysTableColumn>(sysTable, columns, false);
            int maxColumnId = columns.Count == 0 ? 1 : columns.Max(x => x.ColumnId);

            int idLength = maxColumnId.ToString().Length + 2;
            int orderNo = "100000000000000000000".Substring(0, idLength).ObjToInt();
            columns.ForEach(x =>
            {
                x.OrderNo = orderNo - x.ColumnId * 10 - 10;
            });
            return sysTable.ID;
        }
         

        public bool SaveEidt(SysTable sysTable)
        {
            throw new NotImplementedException();
        }
    }
}
