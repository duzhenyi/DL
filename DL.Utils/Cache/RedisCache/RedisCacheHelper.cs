using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Utils.Cache.RedisCache
{
    public class RedisCacheHelper 
    {
        public static void Del<T>(string key)
        {
            throw new NotImplementedException();
        }

        public static bool Exists<T>(string key)
        {
            throw new NotImplementedException();
        }

        public static T Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public static void Set<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public static void Set<T>(string key, T value, DateTime expireTime)
        {
            throw new NotImplementedException();
        }

        public static void Set<T>(string key, T value, TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }
    }
}
