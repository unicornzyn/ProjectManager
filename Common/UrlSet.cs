/*----------------------------------------------------------------
// Copyright (C) 2010 盛拓传媒 
// 文件名：St.cs
// 文件功能描述：字符串操作公共类
----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;
using Webinfo = System.Web.HttpContext;

namespace Common
{
    /// <summary>
    /// 参数实体类
    /// </summary>
    public class UrlSet
    {
        /// <summary>
        /// 作者：王成志
        /// 日期：2011-5-24
        /// 功能：获取图片url
        /// </summary>
        /// <param name="photoId">图片ID</param>
        /// <returns></returns>
        public static string GetShowCasePhotoUrl(string size, int photoId)
        {
            return St.ConfigKey("ShowCaseImagesUrl").TrimEnd('/') + "/" + size + GetPathSpanById(photoId) + ".png" + GetSuffix();
        }

        /// <summary>
        /// 作者：王成志
        /// 日期：2011-5-24
        /// 功能：获取主图的地址
        /// </summary>
        /// <param name="photoId"></param>
        /// <returns></returns>
        public static string GetMainPhotoUrl(string size, int productId)
        {
            return St.ConfigKey("MainImagesUrl").TrimEnd('/') + "/" + size + GetPathSpanById(productId) + ".png" + GetSuffix();
        }

        /// <summary>
        /// 作者：高鑫
        /// 日期：2011-5-24
        /// 功能：获取二级维度默认图片
        /// </summary>
        /// <param name="photoId"></param>
        /// <returns></returns>
        public static string GetDefaultPhotoUrl(string size, int id)
        {
            return St.ConfigKey("PhotoImagesPathShow").TrimEnd('/') + "/" + size + "/" + id + ".jpg" + GetSuffix();
        }

        /// <summary>
        /// 作者：王成志
        /// 日期：2011-5-24
        /// 功能：获取默认的暂无图片地址 
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string GetNoPicUrl(string size)
        {
            return St.ConfigKey("NoPicUrl").TrimEnd('/') + "/" + size + ".png" + GetSuffix();
        }

        /// <summary>
        /// 作者：高鑫
        /// 日期：2011-5-26
        /// 功能：获取默认的暂无图片物理地址 
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string GetNoPicPath(string size)
        {
            return St.ConfigKey("NoPicPath").TrimEnd('\\') + "\\" + size + ".png";
        }

        /// <summary>
        /// 作者：窦海超
        /// 日期：2011-5-24
        /// 功能：获取除以1000000，除以1000得到的中间值。
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static string GetProductPhotoUrl(string size, int productid)
        {
            return St.ConfigKey("ProductImagesPathShow").TrimEnd('/') + "/" + size + "/" + productid / 1000000 + "/" + productid / 1000 + "/" + productid + ".jpg" + GetSuffix();
        }

        /// <summary>
        /// 作者：高鑫
        /// 日期：2011-5-24
        /// 功能：获取二级维度下图片数据的物理路径
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static string GetProductPhotoPath(string size, int productId)
        {
            return St.ConfigKey("ProductImagesPath").TrimEnd('\\') + "\\" + size + GetPathById(productId) + ".jpg";
        }

        /// <summary>
        /// 作者：高鑫
        /// 日期：2011-5-24
        /// 功能：获取二级维度默认图的物理路径
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static string GetDefaultPhotoPath(string size, int productId)
        {
            return St.ConfigKey("PhotoImagesPath").TrimEnd('\\') + "\\" + size + "\\" + productId + ".jpg";
        }

        /// <summary>
        /// 作者：潘兴亮
        /// 时间：2011-1-16
        /// 功能：微动作Web版本列表页URL
        /// </summary>
        /// <param name="brandId">品牌id</param>
        /// <param name="priceBoundId">价格范围</param>
        /// <param name="list">搜索参数</param>
        /// <param name="orderId">排序顺序</param>
        /// <returns></returns>
        public static string GetMicroActionList(string category, int brandId, int priceBoundId, List<int> list, int orderId)
        {
            if (string.IsNullOrEmpty(category))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("http://").Append(Webinfo.Current.Request.Url.Host.TrimEnd('/'));
            sb.Append("/").Append(category).Append("/");

            if (brandId > 0)
            {
                sb.Append("b").Append(brandId).Append("_");
            }
            if (priceBoundId > 0)
            {
                sb.Append("p").Append(priceBoundId).Append("_");
            }
            string oList = string.Empty;
            if (list != null && list.Count > 0)
            {
                oList = "o";
                foreach (int item in list)
                {
                    if (item > 0)
                    {
                        oList += item + "-";
                    }
                }
                sb.Append(oList.TrimEnd('-')).Append("_");
            }
            if (orderId > 0)
            {
                sb.Append("by").Append(orderId);
            }
            return sb.Append(".shtml").ToString();
        }

        /// <summary>
        /// 作者：潘兴亮
        /// 时间：2011-1-16
        /// 功能：微动作Web版本对比页URL
        /// </summary>
        /// <param name="productIDs">产品ID</param>
        /// <param name="version">版本号</param>
        /// <returns></returns>
        public static string GetMicroActionContrast(List<int> productIDs, int version, int categoryID, string category)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("http://").Append(Webinfo.Current.Request.Url.Host.TrimEnd('/'));
            sb.Append("/").Append(category).Append("/");
            if (productIDs != null && productIDs.Count > 0)
            {
                foreach (int item in productIDs)
                {
                    if (item > 0)
                    {
                        sb.Append(item).Append("_");
                    }
                }
            }
            sb.Append(version);
            sb.Append("_").Append(categoryID);
            return sb.Append(".shtml").ToString();
        }

        #region 工具

        /// <summary>
        /// 作者：王成志
        /// 日期：2011-5-24
        /// 功能：获取除以1000000，除以1000得到的中间值。
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private static string GetPathSpanById(int Id)
        {
            return "/" + Id / 1000000 + "/" + Id / 1000 + "/" + Id;
        }

        /// <summary>
        /// 作者：高鑫
        /// 日期：2011-5-26
        /// 功能：获取除以1000000，除以1000得到的中间值（物理路径）。
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private static string GetPathById(int Id)
        {
            return "\\" + Id / 1000000 + "\\" + Id / 1000 + "\\" + Id;
        }

        /// <summary>
        /// 作者：高鑫
        /// 日期：2011-5-24
        /// 功能：获取时间后缀防止缓存
        /// </summary>
        /// <returns></returns>
        private static string GetSuffix()
        {
            return "?net=" + DateTime.Now.ToString("yyyyMMddHHmmsshhh");
        }

        #endregion
    }
}
