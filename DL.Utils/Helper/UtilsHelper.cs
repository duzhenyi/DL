using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace DL.Utils.Helper
{
    public class UtilsHelper
    {
        #region  HtmlEncode(对html字符串进行编码)

        /// <summary>
        /// 对html字符串进行编码
        /// </summary>
        /// <param name="html">html字符串</param>
        public static string HtmlEncode(string html)
        {
            return HttpUtility.HtmlEncode(html);
        }
        /// <summary>
        /// 对html字符串进行解码
        /// </summary>
        /// <param name="html">html字符串</param>
        public static string HtmlDecode(string html)
        {
            return HttpUtility.HtmlDecode(html);
        }

        #endregion

        #region  UrlEncode(对Url进行编码)

        /// <summary>
        /// 对Url进行编码
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="isUpper">编码字符是否转成大写,范例,"http://"转成"http%3A%2F%2F"</param>
        public static string UrlEncode(string url, bool isUpper = false)
        {
            return UrlEncode(url, Encoding.UTF8, isUpper);
        }

        /// <summary>
        /// 对Url进行编码
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="isUpper">编码字符是否转成大写,范例,"http://"转成"http%3A%2F%2F"</param>
        public static string UrlEncode(string url, Encoding encoding, bool isUpper = false)
        {
            var result = HttpUtility.UrlEncode(url, encoding);
            if (!isUpper)
                return result;
            return GetUpperEncode(result);
        }

        /// <summary>
        /// 获取大写编码字符串
        /// </summary>
        /// <param name="encode">字串</param>
        /// <returns></returns>
        private static string GetUpperEncode(string encode)
        {
            var result = new StringBuilder();
            int index = int.MinValue;
            for (int i = 0; i < encode.Length; i++)
            {
                string character = encode[i].ToString();
                if (character == "%")
                    index = i;
                if (i - index == 1 || i - index == 2)
                    character = character.ToUpper();
                result.Append(character);
            }
            return result.ToString();
        }

        #endregion

        #region  UrlDecode(对Url进行解码)

        /// <summary>
        /// 对Url进行解码,对于javascript的encodeURIComponent函数编码参数,应使用utf 8字符编码来解码
        /// </summary>
        /// <param name="url">url</param>
        public static string UrlDecode(string url)
        {
            return HttpUtility.UrlDecode(url);
        }

        /// <summary>
        /// 对Url进行解码,对于javascript的encodeURIComponent函数编码参数,应使用utf 8字符编码来解码
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="encoding">字符编码,对于javascript的encodeURIComponent函数编码参数,应使用utf 8字符编码来解码</param>
        public static string UrlDecode(string url, Encoding encoding)
        {
            return HttpUtility.UrlDecode(url, encoding);
        }

        #endregion

        #region  去除HTML标记
        /// <summary>
        /// 去除HTML标记
        /// </summary>
        /// <param name="NoHTML">包括HTML的源码 </param>
        /// <returns>已经去除后的文字</returns>
        public static string NoHtml(string Htmlstring)
        {
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"  >", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!  .*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&hellip;", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&mdash;", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&ldquo;", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring = Regex.Replace(Htmlstring, @"&rdquo;", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpUtility.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;

        }
        #endregion

        #region  格式化文本（防止SQL注入）
        /// <summary>
        /// 格式化文本（防止SQL注入）
        /// </summary>
        /// <param name="html">html页面数据</param>
        /// <returns></returns>
        public static string Formatstr(string html)
        {
            Regex regex1 = new Regex(@"<script[\s\S]+</script *>", RegexOptions.IgnoreCase);
            Regex regex2 = new Regex(@" href *= *[\s\S]*script *:", RegexOptions.IgnoreCase);
            Regex regex3 = new Regex(@" on[\s\S]*=", RegexOptions.IgnoreCase);
            Regex regex4 = new Regex(@"<iframe[\s\S]+</iframe *>", RegexOptions.IgnoreCase);
            Regex regex5 = new Regex(@"<frameset[\s\S]+</frameset *>", RegexOptions.IgnoreCase);
            Regex regex10 = new Regex(@"select", RegexOptions.IgnoreCase);
            Regex regex11 = new Regex(@"update", RegexOptions.IgnoreCase);
            Regex regex12 = new Regex(@"delete", RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //过滤<script></script>标记
            html = regex2.Replace(html, ""); //过滤href=javascript: (<A>) 属性
            html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on...事件
            html = regex4.Replace(html, ""); //过滤iframe
            html = regex10.Replace(html, "s_elect");
            html = regex11.Replace(html, "u_pudate");
            html = regex12.Replace(html, "d_elete");
            html = html.Replace("'", "’");
            html = html.Replace("&nbsp;", " ");
            return html;
        }
        #endregion

        #region  获取mac地址
        /// <summary>
        /// 返回描述本地计算机上的网络接口的对象(网络接口也称为网络适配器)。
        /// </summary>
        /// <returns></returns>
        public static NetworkInterface[] NetCardInfo()
        {
            return NetworkInterface.GetAllNetworkInterfaces();
        }

        ///<summary>
        /// 通过NetworkInterface读取网卡Mac
        ///</summary>
        ///<returns></returns>
        public static List<string> GetMacByNetworkInterface()
        {
            List<string> macs = new List<string>();
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in interfaces)
            {
                macs.Add(ni.GetPhysicalAddress().ToString());
            }
            return macs;
        }
        #endregion

        #region 截取字符串
        public static string GetSubString(string pSrcString, int pLength, string pTailString)
        {
            return GetSubString(pSrcString, 0, pLength, pTailString);
        }
        public static string GetSubString(string pSrcString, int pStartIndex, int pLength, string pTailString)
        {
            string str = pSrcString;
            byte[] bytes = Encoding.UTF8.GetBytes(pSrcString);
            foreach (char ch in Encoding.UTF8.GetChars(bytes))
            {
                if (((ch > 'ࠀ') && (ch < '一')) || ((ch > 0xac00) && (ch < 0xd7a3)))
                {
                    if (pStartIndex >= pSrcString.Length)
                    {
                        return "";
                    }
                    return pSrcString.Substring(pStartIndex, ((pLength + pStartIndex) > pSrcString.Length) ? (pSrcString.Length - pStartIndex) : pLength);
                }
            }
            if (pLength < 0)
            {
                return str;
            }
            byte[] sourceArray = Encoding.Default.GetBytes(pSrcString);
            if (sourceArray.Length <= pStartIndex)
            {
                return str;
            }
            int length = sourceArray.Length;
            if (sourceArray.Length > (pStartIndex + pLength))
            {
                length = pLength + pStartIndex;
            }
            else
            {
                pLength = sourceArray.Length - pStartIndex;
                pTailString = "";
            }
            int num2 = pLength;
            int[] numArray = new int[pLength];
            byte[] destinationArray = null;
            int num3 = 0;
            for (int i = pStartIndex; i < length; i++)
            {
                if (sourceArray[i] > 0x7f)
                {
                    num3++;
                    if (num3 == 3)
                    {
                        num3 = 1;
                    }
                }
                else
                {
                    num3 = 0;
                }
                numArray[i] = num3;
            }
            if ((sourceArray[length - 1] > 0x7f) && (numArray[pLength - 1] == 1))
            {
                num2 = pLength + 1;
            }
            destinationArray = new byte[num2];
            Array.Copy(sourceArray, pStartIndex, destinationArray, 0, num2);
            return (Encoding.Default.GetString(destinationArray) + pTailString);
        }
        #endregion

        #region 字符串转换


        /// <summary>
        /// 将字符串转换为int类型数组
        /// </summary>
        /// <param name="str">如1,2,3,4,5</param>
        /// <returns></returns>
        public static List<int> StrToListInt(string str)
        {
            var list = new List<int>();
            if (!str.Contains(","))
            {
                list.Add(int.Parse(str));
                return list;
            }
            var slist = str.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in slist)
            {
                list.Add(int.Parse(item));
            }
            return list;
        }

        /// <summary>
        /// 将字符串转换为int类型数组
        /// </summary>
        /// <param name="str">如1,2,3,4,5</param>
        /// <returns></returns>
        public static List<string> StrToListString(string str)
        {
            var list = new List<string>();
            if (!str.Contains(","))
            {
                list.Add(str);
                return list;
            }
            var slist = str.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in slist)
            {
                list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// 将字符串转换为数组
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>字符串数组</returns>
        public static string[] GetStrArray(string str)
        {
            return str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        #endregion

        #region 分割字符串
        public static string[] SplitString(string strContent, char strSplit)
        {
            if (!string.IsNullOrEmpty(strContent))
            {
                return strContent.Split(new char[] { strSplit }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                return new string[0] { };
            }
        }
        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (!string.IsNullOrEmpty(strContent))
            {
                if (strContent.IndexOf(strSplit, StringComparison.Ordinal) < 0)
                    return new string[] { strContent };

                return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
                return new string[0] { };
        }
        #endregion

        #region 截取字符长度
        /// <summary>
        /// 截取字符长度
        /// </summary>
        /// <param name="inputString">字符</param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string CutString(string inputString, int len)
        {
            if (string.IsNullOrEmpty(inputString))
                return "";
            inputString = DropHtml(inputString);
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }
            //如果截过则加上半个省略号 
            byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
            if (mybyte.Length > len)
                tempString += "…";
            return tempString;
        }

        public static string DropHtml(string htmlstring)
        {
            if (string.IsNullOrEmpty(htmlstring)) return "";
            //删除脚本  
            htmlstring = Regex.Replace(htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML  
            htmlstring = Regex.Replace(htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);

            htmlstring = Regex.Replace(htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            htmlstring = htmlstring.Replace("<", "");
            htmlstring = htmlstring.Replace(">", "");
            htmlstring = htmlstring.Replace("\r\n", "");
            //htmlstring = HttpContext.Current.Server.HtmlEncode(htmlstring).Trim(); 
            return htmlstring;
        }
        #endregion

        #region 生成随机字母或数字

        private static readonly Random Random = new Random();

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <returns></returns>
        public static string Number(int length)
        {
            return Number(length, false);
        }

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <param name="sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        public static string Number(int length, bool sleep)
        {
            if (sleep)
                System.Threading.Thread.Sleep(2);
            string result = "";

            for (int i = 0; i < length; i++)
            {
                result += Random.Next(10).ToString();
            }
            return result;
        }


        /// <summary>
        /// 根据日期和随机码生成订单号
        /// </summary>
        /// <returns></returns>
        public static string GetOrderNumber()
        {
            string num = DateTime.Now.ToString("yyyyMMddHHmmssms"); //yyyyMMddHHmmssms
            return num + Number(2);
        }

        #endregion

        #region 显示错层方法
        /// <summary>
        /// 显示错层方法
        /// </summary>
        public static string LevelName(string name, decimal? level)
        {
            if (level == 1)
            { return name; }

            string nbsp = "";
            for (int i = 0; i < level; i++)
            {
                nbsp += "　";
            }
            name = nbsp + "|--" + name;

            Console.WriteLine(name);
            return name;
        }
        #endregion

        #region 上传配置
        /// <summary>
        ///  根据文件类型分配路径
        /// </summary>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        public static string AssigendPath(string fileExt, string path)
        {
            if (IsImage(fileExt))
                return path + "/upload/images/" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "/";
            if (IsVideos(fileExt))
                return path + "/upload/videos/" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "/";
            if (IsDocument(fileExt))
                return "/upload/files/" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "/";
            if (IsMusics(fileExt))
                return "/upload/musics/" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "/";
            return path + "/upload/others/";
        }
        #endregion

        #region 文件格式
        /// <summary>
        /// 是否为图片
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        /// <returns></returns>
        private static bool IsImage(string _fileExt)
        {
            var images = new List<string> { "bmp", "gif", "jpg", "jpeg", "png" };
            if (images.Contains(_fileExt.ToLower())) return true;
            return false;
        }
        /// <summary>
        /// 是否为视频
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        /// <returns></returns>
        private static bool IsVideos(string _fileExt)
        {
            var videos = new List<string> { "rmvb", "mkv", "ts", "wma", "avi", "rm", "mp4", "flv", "mpeg", "mov", "3gp", "mpg" };
            if (videos.Contains(_fileExt.ToLower())) return true;
            return false;
        }
        /// <summary>
        /// 是否为音频
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        /// <returns></returns>
        private static bool IsMusics(string _fileExt)
        {
            var musics = new List<string> { "mp3", "wav" };
            if (musics.Contains(_fileExt.ToLower())) return true;
            return false;
        }
        /// <summary>
        /// 是否为文档
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        /// <returns></returns>
        private static bool IsDocument(string _fileExt)
        {
            var documents = new List<string> { "doc", "docx", "xls", "xlsx", "ppt", "pptx", "txt", "pdf" };
            if (documents.Contains(_fileExt.ToLower())) return true;
            return false;
        }
        #endregion
    }
}
