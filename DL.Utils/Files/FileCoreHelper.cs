using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DL.Utils.Files
{
    public class FileCoreHelper
    {
        private static IWebHostEnvironment _hostingEnvironment = new HttpContextAccessor().HttpContext.RequestServices.GetService(typeof(IWebHostEnvironment)) as IWebHostEnvironment;

        /// <summary>
        /// 目录分隔符
        /// windows "\" OSX and Linux  "/"
        /// </summary>
        private static string DirectorySeparatorChar = Path.DirectorySeparatorChar.ToString();
        /// <summary>
        /// 包含应用程序的目录的绝对路径
        /// </summary>
        private static string _ContentRootPath = _hostingEnvironment.ContentRootPath;

        #region 检测指定路径是否存在

        /// <summary>
        /// 检测指定路径是否存在
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static bool IsExist(string path)
        {
            return IsDirectory(MapPath(path)) ? Directory.Exists(MapPath(path)) : File.Exists(MapPath(path));
        }
        /// <summary>
        /// 检测指定路径是否存在（异步方式）
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static async Task<bool> IsExistAsync(string path)
        {
            return await Task.Run(() => IsDirectory(MapPath(path)) ? Directory.Exists(MapPath(path)) : File.Exists(MapPath(path)));
        }

        #endregion

        #region 检测目录是否为空

        /// <summary>
        /// 检测目录是否为空
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static bool IsEmptyDirectory(string path)
        {
            return Directory.GetFiles(MapPath(path)).Length <= 0 && Directory.GetDirectories(MapPath(path)).Length <= 0;
        }
        /// <summary>
        /// 检测目录是否为空
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static async Task<bool> IsEmptyDirectoryAsync(string path)
        {
            return await Task.Run(() => Directory.GetFiles(MapPath(path)).Length <= 0 && Directory.GetDirectories(MapPath(path)).Length <= 0);
        }

        #endregion

        #region 创建目录

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="path">路径</param>
        public static void CreateFiles(string path)
        {
            try
            {
                if (IsDirectory(MapPath(path)))
                    Directory.CreateDirectory(MapPath(path));
                else
                    File.Create(MapPath(path)).Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 删除文件或目录

        /// <summary>
        /// 删除目录或文件
        /// </summary>
        /// <param name="path">路径</param>
        public static void DeleteFiles(string path)
        {
            try
            {
                if (IsExist(path))
                {
                    if (IsDirectory(MapPath(path)))
                        Directory.Delete(MapPath(path));
                    else
                        File.Delete(MapPath(path));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 清空目录下所有文件及子目录，依然保留该目录
        /// </summary>
        /// <param name="path"></param>
        public static void ClearDirectory(string path)
        {
            if (IsExist(path))
            {
                //目录下所有文件
                string[] files = Directory.GetFiles(MapPath(path));
                foreach (var file in files)
                {
                    DeleteFiles(file);
                }
                //目录下所有子目录
                string[] directorys = Directory.GetDirectories(MapPath(path));
                foreach (var dir in directorys)
                {
                    DeleteFiles(dir);
                }
            }
        }

        #endregion

        #region 判断文件是否为隐藏文件（系统独占文件）

        /// <summary>
        /// 检测文件或文件夹是否为隐藏文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static bool IsHiddenFile(string path)
        {
            return IsDirectory(MapPath(path)) ? InspectHiddenFile(new DirectoryInfo(MapPath(path))) : InspectHiddenFile(new FileInfo(MapPath(path)));
        }
        /// <summary>
        /// 检测文件或文件夹是否为隐藏文件（异步方式）
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static async Task<bool> IsHiddenFileAsync(string path)
        {
            return await Task.Run(() => IsDirectory(MapPath(path)) ? InspectHiddenFile(new DirectoryInfo(MapPath(path))) : InspectHiddenFile(new FileInfo(MapPath(path))));
        }
        /// <summary>
        /// 私有方法 文件是否为隐藏文件（系统独占文件）
        /// </summary>
        /// <param name="fileSystemInfo"></param>
        /// <returns></returns>
        private static bool InspectHiddenFile(FileSystemInfo fileSystemInfo)
        {
            if (fileSystemInfo.Name.StartsWith("."))
            {
                return true;
            }
            else if (fileSystemInfo.Exists &&
                ((fileSystemInfo.Attributes & FileAttributes.Hidden) != 0 ||
                 (fileSystemInfo.Attributes & FileAttributes.System) != 0))
            {
                return true;
            }

            return false;
        }

        #endregion

        #region 文件操作

        #region 检测文件名是否已经存在在该路径下

        /// <summary>
        /// 检测文件名是否已经存在在该路径下
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="fileName"></param>
        /// <param name="staticFiles"></param>
        /// <returns></returns>
        public static bool ExistFileInfo(string path, string fileName)
        {
            var foldersPath = MapPath(path);
            var Files = new List<FilesInfo>();
            foreach (var fsi in new DirectoryInfo(foldersPath).GetFiles())
            {
                if (fsi.FullName == fileName)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region 复制文件

        /// <summary>
        /// 复制文件内容到目标文件夹
        /// </summary>
        /// <param name="sourcePath">源文件</param>
        /// <param name="targetPath">目标文件夹</param>
        /// <param name="isOverWrite">是否可以覆盖</param>
        public static void Copy(string sourcePath, string targetPath, bool isOverWrite = true)
        {
            File.Copy(MapPath(sourcePath), MapPath(targetPath) + GetFileName(sourcePath), isOverWrite);
        }
        /// <summary>
        /// 复制文件内容到目标文件夹
        /// </summary>
        /// <param name="sourcePath">源文件</param>
        /// <param name="targetPath">目标文件夹</param>
        /// <param name="newName">新文件名称</param>
        /// <param name="isOverWrite">是否可以覆盖</param>
        public static void Copy(string sourcePath, string targetPath, string newName, bool isOverWrite = true)
        {
            File.Copy(MapPath(sourcePath), MapPath(targetPath) + newName, isOverWrite);
        }

        #endregion

        #region 移动文件

        /// <summary>
        /// 移动文件到目标目录
        /// </summary>
        /// <param name="sourcePath">源文件</param>
        /// <param name="targetPath">目标目录</param>
        public static void Move(string sourcePath, string targetPath)
        {
            string sourceFileName = GetFileName(sourcePath);
            //如果目标目录不存在则创建
            if (IsExist(targetPath))
            {
                CreateFiles(targetPath);
            }
            else
            {
                //如果目标目录存在同名文件则删除
                if (IsExist(Path.Combine(MapPath(targetPath), sourceFileName)))
                {
                    DeleteFiles(Path.Combine(MapPath(targetPath), sourceFileName));
                }
            }

            File.Move(MapPath(sourcePath), Path.Combine(MapPath(targetPath), sourceFileName));


        }

        #endregion

        /// <summary>
        /// 获取文件名和扩展名
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string GetFileName(string path)
        {
            return Path.GetFileName(MapPath(path));
        }

        /// <summary>
        /// 获取文件名不带扩展名
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string GetFileNameWithOutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(MapPath(path));
        }

        /// <summary>
        /// 获取文件扩展名
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string GetFileExtension(string path)
        {
            return Path.GetExtension(MapPath(path));
        }

        #endregion

        #region 获取文件绝对路径

        /// <summary>
        /// 获取文件绝对路径
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string MapPath(string path)
        {
            return Path.Combine(_ContentRootPath, path.TrimStart('~', '/').Replace("/", DirectorySeparatorChar));
        }
        /// <summary>
        /// 获取文件绝对路径（异步方式）
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static async Task<string> MapPathAsync(string path)
        {
            return await Task.Run(() => Path.Combine(_ContentRootPath, path.TrimStart('~', '/').Replace("/", DirectorySeparatorChar)));
        }


        /// <summary>
        /// 是否为目录或文件夹
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static bool IsDirectory(string path)
        {
            if (path.EndsWith(DirectorySeparatorChar))
                return true;
            else
                return false;
        }

        #endregion

        #region 物理路径转虚拟路径
        public static string PhysicalToVirtual(string physicalPath)
        {
            return physicalPath.Replace(_ContentRootPath, "").Replace(DirectorySeparatorChar, "/");
        }
        #endregion

        #region 文件格式
        /// <summary>
        /// 是否可添加水印
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        /// <returns></returns>
        public static bool IsCanWater(string _fileExt)
        {
            var images = new List<string> { "jpg", "jpeg" };
            if (images.Contains(_fileExt.ToLower())) return true;
            return false;
        }
        /// <summary>
        /// 是否为图片
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        /// <returns></returns>
        public static bool IsImage(string _fileExt)
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
        public static bool IsVideos(string _fileExt)
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
        public static bool IsMusics(string _fileExt)
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
        public static bool IsDocument(string _fileExt)
        {
            var documents = new List<string> { "doc", "docx", "xls", "xlsx", "ppt", "pptx", "txt", "pdf" };
            if (documents.Contains(_fileExt.ToLower())) return true;
            return false;
        }
        #endregion

        #region 文件图标
        public static string FindFileIcon(string fileExt)
        {
            if (IsImage(fileExt))
                return "fa fa-image";
            if (IsVideos(fileExt))
                return "fa fa-film";
            if (IsMusics(fileExt))
                return "fa fa-music";
            if (IsDocument(fileExt))
                switch (fileExt.ToLower())
                {
                    case ".xls":
                    case ".xlsx":
                        return "fa fa-file-excel-o";
                    case ".ppt":
                    case ".pptx":
                        return "fa fa-file-powerpoint-o";
                    case ".pdf":
                        return "fa fa-file-pdf-o";
                    case ".txt":
                        return "fa fa-file-text-o";
                    default:
                        return "fa fa-file-word-o";
                }
            if (fileExt.ToLower() == "zip" || fileExt.ToLower() == "rar")
                return "fa fa-file-zip-o";
            else
                return "fa fa-file";
        }
        #endregion

        #region 文件大小转换
        /// <summary>
        ///  文件大小转为B、KB、MB、GB...
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string FileSizeTransf(long size)
        {
            String[] units = new String[] { "B", "KB", "MB", "GB", "TB", "PB" };
            long mod = 1024;
            int i = 0;
            while (size > mod)
            {
                size /= mod;
                i++;
            }
            return size + units[i];

        }
        #endregion

        #region 获取目录下所有文件
        public static List<FilesInfo> FindFiles(string path, string staticFiles = "/wwwroot")
        {
            string[] folders = Directory.GetDirectories(MapPath(path), "*", SearchOption.AllDirectories);
            var Files = new List<FilesInfo>();

            foreach (var folder in folders)
            {
                foreach (var fsi in new DirectoryInfo(folder).GetFiles())
                {
                    Files.Add(new FilesInfo()
                    {
                        Name = fsi.Name,
                        FullName = fsi.FullName,
                        FileExt = fsi.Extension,
                        FileOriginalSize = fsi.Length,
                        FileSize = FileSizeTransf(fsi.Length),
                        FileIcon = FindFileIcon(fsi.Extension.Remove(0, 1)),
                        FileName = PhysicalToVirtual(fsi.FullName).Replace(staticFiles, ""),
                        FileStyle = IsImage(fsi.Extension.Remove(0, 1)) ? "images" :
                                    IsDocument(fsi.Extension.Remove(0, 1)) ? "documents" :
                                    IsVideos(fsi.Extension.Remove(0, 1)) ? "videos" :
                                    IsMusics(fsi.Extension.Remove(0, 1)) ? "musics" : "others",
                        CreateDate = fsi.CreationTime,
                        LastWriteDate = fsi.LastWriteTime,
                        LastAccessDate = fsi.LastAccessTime
                    });
                }
            }
            return Files;
        }

        /// <summary>
        /// 获得指定文件夹下面的所有文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="staticFiles"></param>
        /// <returns></returns>
        public static List<FilesInfo> ResolveFileInfo(string path, string staticFiles = "/wwwroot")
        {
            var foldersPath = MapPath(path);
            var Files = new List<FilesInfo>();
            foreach (var fsi in new DirectoryInfo(foldersPath).GetFiles())
            {
                Files.Add(new FilesInfo()
                {
                    Name = fsi.Name,
                    FullName = fsi.FullName,
                    FileExt = fsi.Extension,
                    FileOriginalSize = fsi.Length,
                    FileSize = FileSizeTransf(fsi.Length),
                    FileIcon = FindFileIcon(fsi.Extension.Remove(0, 1)),
                    FileName = PhysicalToVirtual(fsi.FullName).Replace(staticFiles, ""),
                    FileStyle = IsImage(fsi.Extension.Remove(0, 1)) ? "images" :
                                IsDocument(fsi.Extension.Remove(0, 1)) ? "documents" :
                                IsVideos(fsi.Extension.Remove(0, 1)) ? "videos" :
                                IsMusics(fsi.Extension.Remove(0, 1)) ? "musics" : "others",
                    CreateDate = fsi.CreationTime,
                    LastWriteDate = fsi.LastWriteTime,
                    LastAccessDate = fsi.LastAccessTime
                });
            }
            return Files;
        }
        #endregion

        #region 添加水印

        /// <summary>
        /// 添加水印
        /// </summary>
        public static void AddwWterMark(Stream stream)
        {

            var watermarkedStream = new MemoryStream();
            using (var img = System.Drawing.Image.FromStream(stream))
            {
                using (var graphic = Graphics.FromImage(img))
                {
                    var font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold, GraphicsUnit.Pixel);
                    var color = Color.FromArgb(128, 255, 255, 255);
                    var brush = new SolidBrush(color);
                    var point = new Point(img.Width - 120, img.Height - 30);

                    graphic.DrawString("www.3sha.cn", font, brush, point);
                    img.Save(watermarkedStream, ImageFormat.Png);
                }

                //img.Save(hostingEnv.WebRootPath + "/" + name);
            }
        }

        #endregion

        #region 发送邮件

        /// <summary>
        /// Install-Package MailKit
        /// 它支持跨平台，并且支持 IMAP, POP3, SMTP 等协议
        /// 其中通过mailkit发送的时候  发送方必须要打开自己的POP3/IMAP/SMTP/Exchange/CardDAV/CalDAV服务 一般是在自己的个人中心  
        /// </summary>
        /// <param name="fromName"></param>
        /// <param name="fromMail"></param>
        /// <param name="toName"></param>
        /// <param name="toMail"></param>
        /// <param name="title"></param>
        /// <param name="txtStr"></param>
        /// <param name="htmlStr"></param>
        /// <param name="path"></param>
        /// <param name="mailFromAccount"></param>
        /// <param name="mailPassword"></param>
        public static void SendMail(string fromName, string fromMail,
            string toName, string toMail, string title, string txtStr, 
            string htmlStr, string path,string mailFromAccount, string mailPassword)
        {
            var message = new MimeKit.MimeMessage();
            message.From.Add(new MimeKit.MailboxAddress(fromName, fromMail));
            message.To.Add(new MimeKit.MailboxAddress(toName, toMail));
            message.Subject = title;
            var plain = new MimeKit.TextPart("plain")
            {
                Text = txtStr
            };
            var html = new MimeKit.TextPart("html")
            {
                Text = htmlStr
            };
            // 图片文件的路径

            var fs = File.OpenRead(path);
            var attachment = new MimeKit.MimePart("image", "jpeg")
            {
                Content = new MimeKit.MimeContent(fs, MimeKit.ContentEncoding.Default),
                ContentDisposition = new MimeKit.ContentDisposition(MimeKit.ContentDisposition.Attachment),
                ContentTransferEncoding = MimeKit.ContentEncoding.Base64,
                FileName = Path.GetFileName(path)
            };
            var alternative = new MimeKit.Multipart("alternative");
            alternative.Add(plain);
            alternative.Add(html);
            // now create the multipart/mixed container to hold the message text and the
            // image attachment
            var multipart = new MimeKit.Multipart("mixed");
            multipart.Add(alternative);
            multipart.Add(attachment);
            message.Body = multipart;
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("smtp.qq.com", 465, true);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                client.Authenticate(mailFromAccount, mailPassword);
                client.Send(message);
                client.Disconnect(true);
            }
            fs.Dispose();
        }

        /// <summary>
        /// FluentEmail - SMTP
        /// </summary>
        /// <param name="path">图片文件路径</param>
        /// <param name="address">发送人</param>
        /// <param name="mailTo">发送给谁</param>
        /// <param name="mailFromAccount">发送人账号</param>
        /// <param name="mailPwd">发送人密码</param>
        public static void TestSmtpClient(string path, string address, string mailTo, string mailFromAccount, string mailPwd)
        {
            MailMessage mymail = new MailMessage();
            mymail.From = new System.Net.Mail.MailAddress(address);
            mymail.To.Add(mailTo);
            mymail.Subject = string.Format("C#自动发送邮件测试 From geffzhang TO {0}", mailTo);
            mymail.Body = @"<p>Hey geffzhang<br><p>不好意思，我在测试程序，刚才把QQ号写错了，Sorry！<br><p>-- Geffzhang<br>";
            mymail.IsBodyHtml = true;
            mymail.Attachments.Add(new Attachment(path));

            System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient();
            smtpclient.Port = 587;
            smtpclient.UseDefaultCredentials = false;
            smtpclient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpclient.Host = "smtp.live.com";
            smtpclient.EnableSsl = true;
            smtpclient.Credentials = new System.Net.NetworkCredential(mailFromAccount, mailPwd);
            try
            {
                smtpclient.Send(mymail);
                Console.WriteLine("发送成功");


            }
            catch (Exception ex)
            {
                Console.WriteLine("发送邮件失败.请检查是否为qq邮箱，并且没有被防护软件拦截" + ex);

            }
        }

        #endregion
    }
}
