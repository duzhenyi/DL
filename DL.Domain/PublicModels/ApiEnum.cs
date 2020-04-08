namespace DL.Domain.PublicModels
{
    public enum ApiEnum
    {
        /// <summary>
        /// 请求(或处理)成功
        /// </summary>
        Status = 200, //请求(或处理)成功

        /// <summary>
        /// 内部请求出错
        /// </summary>
        Error = 500, //内部请求出错

        /// <summary>
        /// 未授权标识
        /// </summary>
        Unauthorized = 401,//未授权标识

        /// <summary>
        /// 请求参数不完整或不正确
        /// </summary>
        ParameterError = 400,//请求参数不完整或不正确

        /// <summary>
        /// 请求TOKEN失效
        /// </summary>
        TokenInvalid = 403,//请求TOKEN失效

        /// <summary>
        /// HTTP请求类型不合法
        /// </summary>
        HttpMehtodError = 405,//HTTP请求类型不合法

        /// <summary>
        /// HTTP请求不合法,请求参数可能被篡改
        /// </summary>
        HttpRequestError = 406,//HTTP请求不合法

        /// <summary>
        /// 该URL已经失效
        /// </summary>
        URLExpireError = 407,//HTTP请求不合法
    }
}
