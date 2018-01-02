/*----------------------------------------------------------------
// Copyright (C) 2010 盛拓传媒 
// 文件名：PriceInfo.cs
// 文件功能描述：报价信息处理
//----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
    public class PriceInfo
    {
        public readonly static bool CrawTimeDebug = St.ConfigKey("CrawTimeDebug") == "true" ? true : false;

        /// <summary>
        /// 作者：李延伟
        /// 日期：2011-5-15
        /// 功能：根据抓取规则获取价格
        /// </summary>
        /// <param name="ActionType">规则类型</param>
        /// <param name="ActionData">抓取规则</param>
        /// <param name="url">抓取地址</param>
        /// <returns>报价</returns>
        public static decimal GetPriceByModel(int ActionType, string ActionData, string url)
        {
            if (String.IsNullOrEmpty(url))
            {
                return 0;
            }
            //抓取的报价
            decimal price = 0;
            //抓取的HTML
            string html = string.Empty;
            //抓取的报价
            string Result = string.Empty;

            //提取商城地址
            string MallUrl = GetMallUrl(url);
           
            try
            {
                if (ActionType == 1)//正则抓取
                {

                    html = GetHtml.GetHTML(MallUrl);

                    if (!String.IsNullOrEmpty(html))
                    {
                     
                        Result = RunRegex(html, ActionData);

                        if (!string.IsNullOrEmpty(Result))
                        {
                            price = Convert.ToDecimal(Result);
                        }
                        else
                        {
                            price = 0;

                        }
                    }
                }
                else if (ActionType == 2) //特殊处理
                {
                    object[] Param = new object[] { MallUrl };

                    price = Convert.ToDecimal(Common.ReflexST.StartMethod(ActionData, Param));

                }

                if (price == 0)
                {
                    //抓不到报价
                    ErrorMessage.WriteLog("没有抓到报价", "抓取地址：" + url);
                }
                if (CrawTimeDebug)
                {
                    ErrorMessage.WriteLog("抓取地址：" + url, "最终抓取的报价是：" + price);
                }
                return price;

            }
            catch (Exception ex)
            {

                ErrorMessage.WriteLog("GetPriceByModel，错误信息为:" + ex.Message + "抓取规则是：" + ActionData, "抓取地址：" + MallUrl);
                //抓报价出错 记录报错信息 ， URL

                return 0;
            }
        }


        /// <summary>
        /// 作者：李延伟
        /// 时间：2011-05-18
        /// 功能：执行正则表达式 返回结果
        /// </summary>
        /// <param name="Html">字符串</param>
        /// <param name="rules">规则</param>
        /// <returns></returns>
        public static string RunRegex(string Html, string rules)
        {
            return RunRegex(Html, rules, 1);
        }
        /// <summary>
        /// 作者：李延伟
        /// 时间：2011-05-18
        /// 功能：执行正则表达式 返回结果
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="rules"></param>
        /// <returns></returns>
        public static string RunRegex(string Html, string rules, int index)
        {
            if (string.IsNullOrEmpty(Html))
            {
                return String.Empty;
            }
            string Result = String.Empty;
            string buyRegex = rules;
            Regex rx = new Regex(buyRegex);
            Match myMatch = rx.Match(Html);

            Result = myMatch.Groups[index].ToString();

            return Result;
        }

        /// <summary>
        ///  作者：李延卫
        ///  时间：2011-6-13
        ///  功能：根据商城URL 获取亿起发联盟URL
        /// </summary>
        /// <returns></returns>
        public static string GetYQFUrl(string MallUrl)
        {
            StringBuilder strUrl = new StringBuilder();
            string Domain = RunRegex(MallUrl, @"[a-zA-Z0-9][-a-zA-Z0-9]{0,62}(\.[a-zA-Z0-9][-a-zA-Z0-9]{0,62})+\.?", 0).ToLower();
            string Url = string.Empty; //对应商场的亿起发链接
            switch (Domain)
            {

                case "www.360buy.com":
                    strUrl.Append("http://p.yiqifa.com/c?s=75414ff3&w=344684&c=254&i=160&l=0&e=&t=");
                    break;
                case "www.amazon.cn":
                    strUrl.Append("http://p.yiqifa.com/c?s=405e9c75&w=344684&c=245&i=201&l=0&e=&t=");
                    break;
                case "www.newegg.com.cn":
                    strUrl.Append("http://p.yiqifa.com/c?s=299eeb96&w=344684&c=280&i=240&l=0&e=&t=");
                    break;
                case "www.suning.com":
                    strUrl.Append("http://p.yiqifa.com/c?s=42952802&w=344684&c=4459&i=5662&l=0&e=&t=");
                    break;
                case "www.gome.com.cn":
                    strUrl.Append("");
                    break;
                case "www.all3c.com":
                    strUrl.Append("http://p.yiqifa.com/c?s=cfe603ab&w=344684&c=4923&i=8062&l=0&e=&t=");
                    break;
                case "www.5366.com":
                    strUrl.Append("http://p.yiqifa.com/c?s=5985b446&w=344684&c=4112&i=5022&l=0&e=&t=");
                    break;
                case "www.coo8.com":
                    strUrl.Append("http://p.yiqifa.com/c?s=cbab1a29&w=344684&c=3461&i=2462&l=0&e=&t=");
                    break;
                case "www.tao3c.com":
                    strUrl.Append("http://p.yiqifa.com/c?s=620e684b&w=344684&c=5306&i=11062&l=0&e=&t=");
                    break;
                case "www.21fox.com":
                    strUrl.Append("http://p.yiqifa.com/c?s=04234af1&w=344684&c=5314&i=11182&l=0&e=&t=");
                    break;
                case "www.egou.com":
                    strUrl.Append("");
                    break;
                case "www.icson.com":
                    strUrl.Append("http://p.yiqifa.com/c?s=47a9f02c&w=344684&c=4330&i=4984&l=0&e=&t=");
                    break;
                default:
                    strUrl.Append("");
                    break;

            }

            if (String.IsNullOrEmpty(strUrl.ToString()))
            {
                return MallUrl;
            }
            else
            {
                return strUrl.Append(MallUrl).ToString();
            }


        }


        /// <summary>
        ///  作者：李延卫
        ///  时间：2011-6-13
        ///  功能：根据抓取地址 获取商城地址
        /// </summary>
        /// <returns></returns>
        public static string GetMallUrl(string CrawUrl)
        {


            if (CrawUrl.Contains("p.yiqifa.com"))
            {


                return CrawUrl.Substring(CrawUrl.LastIndexOf("http"));
            }
            else
            {
                return CrawUrl;
            }
           
        }

      

    }
}
