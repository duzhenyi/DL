namespace DL.Utils.Auth
{
  
    /// <summary>
    /// token
    /// </summary>
    public class TokenModel
    {
        /// <summary>
        /// 所属项目 必填
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserAccount { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 所属身份
        /// </summary>
        public string Role { get; set; }
    }
}
