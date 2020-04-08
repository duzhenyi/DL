using System;
using Microsoft.Extensions.Caching.Memory;

namespace DL.Utils.Cache.MemoryCache
{
    public class MemoryCacheHelper  
    {

        protected static IMemoryCache _cache;

        public  MemoryCacheHelper(IMemoryCache cache)
        {
            _cache = cache;
        }

        public static MemoryCacheHelper Default { get; private set; }
        static MemoryCacheHelper()
        {
            Default = new MemoryCacheHelper(new Microsoft.Extensions.Caching.Memory.MemoryCache(new MemoryCacheOptions()));
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public static void Del<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            _cache.Remove(key);
        }

        /// <summary>
        /// 缓存是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Exists<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            object v = null;
            return _cache.TryGetValue<object>(key, out v);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            T v = default(T);
            _cache.TryGetValue<T>(key, out v);
            return v;
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set<T>(string key, T value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            object v = null;
            if (_cache.TryGetValue(key, out v))
                _cache.Remove(key);
            _cache.Set<object>(key, value);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireTime">过期时间</param>
        public static void Set<T>(string key, T value, DateTime expireTime)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            object v = null;
            if (_cache.TryGetValue(key, out v))
                _cache.Remove(key);
            _cache.Set<object>(key, value, expireTime);
        }


        /// <summary>
        /// 设置缓存,相对过期时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan"></param>
        /// MemoryCacheService.Default.SetCache("test", "MemoryCache works!",TimeSpan.FromSeconds(30));
        public static void Set<T>(string key, T value, TimeSpan timeSpan)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            object v = null;
            if (_cache.TryGetValue(key, out v))
                _cache.Remove(key);

            _cache.Set(key, value, new MemoryCacheEntryOptions()
            {
                SlidingExpiration = timeSpan
            });
        }
    }
}
