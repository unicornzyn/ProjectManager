/*----------------------------------------------------------------
// Copyright (C) 2010 盛拓传媒 
// 文件名：GetHtml.cs
// 文件功能描述：页面抓取类
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
namespace Common
{




    public class GetHtml
    {

        //是否计算运行时间
        public readonly static bool CrawTimeDebug = St.ConfigKey("CrawTimeDebug") == "true" ? true : false;
        #region 获取指定页面的HTML代码
        /// <summary>
        /// 作者：yjq
        /// 时间：2011-03-11
        /// 功能：获取指定页面的HTML代码
        /// </summary>
        /// <param name="url">指定页面的路径</param>
        /// <returns></returns>
        public static string GetHTML(string url)
        {
            try
            {
                if (String.IsNullOrEmpty(url))
                {
                    return string.Empty;
                }

                #region 统计开始
                Stopwatch timer = new Stopwatch();
                if (CrawTimeDebug)
                {

                    timer.Start();
                }
                #endregion

                Encoding Format = GetEncoding(GetHtmlNoFormat(url)); //编码格式
                HttpWebRequest httpWebRequest;
                httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpWebRequest.Referer = url;
                httpWebRequest.Method = "GET";
                httpWebRequest.UserAgent = "User-Agent: Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
                HttpWebResponse httpWebResponse;
                httpWebResponse = GetHttpWebResponse(httpWebRequest);
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream, Format);
                string html = streamReader.ReadToEnd();
                streamReader.Close();
                responseStream.Close();

                #region 统计开始
                if (CrawTimeDebug)
                {
                    timer.Stop();
                    ErrorMessage.WriteLog("根据编码获取HTML", "地址：" + url + ". 时间为: " + timer.Elapsed.TotalSeconds.ToString());
                }
                #endregion
                return html;
            }
            catch (Exception ex)
            {
                ErrorMessage.WriteLog("获取HTML代码出错", "抓取地址：" + url + ". 错误信息为: " + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 作者：李延伟
        /// 日期：2011-5-12
        /// 功能：根据页面源代码区分编码格式；
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetHtmlNoFormat(string url)
        {
            try
            {
                if (String.IsNullOrEmpty(url))
                {
                    return string.Empty;
                }

                #region 统计开始
                Stopwatch timer = new Stopwatch();
                if (CrawTimeDebug)
                {

                    timer.Start();
                }
                #endregion
                HttpWebRequest httpWebRequest;
                httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpWebRequest.Referer = url;
                httpWebRequest.Method = "GET";
                httpWebRequest.UserAgent = "User-Agent: Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
                HttpWebResponse httpWebResponse;



                httpWebResponse = GetHttpWebResponse(httpWebRequest);

                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                string html = streamReader.ReadToEnd();
                streamReader.Close();
                responseStream.Close();

                #region 统计开始
                if (CrawTimeDebug)
                {
                    timer.Stop();
                    ErrorMessage.WriteLog("第一次获取HTML", "地址：" + url + ". 时间为: " + timer.Elapsed.TotalSeconds.ToString());
                }
                #endregion
                return html;
            }
            catch (Exception ex)
            {
                ErrorMessage.WriteLog("获取HTML代码出错", "地址：" + url + ". 错误信息为: " + ex.Message);
                return null;
            }

        }

        /// <summary>
        /// 获取指定页面的HTML代码  用于国美抓报价
        /// </summary>
        /// <param name="postData">postData</param>
        /// <param name="url">url</param>
        /// <returns></returns>
        public static string GetGomeHTML(string postData, string url)
        {

            #region 统计开始
            Stopwatch timer = new Stopwatch();
            if (CrawTimeDebug)
            {

                timer.Start();
            }
            #endregion
            System.Net.ServicePointManager.Expect100Continue = false;
            byte[] byteRequest = Encoding.UTF8.GetBytes(postData);
            HttpWebRequest httpWebRequest;
            httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.Method = "Post";
            httpWebRequest.KeepAlive = false;
            httpWebRequest.ContentLength = byteRequest.Length;
            httpWebRequest.Timeout = 99999999;
            Stream stream = httpWebRequest.GetRequestStream();
            stream.Write(byteRequest, 0, byteRequest.Length);
            stream.Close();
            HttpWebResponse httpWebResponse;
            httpWebResponse = GetHttpWebResponse(httpWebRequest);
            Stream responseStream = httpWebResponse.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
            string html = streamReader.ReadToEnd();
            streamReader.Close();
            responseStream.Close();

            #region 统计开始
            if (CrawTimeDebug)
            {
                timer.Stop();
                ErrorMessage.WriteLog("获取国美HTML", "地址：" + url + ". 时间为: " + timer.Elapsed.TotalSeconds.ToString());
            }
            #endregion
            return html;
        }

        /// <summary>
        /// 防止获取HTML时出错
        /// </summary>
        /// <param name="webRequest"></param>
        /// <returns></returns>
        private static HttpWebResponse GetHttpWebResponse(HttpWebRequest webRequest)
        {
            try
            {
                return (HttpWebResponse)webRequest.GetResponse();
            }
            catch (WebException ex)
            {

                ErrorMessage.WriteLog("", "获取HTML代码出错,错误信息是：" + ex.Message + " 链接地址：" + ex.HelpLink);

                return (HttpWebResponse)ex.Response;
            }
        }

        /// <summary>
        /// 作者：李延伟
        /// 日期：2011-5-12
        /// 功能：根据meta获取网页的编码格式
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        static Encoding GetEncoding(string html)
        {
            #region 统计开始
            Stopwatch timer = new Stopwatch();
            if (CrawTimeDebug)
            {

                timer.Start();
            }
            #endregion
            string pattern = @"(?i)\bcharset=(?<charset>[-a-zA-Z_0-9]+)";
            string charset = Regex.Match(html, pattern).Groups["charset"].Value;
            #region 统计开始
            if (CrawTimeDebug)
            {
                timer.Stop();
                ErrorMessage.WriteLog("获取页面编码格式", " 正则时间: " + timer.Elapsed.TotalSeconds.ToString());
            }
            #endregion
            try { return Encoding.GetEncoding(charset); }
            catch (ArgumentException) { return null; }
        }

        #endregion
    }
}
