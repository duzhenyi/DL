using System;

namespace DL.Utils.Auth
{

    /// <summary>
    /// 令牌生成，令牌验证，令牌解析
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// 生成令牌
        /// </summary>
        /// <returns></returns>
        string IssueToken(TokenModel tokenModel);

        /// <summary>
        /// 解析令牌
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        TokenModel SerializeToken(string token);

        /// <summary>
        /// 验证令牌
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        string ValidateToken(string token, out DateTime dateTime);
    }
}
