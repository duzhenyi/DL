using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Utils.Auth
{

    /// <summary>
    /// 全局配置
    /// </summary>
    public partial class GlobalKey
    {
        /// <summary>
        /// 签发者 颁发机构 必填
        /// </summary>
        public static string Issuer { get; set; } = AppConfig.Appsettings.app("JwtAuth:Issuer");
        /// <summary>
        /// 颁发给谁 必填
        /// </summary>
        public static string Audience { get; set; } = AppConfig.Appsettings.app("JwtAuth:Audience");

        /// <summary>
        /// 秘钥 必填
        /// </summary>
        public static string JWTSecretKey { get; set; } = AppConfig.Appsettings.app("JwtAuth:SecurityKey");

        /// <summary>
        /// 过期时间（分钟）
        /// </summary>
        public static double Expires { get; set; } = double.Parse(AppConfig.Appsettings.app("JwtAuth:Expires"));
    }
}
