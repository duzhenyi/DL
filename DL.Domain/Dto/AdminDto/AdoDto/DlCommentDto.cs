using System;

namespace DL.Domain.Dto.AdminDto.AdoDto
{
    /// <summary>
    /// 评论显示
    /// </summary>
    public class AdoCommentDto
    {
        /// <summary>
        /// 评论内容
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateTime { get; set; }
          
        /// <summary>
        /// Desc:主键ID
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string ID { get; set; }

        /// <summary>
        /// Desc:消息ID
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string MessageID { get; set; }

        /// <summary>
        /// Desc:评论人ID
        /// Default:0
        /// Nullable:True
        /// </summary>           
        public string UserID { get; set; }
  
        /// <summary>
        /// Desc:是否审核通过 0正在审核 1 审核通过 2 审核失败
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int Audit { get; set; }

        /// <summary>
        /// Desc:审核管理员
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string AuditAdminId { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime AuditTime { get; set; }

        /// <summary>
        /// 添加的用户昵称
        /// </summary>
        public string AddUserName { get; set; }

        /// <summary>
        /// 添加的用户头像
        /// </summary>
        public string HeadPic { get; set; }
        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 审核人员
        /// </summary>
        public string AuditAdminName { get; set; }
    }
}
