using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace DL.Utils.Helper
{
    public class ImageHelper
    {

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="file"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool SaveImage(IFormFile file, string path)
        {
            if (file.Length > 0)
            {
                //var name = Path.GetFileName(file.FileName);
                //if (name != null)
                //{
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);

                    // Add watermark
                    var watermarkedStream = new MemoryStream();

                    using (var img = Image.FromStream(stream))
                    {
                        if (ImageHelper.IsIndexedPixelFormat(img.PixelFormat))
                        {//如果原图片是索引像素格式之列的，则需要转换

                            Bitmap bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb);
                            using (Graphics g = Graphics.FromImage(bmp))
                            {
                                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                                g.DrawImage(img, 0, 0);
                            }
                            DrawImgString(watermarkedStream, null, bmp);
                        }
                        else
                        {
                            DrawImgString(watermarkedStream, img, null);
                        }

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        img.Save(path);
                    }
                    return true;
                }
                //}
            }

            return false;

        }

        /// <summary>
        /// 添加水印
        /// </summary>
        public static void DrawImgString(MemoryStream watermarkedStream, Image imagae, Bitmap bitmap)
        {
            Image img;
            if (imagae != null)
            {
                img = imagae;
            }
            else
            {
                img = bitmap;
            }

            //水印操作
            using (var graphic = Graphics.FromImage(img))
            {
                var font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold, GraphicsUnit.Pixel);
                var color = Color.FromArgb(128, 255, 255, 255);
                var brush = new SolidBrush(color);
                var point = new Point(img.Width - 150, img.Height - 30);

                graphic.DrawString("www.3sha.com", font, brush, point);
                img.Save(watermarkedStream, ImageFormat.Png);
            }
        }

        /// <summary>
        /// 判断图片是否索引像素格式,是否是引发异常的像素格式
        /// </summary>
        /// <param name="imagePixelFormat">图片的像素格式</param>
        /// <returns></returns>
        public static bool IsIndexedPixelFormat(PixelFormat imagePixelFormat)
        {
            PixelFormat[] pixelFormatArray = {
                                            PixelFormat.Format1bppIndexed,
                                            PixelFormat.Format4bppIndexed,
                                            PixelFormat.Format8bppIndexed,
                                            PixelFormat.Undefined,
                                            PixelFormat.DontCare,
                                            PixelFormat.Format16bppArgb1555,
                                            PixelFormat.Format16bppGrayScale
                                        };
            foreach (PixelFormat pf in pixelFormatArray)
            {
                if (imagePixelFormat == pf)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
