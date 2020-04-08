using DL.Domain.Dto.AdminDto.BuilderDto;
using DL.Domain.PublicModels;
using DL.IService.BuilderIService;
using SqlSugar;
using System.Collections.Generic;
using System.Linq;

namespace DL.Service.BuilderService
{
    public class GenerateService : DbContext, IGenerateService
    {
        /// <summary>
        /// 获取数据库表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public ApiResult<List<string>> GetTable(DbDto parm)
        {
            DbType dbType;
            var sql = string.Empty;
            switch (parm.DbType)
            {
                case 0://sqlserver
                    dbType = DbType.SqlServer;
                    sql = "SELECT name FROM sysobjects WHERE xtype='U' AND name  <>  'dtproperties' order by name asc";

                    #region 注释 
                    /*
                     * select name from sysobjects where xtype='u' ---
                        C = CHECK 约束
                        D = 默认值或 DEFAULT 约束
                        F = FOREIGN KEY 约束
                        L = 日志
                        FN = 标量函数
                        IF = 内嵌表函数
                        P = 存储过程
                        PK = PRIMARY KEY 约束（类型是 K）
                        RF = 复制筛选存储过程
                        S = 系统表
                        TF = 表函数
                        TR = 触发器
                        U = 用户表
                        UQ = UNIQUE 约束（类型是 K）
                        V = 视图
                        X = 扩展存储过程
                     */
                    #endregion
                    break;
                case 1://mysql
                    dbType = DbType.MySql;
                    break;
                case 2://oracle
                    dbType = DbType.Oracle;
                    break;
                default:
                    dbType = DbType.SqlServer;
                    break;
            }

            var res = new ApiResult<List<string>>();

            using (var sqlDB = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = parm.DbConn,
                DbType = dbType,
                IsAutoCloseConnection = true
            }))
            {
                res.data = sqlDB.Ado.SqlQuery<string>(sql);
            }

            return res;
        }


        /// <summary>
        /// 获取表字段
        /// </summary>
        /// <param name="dbDto"></param>
        /// <returns></returns>
        public List<DataFieldDto> GetField(DbDto dbDto)
        {
            DbType dbType;
            var sql = string.Empty;

            switch (dbDto.DbType)
            {
                case 0://sqlserver
                    dbType = DbType.SqlServer;
                    sql = @"SELECT
                            TbName =case when a.colorder = 1 then d.name else '' end, 
                            TbDesc =case when a.colorder = 1 then isnull(f.value,'') else '' end,
                            FieldIndex = a.colorder, 
                            FieldName = a.name, 
                            IsIdentity =case when COLUMNPROPERTY(a.id, a.name, 'IsIdentity') = 1 then '√'else '' end, 
                            IsKey =case when exists(SELECT 1 FROM sysobjects where xtype = 'PK' and name in (
                                 SELECT name FROM sysindexes WHERE indid in(
                                  SELECT indid FROM sysindexkeys WHERE id = a.id AND colid = a.colid
                               ))) then '√' else '' end, 
                            FiledType = b.name, 
                            ByteLength = a.length, 
                            FiledLength = COLUMNPROPERTY(a.id, a.name, 'PRECISION'), 
                            DecimalLength = isnull(COLUMNPROPERTY(a.id, a.name, 'Scale'), 0), 
                            IsNullable =case when a.isnullable = 1 then '√'else '' end, 
                            DefaultVal = isnull(e.text, ''), 
                            FieldDesc = isnull(g.[value], '')
                            FROM syscolumns a
                            left join systypes b on a.xtype = b.xusertype
                            inner join sysobjects d on a.id = d.id and d.xtype = 'U' and d.name <> 'dtproperties'
                            left join syscomments e on a.cdefault = e.id
                            left join sys.extended_properties g on a.id = g.major_id and a.colid = g.minor_id
                            left join sys.extended_properties f on d.id = f.major_id and f.minor_id = 0
                            where d.name = @TableName 
                            order by a.id,a.colorder";
                    break;
                case 1://mysql
                    dbType = DbType.MySql;
                    break;
                case 2://oracle
                    dbType = DbType.Oracle;
                    break;
                default:
                    dbType = DbType.SqlServer;
                    break;
            }

            var res = new List<DataFieldDto>();

            using (var sqlDB = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = dbDto.DbConn,
                DbType = dbType,
                IsAutoCloseConnection = true
            }))
            {
                res = sqlDB.Ado.SqlQuery<DataFieldDto>(sql, new { TableName = dbDto.TableName });
            }

            return res.Select(m => new DataFieldDto
            {
                IsSearch = m.IsSearch,
                IsShowList = m.IsShowList,
                IsShowForm = m.IsShowForm,
                TbName = m.TbName,
                TbDesc = m.TbDesc,
                FieldIndex = m.FieldIndex,
                FieldName = m.FieldName,
                FiledType = ConvertDataType(m.FiledType, m.IsNullable),
                FiledLength = m.FiledLength,
                IsIdentity = m.IsIdentity,
                IsKey = m.IsKey,
                IsNullable = m.IsNullable,
                DefaultVal = m.DefaultVal,
                FieldDesc = m.FieldDesc,
                ByteLength = m.ByteLength,
                DecimalLength = m.DecimalLength
            }).ToList();
        }

        #region 类型转换
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="oldType">数据库表类型</param>
        /// <param name="isNull">是否为空</param>
        /// <returns></returns>
        private string ConvertDataType(string oldType, string isNull)
        {
            string data_type = "string";
            switch (oldType)
            {
                case "int":
                    if (isNull == "")
                    {
                        data_type = "int";
                    }
                    else
                    {
                        data_type = "int?";
                    }
                    break;
                case "bigint":
                    if (isNull == "")
                    {
                        data_type = "long";
                    }
                    else
                    {
                        data_type = "long?";
                    }
                    break;
                case "decimal":
                    if (isNull == "")
                    {
                        data_type = "decimal";
                    }
                    else
                    {
                        data_type = "decimal?";
                    }
                    break;
                case "nvarchar":
                    data_type = "string";
                    break;
                case "nchar":
                    data_type = "string";
                    break;
                case "varchar":
                    data_type = "string";
                    break;
                case "text":
                    data_type = "string";
                    break;
                case "ntext":
                    data_type = "string";
                    break;
                case "varbinary":
                    data_type = "byte";
                    break;
                case "datetime":
                    if (isNull == "")
                    {
                        data_type = "DateTime";
                    }
                    else
                    {
                        data_type = "DateTime?";
                    }
                    break;
                case "bit":
                    if (isNull == "")
                    {
                        data_type = "bool";
                    }
                    else
                    {
                        data_type = "bool?";
                    }
                    break;
                default:
                    data_type = oldType;
                    break;
            }
            return data_type;
        }
        #endregion
    }
}
