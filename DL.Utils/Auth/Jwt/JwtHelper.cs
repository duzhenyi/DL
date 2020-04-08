using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace DL.Utils.Auth.Jwt
{

    /// <summary>
    /// Jwt授权认证
    /// </summary>
    public class JwtHelper
    {
        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public static string IssueToken(TokenModel tokenModel)
        {
            var dateTime = DateTime.UtcNow;

            Console.WriteLine("------------------jwtIssue:dateTime-------------------");
            Console.WriteLine(dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            Console.WriteLine("-------------------------------------");

            var claims = new List<Claim>()
            {
                new Claim("ProjectName", tokenModel.ProjectName),
                new Claim("UserName", tokenModel.UserName),
                new Claim("UserAccount", tokenModel.UserAccount),
                new Claim("Role",tokenModel.Role),
                new Claim(JwtRegisteredClaimNames.Jti,tokenModel.UserID.ToString()),
                new Claim(JwtRegisteredClaimNames.Iss,GlobalKey.Issuer),
                new Claim(JwtRegisteredClaimNames.Aud,GlobalKey.Audience)
            };


            //一个用户多个角色
            claims.AddRange(tokenModel.Role.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));

            //秘钥 (SymmetricSecurityKey 对安全性的要求，密钥的长度太短会报出异常)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GlobalKey.JWTSecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtHandler = new JwtSecurityTokenHandler();
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

            var expires = dateTime.AddMinutes(GlobalKey.Expires);
            Console.WriteLine("------------------jwtIssue-expires-------------------");
            Console.WriteLine(expires.ToString("yyyy-MM-dd HH:mm:ss"));
            Console.WriteLine("-------------------------------------");

            return jwtHandler.WriteToken(new JwtSecurityToken(
                    issuer: GlobalKey.Issuer,//签发者
                    signingCredentials: creds,//签名
                    claims: claims,//签名数据
                    expires: expires,//添加过期时间
                    notBefore: DateTime.Now//token不能早于这个时间使用
                ));
        }

        /// <summary>
        /// 解析token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static TokenModel SerializeToken(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(token);
            object role = new object();
            object userName = new object();
            object userAccount = new object();
            try
            {
                jwtToken.Payload.TryGetValue(ClaimTypes.Role, out role);
                jwtToken.Payload.TryGetValue("UserName", out userName);
                jwtToken.Payload.TryGetValue("UserAccount", out userAccount);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return new TokenModel
            {
                UserID = jwtToken.Id,
                Role = role?.ToString(),
                UserName = userName?.ToString(),
                UserAccount = userAccount?.ToString()
            };
        }

        /// <summary>
        /// 验证token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ValidateToken(string token, out DateTime dateTime)
        {
            dateTime = DateTime.Now;
            var principal = GetPrincipal(token, out dateTime);

            if (principal == null)
                return default(string);

            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }
            return identity.FindFirst("UserAccount").Value;
        }


        /// <summary>
        /// 从Token中得到ClaimsPrincipal对象
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static ClaimsPrincipal GetPrincipal(string token, out DateTime dateTime)
        {
            dateTime = DateTime.Now;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);

                if (jwtToken == null)
                    return null;

                var parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,//验证创建该令牌的发布者
                    ValidateLifetime = true,//检查令牌是否未过期，以及发行者的签名密钥是否有效
                    ValidateAudience = false,//确保令牌的接收者有权接收它
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GlobalKey.JWTSecretKey)),
                    ValidIssuer = GlobalKey.Issuer //验证创建该令牌的发布者
                };
                //验证token 
                var principal = tokenHandler.ValidateToken(token, parameters, out var securityToken);
                //若开始时间大于当前时间 或结束时间小于当前时间 则返回空
                if (securityToken.ValidFrom.ToLocalTime() > DateTime.Now || securityToken.ValidTo.ToLocalTime() < DateTime.Now)
                {
                    dateTime = DateTime.Now;
                    return null;
                }
                dateTime = securityToken.ValidTo.ToLocalTime();//返回Token结束时间
                return principal;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
