using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
namespace Common
{
    public class ConfigManager
    {
        /// <summary>
        /// 获取汽车类型IDs
        /// </summary>
        /// <returns></returns>
        public static string GetCarSubCategoryIds()
        {
            return string.IsNullOrEmpty(ConfigurationManager.AppSettings["SubCategoryCar"]) ? string.Empty : ConfigurationManager.AppSettings["SubCategoryCar"];
        }


        /// <summary>
        /// 获取图片尺寸
        /// </summary>
        /// <returns></returns>
        public static string GetProductImageSizes()
        {
            return string.IsNullOrEmpty(ConfigurationManager.AppSettings["ProductImageSizes"]) ? string.Empty : ConfigurationManager.AppSettings["ProductImageSizes"];
        }

        /// <summary>
        /// 获取产品图片原图URL前缀
        /// </summary>
        /// <returns></returns>
        public static string GetProductImagesOriginalUrl()
        {
            return string.IsNullOrEmpty(ConfigurationManager.AppSettings["ProductImagesOriginalUrl"]) ? string.Empty : ConfigurationManager.AppSettings["ProductImagesOriginalUrl"];
        }

        /// <summary>
        /// 获取泡泡产品图片同步到的目标位置前缀
        /// </summary>
        /// <returns></returns>
        public static string GetProductImagesPath()
        {
            return string.IsNullOrEmpty(ConfigurationManager.AppSettings["ProductImagesTargetPath"]) ? string.Empty : ConfigurationManager.AppSettings["ProductImagesTargetPath"];
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetIsBigQuery()
        {
            return string.IsNullOrEmpty(ConfigurationManager.AppSettings["IsBigQuery"]) ? "0" : ConfigurationManager.AppSettings["IsBigQuery"];
        }
    }
}
