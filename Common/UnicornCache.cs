using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public enum CacheKey
    {
        Project,
        User,
        BugzillaUsers
    }
    public class UnicornCache
    {
        public static T Get<T>(CacheKey key)
        {
            object obj = ProductCache.Get(key.ToString());
            if (obj!=null)
            {
                return (T)obj;
            }
            else
            {
                return default(T);
            }
        }

        public static void Add(CacheKey key,object obj)
        {
            ProductCache.Add(key.ToString(), obj);
        }

        public static void Remove(CacheKey key)
        {
            ProductCache.Remove(key.ToString());
        }
    }
}
