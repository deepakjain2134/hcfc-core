using HCF.Utility.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.BAL.Interfaces.Services
{
    public partial interface ICacheService
    {
        T Get<T>(string key, string orgkey = null);

        /// <summary>
        /// Cache objects for a specified amount of time
        /// </summary>
        /// <param name="key">The cache key</param>
        /// <param name="data">Object / Data to cache</param>
        /// <param name="minutesToCache">How many minutes to cache them for</param>
        void Set(string key, object data, CacheTimes minutesToCache, string orgkey = null);
        bool IsSet(string key);
        void Invalidate(string key);
        void Clear();
        void ClearStartsWith(string keyStartsWith);
        void ClearStartsWith(List<string> keysStartsWith);
        //T CachePerRequest<T>(string cacheKey, Func<T> getCacheItem);
        //void SetPerRequest(string cacheKey, object objectToCache);

        T GetFromCache<T>(string key) where T : class;
        void SetCache<T>(string key, T value) where T : class;
        void SetCache<T>(string key, T value, DateTimeOffset duration) where T : class;
        void ClearCache(string key);
    }
}
