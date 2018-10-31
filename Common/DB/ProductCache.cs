/*----------------------------------------------------------------
// Copyright (C) 2010 盛拓传媒 
// 文件名：ProductCache.cs
// 文件功能描述：数据缓存
//----------------------------------------------------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Web;
using System.Threading;
using System.Web.Caching;

namespace Common
{
    public class ProductCache
    {
        private readonly static int CacheMode = 1;//Convert.ToInt32(St.ConfigKey("CacheMode"));

        public static void Add(string CacheName, object CacheValue, int Minute)
        {
            if (string.IsNullOrEmpty(CacheName))
            {
                return;
            }
            if (CacheName.Length > 50)
            {
                CacheName = Common.St.GetMd5(CacheName);
            }
            switch (CacheMode)
            {
                case 0://不使用缓存

                    return;
                case 1://.net缓存
                    System.Web.HttpContext.Current.Cache.Insert(CacheName, CacheValue, null, DateTime.Now.AddMinutes(Minute), System.Web.Caching.Cache.NoSlidingExpiration);
                    break;
                case 2://memorycache(还未实现)
                    return;
                case 3://winform或控制台服务器缓存
                    DeskAppCache.Insert(CacheName, CacheValue, null, DateTime.Now.AddMinutes(Minute), System.Web.Caching.Cache.NoSlidingExpiration);
                    break;
            }
        }

        public static void Add(string CacheName, object CacheValue)
        {
            Add(CacheName, CacheValue, 120);
        }

        /// <summary>
        /// 功能：设置依赖缓存
        /// 作者：haojingbo
        /// 时间：2009-12-08
        /// </summary>
        /// <param name="cacheName">缓存key</param>
        /// <param name="dependencies">依赖类型 key cacheKey依赖 file 文件依赖</param>
        /// <param name="dependencyKey">cacheKey依赖 key数组</param>
        /// <param name="dependencyFileUrl">依赖的文件url</param>
        /// <param name="source"></param>
        public static void Add(string cacheName, string dependencies, string[] dependencyKey, string dependencyFileUrl, object source)
        {

            CacheDependency cacheDependency = null;
            switch (dependencies)
            {
                case "":
                    cacheDependency = null;
                    break;
                case "key":
                    cacheDependency = new CacheDependency(null, dependencyKey);
                    break;
                case "file":
                    cacheDependency = new CacheDependency(dependencyFileUrl);
                    break;

            }
            System.Web.HttpContext.Current.Cache.Insert(cacheName, source, cacheDependency);
        }

        public static Object Get(string cacheName)
        {
            if (cacheName.Length > 50)
            {
                cacheName = Common.St.GetMd5(cacheName);
            }
            switch (CacheMode)
            {
                   
                case 0://不使用缓存
                    return null;
                case 1://.net缓存
                    return (Object)HttpContext.Current.Cache[cacheName]; 
                case 2://memorycache(还未实现)
                    return null; ;
                case 3://winform或控制台服务器缓存
                      return (Object)DeskAppCache[cacheName];
                  
            }
            return null;
        }

        public static void Remove(string cacheName)
        {
            HttpContext.Current.Cache.Remove(cacheName);
        }

        #region 桌面程序Cache专用
        private static HttpRuntime _httpRuntime;
        private static Cache DeskAppCache
        {
            get
            {
                EnsureHttpRuntime();
                return HttpRuntime.Cache;
            }
        }
        private static void EnsureHttpRuntime()
        {
            if (null == _httpRuntime)
            {
                try
                {
                    Monitor.Enter(typeof(ProductCache));
                    if (null == _httpRuntime)
                    {
                        // Create an Http Content to give us access to the cache.
                        _httpRuntime = new HttpRuntime();
                    }
                }
                finally
                {
                    Monitor.Exit(typeof(ProductCache));
                }
            }
        }
        #endregion
    }
}
