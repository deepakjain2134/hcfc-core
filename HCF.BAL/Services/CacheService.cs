using HCF.BAL.Interfaces.Services;
using HCF.Utility.Enum;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HCF.BAL
{
    public partial class CacheService : ICacheService
    {
        private const int CacheSeconds = 1200; // 10 Seconds
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void ClearStartsWith(string keyStartsWith)
        {
            throw new NotImplementedException();
        }

        public void ClearStartsWith(List<string> keysStartsWith)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key, string orgkey = null)
        {
            key = !string.IsNullOrEmpty(orgkey) ? $"{key}_{orgkey}" : key;
            var objectToReturn = _cache.Get(key);
            if (objectToReturn != null)
            {
                if (objectToReturn is T)
                {
                    return (T)objectToReturn;
                }
                try
                {
                    return (T)Convert.ChangeType(objectToReturn, typeof(T));
                }
                catch (InvalidCastException)
                {
                    return default(T);
                }
            }
            return default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetFromCache<T>(string key) where T : class
        {
            var cachedResponse = _cache.Get(key);
            return cachedResponse as T;
        }

        public void Invalidate(string key)
        {
            throw new NotImplementedException();
        }

        public bool IsSet(string key)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, object data, CacheTimes minutesToCache, string orgkey = null)
        {
            key = !string.IsNullOrEmpty(orgkey) ? $"{key}_{orgkey}" : key;
            SetCache(key, data, DateTimeOffset.Now.AddSeconds(CacheSeconds));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetCache<T>(string key, T value) where T : class
        {
            SetCache(key, value, DateTimeOffset.Now.AddSeconds(CacheSeconds));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="duration"></param>
        public void SetCache<T>(string key, T value, DateTimeOffset duration) where T : class
        {
            _cache.Set(key, value, duration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void ClearCache(string key)
        {
            _cache.Remove(key);
        }
    }
}
