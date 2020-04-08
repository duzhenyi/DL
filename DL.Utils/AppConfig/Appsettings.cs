using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DL.Utils.AppConfig
{
    /// <summary>
    /// 读取配置文件
    /// </summary>
    public class Appsettings
    {
        public static IConfiguration Configuration { get; set; }

        static Appsettings()
        {
            string Path = "appsettings.json";
            {
                //如果你把配置文件 是 根据环境变量来分开了，可以这样写
                //Path = $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json";
            }

            Configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .Add(new JsonConfigurationSource { Path = Path, Optional = false, ReloadOnChange = true })//这样的话，可以直接读目录里的json文件，而不是 bin 文件夹下的，所以不用修改复制属性
               .Build();
        }

        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static string app(params string[] sections)
        {
            try
            {
                var val = string.Empty;
                for (int i = 0; i < sections.Length; i++)
                {
                    val += sections[i] + ":";
                }

                return Configuration[val.TrimEnd(':')];
            }
            catch (Exception)
            {
                return "";
            }

        }

        public static IConfigurationSection GetSection(string key)
        {
            return Configuration?.GetSection(key);
        }
        public static string GetConnectionString(string name)
        {
            return Configuration.GetConnectionString(name);
        }
        public static IEnumerable<IConfigurationSection> GetChildren()
        {
            return Configuration?.GetChildren();
        }
    }
}
