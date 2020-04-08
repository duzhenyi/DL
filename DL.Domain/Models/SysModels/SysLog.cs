using SqlSugar;
using System;

namespace DL.Domain.Models.SysModels
{
    ///<summary>
    /// 系统操作表
    ///</summary>
    [SugarTable("Sys_Log")]
	public partial class SysLog:BaseModel
	{

		/// <summary>
		/// 日志等级
		/// </summary>
		public int Layer { get; set; }

		/// <summary>
		/// 消息内容
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// 请求Url
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// 异常信息
		/// </summary>
		public string Exception { get; set; }

		/// <summary>
		/// IP地址
		/// </summary>
		public string IP { get; set; }
         
		/// <summary>
		/// 浏览器信息
		/// </summary>
		public string Browser { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string RelName { get; set; }
    }
}
