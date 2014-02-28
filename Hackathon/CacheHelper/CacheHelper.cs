using System;
using System.Web.Caching;

namespace Hackathon.CacheHelper
{
    /// <summary>
    /// Wrapper for ASP.NET cache
    /// </summary>
    public static class CacheHelper
    {
        /// <summary>
        /// Does the standard check for an object being in cache and returning it if so.
        /// If it's not, calls a method to generate it and then stuff it in cache before returning it.
        /// </summary>
        public static T GetOrAdd<T>(this Cache cache, string cacheKey, Func<T> valueFactory)
            where T : class
        {
            var dataToReturn = cache[cacheKey] as T;
            if (dataToReturn != null) return dataToReturn;

            //cache miss
            dataToReturn = valueFactory();
            cache[cacheKey] = dataToReturn;
            return dataToReturn;
        }
    }
}