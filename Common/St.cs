/*----------------------------------------------------------------
// Copyright (C) 2010 盛拓传媒 
// 文件名：St.cs
// 文件功能描述：字符串操作公共类
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
using System.Drawing.Imaging;
using Webinfo = System.Web.HttpContext;
using Common.CSV;

namespace Common
{
    public class St
    {
        public static Random random = new Random();
        /// <summary>
        /// 作者：王成志
        /// 日期：2011-4-24
        /// 功能：输出XML结构
        /// </summary>
        /// <param name="xml">数据xml</param>
        public static void OutPutXml(StringBuilder xml)
        {
            Webinfo.Current.Response.ContentType = "text/xml";
            Webinfo.Current.Response.Write("<?xml version=\"1.0\" encoding=\"gb2312\"?><root>" + xml + "</root>");
        }

        #region 将字符串转换为Int32

        public static Int32 ToInt32(object str)
        {
            return ToInt32(str, 0);
        }
        public static Int32 ToInt32(object str, Int32 defValue)
        {
            Int32 outValue;

            if (str != null && !String.IsNullOrEmpty(str.ToString()))
            {
                if (Int32.TryParse(str.ToString(), out outValue))
                {
                    return outValue;
                }
            }
            return defValue;
        }
        /// <summary>
        /// 将字符串转换为Float
        /// </summary>
        /// <param name="PriceData"></param>
        /// <returns></returns>
        public static decimal ToDecimal(object str, decimal defValue)
        {

            decimal outValue;

            if (str != null && !String.IsNullOrEmpty(str.ToString()))
            {
                if (decimal.TryParse(str.ToString(), out outValue))
                {
                    return outValue;
                }
            }
            return defValue;


        }
        /// <summary>
        /// 将字符串转换为Float
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float ToFloat(String str)
        {
            return ToFloat(str, 0);
        }
        /// <summary>
        /// 将字符串转换为Float
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float ToFloat(String str, float defValue)
        {
            float outValue;

            if (!String.IsNullOrEmpty(str))
            {
                if (float.TryParse(str, out outValue))
                {
                    return outValue;
                }
            }
            return defValue;
        }

        ///// <summary>
        ///// 转义XML非法字符
        ///// </summary>
        ///// <param name="xmlString"></param>
        ///// <returns></returns>
        //public static string SaftXml(string xmlString) 
        //{
        //    return xmlString.Replace("<","&lt;" ).Replace(">","&gt;" ).Replace( "&","&amp;").Replace( "'","&apos;").Replace("\"","&quot;"); 
        //}
        #endregion

        #region txt日志文件保存方法
        /// <summary>
        /// 将文本保存成UTF-8编码文件。
        /// </summary>
        /// <param name="FileBody">要保存的文本</param>
        /// <param name="Path">保存文件路径</param>
        /// <param name="IsAppend">
        /// 是否为追加。
        /// true:追加
        /// false:覆盖原有内容
        /// </param>
        /// <returns>是否成功</returns>
        public static Boolean SaveToFile(StringBuilder FileBody, string Path, Boolean IsAppend)
        {
            return SaveToFile(FileBody, Path, IsAppend, Encoding.GetEncoding("UTF-8"));
        }

        /// <summary>
        /// 将文本保存成UTF-8编码文件。
        /// </summary>
        /// <param name="FileBody">要保存的文本</param>
        /// <param name="Path">保存文件路径</param>
        /// <param name="IsAppend">
        /// 是否为追加。
        /// true:追加
        /// false:覆盖原有内容
        /// </param>
        /// <returns>是否成功</returns>
        public static Boolean SaveToFile(StringBuilder FileBody, string Path, Boolean IsAppend, Encoding encoder)
        {
            if (Path.Length < 1)
                return false;
            string FilePath = Path.Substring(0, Path.LastIndexOf('\\'));         //设置文件要保存的文件夹。
            if (!System.IO.Directory.Exists(FilePath))
                System.IO.Directory.CreateDirectory(FilePath);
            FilePath = Path;
            try
            {
                //进行文件保存。
                using (StreamWriter sw = new StreamWriter(FilePath, IsAppend, encoder))
                {
                    sw.Write(FileBody.ToString());
                    sw.Flush();
                    sw.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 读取文件信息
        /// <summary>
        /// 以UTF-8编码从指定文件中获取文本。
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <returns>返回读取的字符串</returns>
        public static string GetFromFile(string Path)
        {
            if (!File.Exists(Path))
            {
                return "";
            }//不存在文件返回空字符串。
            StreamReader StrReader = File.OpenText(Path);
            string TempStr = StrReader.ReadToEnd();
            StrReader.Close();
            return TempStr;
        }
        #endregion

        #region 取Web.Config值

        /// <summary>
        /// 取Web.Config值

        /// </summary>
        /// <param name="KeyName"></param>
        /// <returns></returns>
        public static string ConfigKey(string KeyName)
        {
            return System.Configuration.ConfigurationManager.AppSettings[KeyName];
        }
        #endregion


        #region 精确截取
        /// <summary>
        /// 精确截取
        /// </summary>
        /// <param name="length"></param>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string CutStr(int length, string src)
        {
            if (length > 0)
            {
                Font font = new Font("Arial", 10);
                Bitmap bmp = new Bitmap(1, 1);
                Graphics g = Graphics.FromImage(bmp);
                SizeF s = new SizeF();
                float len = length * float.Parse("15.10417") + float.Parse("4.4401");
                for (int i = 0; i < src.Length; i++)
                {
                    s = g.MeasureString(src.Substring(0, src.Length - i), font);
                    if (s.Width <= len)
                    {
                        if (i != 0)
                        {
                            return src.Substring(0, src.Length - i - 1) + "...";//切后字符串
                        }
                        else
                        {
                            return src;//原始字符串
                        }

                    }
                }
            }
            return "";
        }
        #endregion

        #region 转换DateTime
        /// <summary>
        /// 将时间字符串转化为指定的格式 1900-01-01 00:00:00转化为空字符串
        /// 赵英楠
        /// 2014-09-28
        /// </summary>
        /// <param name="str"></param>
        /// <param name="fmt"></param>
        /// <returns></returns>
        public static string ToDateTimeString(object obj, string fmt = "yyyy-MM-dd HH:mm:ss") 
        {
            string str = obj.ToString();
            string s = "";
            if (str!="")
            {
                DateTime time = ToDateTime(str);
                if (time != DateTime.Parse("1900-01-01 00:00:00"))
                {
                    s = time.ToString(fmt);
                }
            }
            return s;
        }
        /// <summary>
        /// 将字符串转换为DateTime
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(String str, DateTime defValue)
        {
            DateTime outValue;

            if (!String.IsNullOrEmpty(str))
            {
                if (DateTime.TryParse(str, out outValue))
                {
                    return outValue;
                }
            }

            return defValue;
        }
        /// <summary>
        /// 将字符串转换为DateTime,转换失败默认2001-1-1
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(String str)
        {
            return ToDateTime(str, DateTime.Parse("1900-01-01 00:00:00"));
        }
        /// <summary>
        /// 将对象转换为DateTime
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="defValue">The def value.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(object obj, DateTime defValue)
        {
            if (obj != null)
            {
                defValue = ToDateTime(obj.ToString(), defValue);
            }

            return defValue;
        }
        #endregion

    
        #region 过滤掉字符串中所有html元素
        public static string CutAllHtmlElement(string InputStr)
        {
            return Regex.Replace(InputStr, @"<[^>]+>", "");
        }
        public static string CutAllHtmlElement_All(string InputStr)
        {
            return Regex.Replace(InputStr, @"<([\s\S]*?)>", "");
        }
        #endregion

        #region 过滤html中危险标签
        public static string CutDangerousHtmlElement(string html)
        {
            System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"<script[^>]*>[^>]*<[^>]script[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex4 = new System.Text.RegularExpressions.Regex(@"<iframe[^>]*>[^>]*<[^>]iframe[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex5 = new System.Text.RegularExpressions.Regex(@"<frameset[^>]*>[^>]*<[^>]frameset[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex6 = new System.Text.RegularExpressions.Regex(@"<img (.+?) />", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //过滤<script></script>标记  
            html = regex4.Replace(html, ""); //过滤iframe  
            html = regex5.Replace(html, ""); //过滤frameset 

            html = regex6.Replace(html, "<img  " + "${1}" + " />");

            return html;
        }
        #endregion

        #region 根据正则匹配，返回第一个结果
        /// <summary>
        /// 根据正则匹配，返回第一个结果
        /// </summary>
        /// <param name="regTxt"></param>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string GetSingleValue(string regTxt, string html)
        {
            Regex regex = new Regex(regTxt);
            Match m = regex.Match(html);
            string result = string.Empty;
            if (m.Success)
            {
                if (m.Groups.Count == 2)
                {
                    result = m.Groups[1].Value;
                }
                else
                {
                    result = m.Groups[0].Value;
                }
            }

            return result;

        }

        #endregion


        #region 判断输入是否是数字
        public static bool IsNumber(object inputData)
        {
            if (inputData != null && !Convert.IsDBNull(inputData))
            {
                if (inputData.ToString() != String.Empty)
                {
                    Match m = Regex.Match(inputData.ToString(), @"^[0-9]+$");
                    if (m.Success)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region 统计字符串的长度intercept
        /// <summary>
        /// 统计字符串的长度intercept
        /// </summary>
        public static int StringLength(string StringObject)
        {
            if (StringObject == null)
            {
                return 0;
            }

            int digit = 0;
            for (int i = 0; i < StringObject.Length; i++)
            {
                if (Convert.ToInt32(Convert.ToChar(StringObject.Substring(i, 1))) < Convert.ToInt32(Convert.ToChar(128)))
                {
                    digit += 1;
                }
                else
                {
                    digit += 2;
                }
            }
            return digit;
        }
        #endregion


        #region 获取指定页面的HTML代码

        /// <summary>
        /// 获取指定页面的HTML代码
        /// </summary>
        /// <param name="url">指定页面的路径</param>
        /// <param name="cookieCollection">Cookie集合</param>
        /// <returns></returns>
        public static string GetHTML(string postData, string url)
        {
            byte[] byteRequest = Encoding.UTF8.GetBytes(postData);
            HttpWebRequest httpWebRequest;
            httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);

            //  httpWebRequest.Headers.Add("Accept-Encoding", "deflate");
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            ServicePointManager.Expect100Continue = false;
            //httpWebRequest.Referer = url;
            //httpWebRequest.Accept = accept;
            //httpWebRequest.UserAgent = userAgent;
            httpWebRequest.Method = "Post";
            httpWebRequest.KeepAlive = false;
            httpWebRequest.ContentLength = byteRequest.Length;
            httpWebRequest.Timeout = 99999999;
            Stream stream = httpWebRequest.GetRequestStream();
            stream.Write(byteRequest, 0, byteRequest.Length);
            stream.Close();
            HttpWebResponse httpWebResponse;
            httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream responseStream = httpWebResponse.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
            string html = streamReader.ReadToEnd();
            streamReader.Close();
            responseStream.Close();

            return html.Replace("\n", "").Replace("\r", "");
        }
        #endregion

        #region 双英文 字符串连接加空格
        /// <summary>
        /// 双英文 字符串连接加空格
        /// </summary>
        /// <param name="strOne"></param>
        /// <param name="stringTwo"></param>
        /// <returns></returns>
        public static string GetTowEnglishString(string strOne, string stringTwo)
        {
            if (string.IsNullOrEmpty(strOne) || string.IsNullOrEmpty(stringTwo)) return string.Empty;
            string strMerge = string.Empty;
            if (string.IsNullOrEmpty(stringTwo)) return strOne;
            if (Common.St.IsNumber(stringTwo.Substring(0, 1))) return (strOne + stringTwo);//stringTwo字符串的首字母是是数字返回 strOne + stringTwo
            if (!Common.St.IsChineseLetter(strOne) && !Common.St.IsChineseLetter(stringTwo))
                strMerge = strOne + " " + stringTwo;
            else
                strMerge = strOne + stringTwo;
            return strMerge;
        }
        #endregion


        #region 判断是中文
        /// <summary>
        /// 判断是中文
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsChineseLetter(string input)
        {
            bool isChineseLetter = false;
            if (!string.IsNullOrEmpty(input))
            {
                int bC = 0;
                for (int i = 0; i < input.Length; i++)
                {
                    Regex rx = new Regex("^[\u4e00-\u9fa5]$");
                    if (rx.IsMatch(input[i].ToString()))
                    {
                        bC++;
                    }
                }
                if (bC == 0)
                    isChineseLetter = false;
                else
                    isChineseLetter = true;
            }
            else
            {
                isChineseLetter = false;
            }
            return isChineseLetter;
        }
        #endregion

        #region 判断是否是英文字母
        /// <summary>
        /// 作者：lj
        /// 时间：2010-07-28
        /// 功能：判断是否是英文字母
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEnglishLetter(string input)
        {
            bool isEnglishLetter = false;
            if (!string.IsNullOrEmpty(input))
            {
                for (int i = 0; i < input.Length; i++)
                {
                    Regex rx = new Regex(@"^[a-zA-Z]$");
                    if (!rx.IsMatch(input[i].ToString()))
                    {
                        isEnglishLetter = false;
                        break;
                    }
                    else isEnglishLetter = true;
                }
            }
            else isEnglishLetter = false;

            return isEnglishLetter;
        }
        #endregion

        #region 返回符合正则式的结果集
        public static ArrayList GetLinks(string inputstr, string Regexstr)
        {
            return GetLinks(inputstr, Regexstr, false);
        }

        public static ArrayList GetLinks(string inputstr, string Regexstr, bool IgnoreRepeated)
        {
            ArrayList compArray = new ArrayList();
            ArrayList myArray = new ArrayList();
            Match mymatch = Regex.Match(inputstr, Regexstr, RegexOptions.IgnoreCase);
            while (mymatch.Success)
            {
                bool isAdd = true;
                if (IgnoreRepeated && compArray.IndexOf(mymatch.Value) > -1)
                {
                    isAdd = false;
                }
                if (isAdd)
                {
                    myArray.Add(mymatch.Value);
                }
                compArray.Add(mymatch.Value);
                mymatch = mymatch.NextMatch();
            }
            return myArray;
        }
        #endregion

        #region SearchAndReplace
        public static string SearchAndReplace(string HtmlStr, string RegexStr, string ReplaceRegex, bool CutHtml)
        {
            string ReturnStr = "";
            Match mymatch = Regex.Match(HtmlStr, RegexStr, RegexOptions.IgnoreCase);
            if (mymatch.Success)
            {
                ReturnStr = mymatch.Value;
            }
            else
            {
                ReturnStr = "";
            }
            ReturnStr = Regex.Replace(ReturnStr, ReplaceRegex, "", RegexOptions.IgnoreCase);
            if (CutHtml)
            {
                ReturnStr = St.CutAllHtmlElement(ReturnStr);
            }
            return ReturnStr.Trim();
        }

        #endregion

        #region 将汉字转化为Uncode字符
        static Regex regex = new Regex("[a-zA-Z0-9]");
        /// <summary>
        /// 将汉字转换为Unicode
        /// </summary>
        /// <param name="text">要转换的字符串</param>
        /// <returns></returns>
        public static string StringToUnicode(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }
            string result = string.Empty;

            foreach (char a in text)
            {
                string lowCode = "", temp = "";

                if (!regex.IsMatch(a.ToString()))
                {
                    byte[] bytes = System.Text.Encoding.Unicode.GetBytes(a.ToString());

                    for (int i = 0; i < bytes.Length; i++)
                    {
                        if (i % 2 == 0)
                        {
                            temp = System.Convert.ToString(bytes[i], 16);//取出元素4编码内容（两位16进制）
                            if (temp.Length < 2) temp = "0" + temp;
                        }
                        else
                        {
                            string mytemp = Convert.ToString(bytes[i], 16);
                            if (mytemp.Length < 2) mytemp = "0" + mytemp; lowCode = lowCode + @"\u" + mytemp + temp;//取出元素4编码内容（两位16进制）
                        }
                    }
                }
                else
                {
                    lowCode = a.ToString();
                }
                result += lowCode;
            }
            return result;

        }

        /// <summary>
        /// 将Unicode字串\u....\u....格式字串转换为原始字符串
        /// </summary>
        /// <param name="srcText"></param>
        /// <returns></returns>
        public static string UnicodeToString(string srcText)
        {
            string str = "";
            str = srcText.Remove(0, 2);
            byte[] bytes = new byte[2];
            bytes[1] = byte.Parse(int.Parse(str.Substring(0, 2), NumberStyles.HexNumber).ToString());
            bytes[0] = byte.Parse(int.Parse(str.Substring(2, 2), NumberStyles.HexNumber).ToString());
            return Encoding.Unicode.GetString(bytes);
        }

        public static string UnicodeToStrings(string str)
        {
            Regex regex = new Regex(@"\\u[a-zA-Z0-9]{4}");
            MatchCollection mc = regex.Matches(str);
            foreach (Match m in mc)
            {
                string oldValue = m.Value;
                string newValue = UnicodeToString(m.Value);
                str = str.Replace(oldValue, newValue);
            }
            return str;
        }

        #endregion

        #region 获取首字母 GetChineseEdh
        /// <summary>
        /// 获取首字母 GetChineseEdh
        /// </summary>
        /// <param name="str"></param>getFirstPY
        /// <returns></returns>
        public static string getFirstPY(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                string source = str.Substring(0, 1);
                try
                {
                    char[] s = source.ToCharArray();
                    int num = (int)s[0];
                    if ((num > 96 && num < 123) || (num > 64 && num < 91) || (num > 47 && num < 58))
                        return source;
                    byte[] array = new byte[2];
                    array = System.Text.Encoding.Default.GetBytes(source);
                    int i = (short)(array[0] - '\0') * 256 + ((short)(array[1] - '\0'));
                    //       if (i < 0xB0A1) return source;
                    if (i < 0xB0C5) return "a";
                    if (i < 0xB2C1) return "b";
                    if (i < 0xB4EE) return "c";
                    if (i < 0xB6EA) return "d";
                    if (i < 0xB7A2) return "e";
                    if (i < 0xB8C1) return "f";
                    if (i < 0xB9FE) return "g";
                    if (i < 0xBBF7) return "h";
                    if (i < 0xBFA6) return "j";
                    if (i < 0xC0AC) return "k";
                    if (i < 0xC2E8) return "l";
                    if (i < 0xC4C3) return "m";
                    if (i < 0xC5B6) return "n";
                    if (i < 0xC5BE) return "o";
                    if (i < 0xC6DA) return "p";
                    if (i < 0xC8BB) return "q";
                    if (i < 0xC8F6) return "r";
                    if (i < 0xCBFA) return "s";
                    if (i < 0xCDDA) return "t";
                    if (i < 0xCEF4) return "w";
                    if (i < 0xD1B9) return "x";
                    if (i < 0xD4D1) return "y";
                    if (i < 0xD7FA) return "z";
                }
                catch
                {
                    //         return source;
                }

                return ChineseDictionary.GetChineseEdh(source);
            }

            return "";
        }

        /// <summary>
        /// 获取字符串中每个字的首字母
        /// </summary>
        /// <returns></returns>
        public static string GetPY(string str) 
        {
            StringBuilder sb = new StringBuilder();
            foreach (char s in str)
            {
                sb.Append(getFirstPY(s.ToString()));
            }
            return sb.ToString();
        }
        #endregion


        #region saftJson
        /// <summary>
        /// 过滤JSON的保留字符
        /// </summary>
        /// <param name="r"></param>
        public static string saftJson(object r)
        {
            if (r == null)
                return "\"\"";
            StringBuilder sb = new StringBuilder();
            foreach (char c in r.ToString().ToCharArray())
            {
                switch (c)
                {
                    case '\\':
                        sb.Append("\\\\".ToCharArray());
                        break;
                    case '\'':
                        sb.Append("\\\'".ToCharArray());
                        break;
                    case '\"':
                        sb.Append("\\\"".ToCharArray());
                        break;
                    case '\r':
                        sb.Append("\\r".ToCharArray());
                        break;
                    case '\n':
                        sb.Append("\\n".ToCharArray());
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            return "\"" + sb + "\"";
        }
        #endregion
        
        #region 转换GB2312 的字符串为UTF8编码
        /// <summary>
        /// 转换GB2312 的字符串为UTF8编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GB2312ToUTF8(string str)
        {
            try
            {
                Encoding uft8 = Encoding.GetEncoding(65001);
                Encoding gb2312 = Encoding.GetEncoding("gb2312");
                byte[] temp = gb2312.GetBytes(str);
                byte[] temp1 = Encoding.Convert(gb2312, uft8, temp);
                string result = uft8.GetString(temp1);
                return result;
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion



        #region 去除HTML
        /// <summary>
        /// 作者：yjq
        /// 时间：2010-03-05
        /// 去除 htmlCode 中所有的HTML标签(包括标签中的属性)
        /// </summary>
        /// <param name="htmlCode">包含 HTML 代码的字符串</param>
        /// <returns>返回一个不包含 HTML 代码的字符串</returns>
        public static string RemoveHtml(string htmlCode)
        {
            if (null == htmlCode || 0 == htmlCode.Length)
            {
                return string.Empty;
            }
            return Regex.Replace(htmlCode, @"<[^>]+>", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Multiline);

        }
        #endregion



        #region 获取页面传递参数变量值
        /// <summary>
        /// 作者：yjq
        /// 时间：2011-01-05
        /// 功能：获取由Get方式传参的参数值
        /// </summary>
        /// <param name="queryName">要过滤的字符串</param>
        /// <returns></returns>
        public static string GetQueryString(string queryName)
        {
            string str = string.Empty;
            if (System.Web.HttpContext.Current.Request.QueryString[queryName] == null)
            {
                return str;
            }
            str = System.Web.HttpContext.Current.Request.QueryString[queryName];
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            if (string.IsNullOrEmpty(str.Trim()))
            {
                return str;
            }
            str = SaftXml(str);
            return str;
        }

        /// <summary>
        /// 作者：yjq
        /// 时间：2011-04-08
        /// 功能：接受参数兼容From GET 形式
        /// </summary>
        /// <param name="queryName">参数名称</param>
        /// <returns></returns>
        public static string GetString(string queryName)
        {

            string str = string.Empty;
            if (System.Web.HttpContext.Current.Request[queryName] == null)
            {
                return str;
            }
            str = Convert.ToString(System.Web.HttpContext.Current.Request[queryName]);
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            if (string.IsNullOrEmpty(str.Trim()))
            {
                return str;
            }
            str = SaftXml(str);
            return str;
        }

        /// <summary>
        /// 作者：yjq
        /// 时间：2011-01-05
        /// 功能：获取由From方式传参的参数值
        /// </summary>
        /// <param name="queryName">要过滤的字符串</param>
        /// <returns></returns>
        public static string FormQueryString(string queryName)
        {
            string str = string.Empty;
            if (System.Web.HttpContext.Current.Request.Form[queryName] == null)
            {
                return str;
            }
            str = Convert.ToString(System.Web.HttpContext.Current.Request.Form[queryName]);
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            if (string.IsNullOrEmpty(str.Trim()))
            {
                return str;
            }
            str = SaftXml(str);
            return str;
        }
        /// <summary>
        /// 作者：yjq
        /// 时间：2011-04-01
        /// 功能：转义比较危险的字符
        /// </summary>
        /// <param name="str">要进行转义的字符串</param>
        /// <returns>转义后的字符串</returns>
        public static string SaftXml(string str)
        {
            string newstr = string.Empty;
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(str.Trim()))
            {
                return newstr;
            }
            newstr = str;
            newstr = newstr.Replace("&", "&amp;");
            newstr = newstr.Replace("<", "&lt;");
            newstr = newstr.Replace(">", "&gt;");
            newstr = newstr.Replace("'", "&apos;");
            newstr = newstr.Replace("\"", "&quot;");

            return newstr;
        }

        #endregion

        #region 适应ISAPI_UrlRewrite获取当前RawUrl
        public static string RawUrl()
        {
            string cUrl = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_REWRITE_URL"];
            if (cUrl == null || cUrl == "")
            {
                cUrl = System.Web.HttpContext.Current.Request.RawUrl;
            }
            return cUrl;
        }
        #endregion

        #region 操作Cookie
        /// <summary>
        /// 设置加密过的COOKIE
        /// by:wangchengzhi
        /// date:2009-12-2
        /// </summary>
        /// <param name="CookieName"></param>
        /// <param name="CookieValue"></param>
        public static void SetCookie(string CookieName, string CookieValue)
        {
            CookieValue = Encrypt(CookieValue);
            System.Web.HttpContext.Current.Response.Cookies[CookieName].Value = CookieValue;
        }
        /// <summary>
        /// 设置加密过的COOKIE
        /// by:wangchengzhi
        /// date:2009-12-2
        /// </summary>
        /// <param name="CookieName"></param>
        /// <param name="CookieValue"></param>
        /// <param name="ExpireTime"></param>
        public static void SetCookie(string CookieName, string CookieValue, DateTime ExpireTime)
        {
            System.Web.HttpContext.Current.Response.Cookies[CookieName].Expires = ExpireTime;
            SetCookie(CookieName, CookieValue);
        }
        /// <summary>
        /// 读取加密过的COOKIE
        /// by:wangchengzhi
        /// date:2009-12-2
        /// </summary>
        /// <param name="CookieName"></param>
        /// <returns></returns>
        public static string GetCookie(string CookieName)
        {
            string CookieValue = null;
            if (System.Web.HttpContext.Current.Request.Cookies[CookieName] != null)
            {
                CookieValue = Decrypt(System.Web.HttpContext.Current.Request.Cookies[CookieName].Value);
            }
            return CookieValue;
        }
        /// <summary>
        /// 删除COOKIE
        /// by:wangchengzhi
        /// date:2009-12-2
        /// </summary>
        /// <param name="CookieName"></param>
        public static void DelCookie(string CookieName)
        {
            SetCookie(CookieName, "", DateTime.Now.AddYears(-1));
        }
        #endregion

        #region 加密/解密
        /// <summary>
        /// 对字符串进行加密
        /// by:wangchengzhi
        /// date:2009-12-2
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string Encrypt(string src)
        {
            if (string.IsNullOrEmpty(src))
                return null;

            DES_ des = new DES_();
            return des.Encrypt(src);
        }
        /// <summary>
        /// 对字符串进行解密
        /// by:wangchengzhi
        /// date:2009-12-2
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string Decrypt(string src)
        {
            if (string.IsNullOrEmpty(src))
                return null;

            src = src.Replace(" ", "+");

            DES_ des = new DES_();
            try
            {
                return des.Decrypt(src);
            }
            catch
            {
                return null;
            }
        }

        #region MD5签名
        /// <summary>
        /// md5签名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMd5(string str)
        {
            string key = "youjing";
            string _input_charset = "UTF-8";
            StringBuilder sb = new StringBuilder(32);

            str = str + key;

            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(str));
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }

            return sb.ToString();
        }
        #endregion

        #endregion



        #region 文件操作
        /// <summary>
        /// 获取一个文件里文本内容
        /// </summary>
        /// <param name="FileFullPath">文件完整路径</param>
        /// <returns></returns>
        public static string GetFileContent(string FileFullPath, Encoding en)
        {
            string Content = "";
            Encoding enCode = Encoding.UTF8;
            if (en != null)
            {
                enCode = en;
            }
            if (System.IO.File.Exists(FileFullPath))
            {
                StreamReader sr = new StreamReader(FileFullPath, enCode);
                Content = sr.ReadToEnd();
                sr.Close();
            }
            return Content;
        }

        /// <summary>
        /// 建立多层文件夹
        /// </summary>
        /// <param name="FolderPath"></param>
        public static void CreateFolder(string FolderPath)
        {
            string[] arrFolder = FolderPath.Split('\\');
            string floder = "";
            for (int i = 0; i < arrFolder.Length; i++)
            {
                if (i > 0)
                {
                    floder = floder + "/" + arrFolder[i];
                    if (arrFolder[i].IndexOf("$") < 0 && arrFolder[i].IndexOf(".") < 0 && arrFolder[i] != "" && arrFolder[i].IndexOf("\\") < 0)
                    {

                        if (!System.IO.Directory.Exists(floder))
                        {
                            System.IO.Directory.CreateDirectory(floder);
                        }
                    }
                }
            }
        }
        #region 将文本写成任意扩展名的文件
        public static void WriteTextToFile(string Text, string FileFullPathAndName)
        {
            if (Text == "" || Text == null)
            {
                return;
            }
            string DirName = GetDirByFileName(FileFullPathAndName);
            if (!Directory.Exists(DirName))
            {
                Directory.CreateDirectory(DirName);
            }
            if (!File.Exists(FileFullPathAndName))
            {
                using (File.Create(FileFullPathAndName)) { };
            }
            Encoding gbEncode = Encoding.GetEncoding("gb2312");
            using (StreamWriter swFromFile = new StreamWriter(FileFullPathAndName, false, gbEncode))
            {
                swFromFile.Write(Text);
                swFromFile.Close();
            }
        }
        public static void WriteTextToFile(string Text, string FileFullPathAndName, bool IsAppend)
        {
            if (Text == "" || Text == null)
            {
                return;
            }
            string DirName = GetDirByFileName(FileFullPathAndName);
            if (!Directory.Exists(DirName))
            {
                Directory.CreateDirectory(DirName);
            }
            if (!File.Exists(FileFullPathAndName))
            {
                using (File.Create(FileFullPathAndName)) { };
            }
            Encoding gbEncode = Encoding.GetEncoding("gb2312");
            using (StreamWriter swFromFile = new StreamWriter(FileFullPathAndName, IsAppend, gbEncode))
            {
                swFromFile.Write(Text);
                swFromFile.Close();
            }
        }
        #endregion

        #region 根据文件名,取得目录名
        public static string GetDirByFileName(string FileName)
        {
            int position = FileName.LastIndexOf(@"\");
            if (position > 0)
            {
                return FileName.Substring(0, position);
            }
            else
            {
                return "";
            }
        }

        #endregion
        #endregion

        #region 去除参数值后的 0
        /// <summary>
        /// 2.0 --> 2; 2.00 --> 2; 2.220 --> 2.22; 2.00222 --> 2.00222; 0.00000 --> 0
        /// <para>author:xuefeng</para>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FormatFloat(string value)
        {
            string temp = value;

            if (string.IsNullOrEmpty(temp))
                return value;

            if (!Regex.IsMatch(temp, @"^[+-]?\d+[.]?\d*$"))
                return value;

            if (!temp.Contains("."))
                return value;

            int len = temp.Split('.')[1].Length;

            string[] temps = temp.Split('.');

            for (int i = temps[1].Length - 1; i >= 0; i--)
            {
                if (temps[1].EndsWith("0"))
                    temps[1] = temps[1].Remove(i);
                else
                    i = -1;
            }
            if (temps[1].Length <= 0)
                return temps[0];
            else
                return temps[0] + "." + temps[1];
        }

        public static bool CheckInt32(string value)
        {
            if (!Regex.IsMatch(value, @"^[+-]?\d+[.]?\d*$"))
                return false;
            else
                return true;
        }
        #endregion

        #region CDN抓取
        /// <summary>
        /// 用于CDN缓存页面
        /// </summary>
        public static void SetPageCacheHead()
        {
            SetPageCacheHead(120);
        }

        public static void SetPageCacheHead(int minutes)
        {
            System.Web.HttpContext.Current.Response.Cache.SetCacheability(System.Web.HttpCacheability.Public);
            System.Web.HttpContext.Current.Response.Cache.SetLastModified(DateTime.Now);
            System.Web.HttpContext.Current.Response.Cache.SetExpires(DateTime.Now.AddMinutes(minutes));
            System.Web.HttpContext.Current.Response.Cache.SetMaxAge(new TimeSpan(0, minutes, 0));
            System.Web.HttpContext.Current.Response.Cache.SetOmitVaryStar(true);
        }
        #endregion

        #region 双英文 字符串连接加空格
        /// <summary>
        /// 双英文 字符串连接加空格
        /// </summary>
        /// <param name="strOne"></param>
        /// <param name="stringTwo"></param>
        /// <returns></returns>
        public static string GetNewTwoEnglishString(string strOne, string stringTwo)
        {
            string strMerge = string.Empty;
            if (St.IsNumber(stringTwo))
                return strMerge = strOne + stringTwo;
            if ((strOne == null || strOne == "") && (stringTwo == null || stringTwo == ""))
                return "";
            if (strOne == null || strOne == "")
                return stringTwo;
            if (stringTwo == null || stringTwo == "")
                return strOne;
            if (St.IsNumber(strOne.Substring(strOne.Length - 1, 1)) || St.IsNumber(stringTwo.Substring(0, 1)))
                return (strOne.Trim() + stringTwo.Trim());

            if (!St.IsChineseLetter(strOne.Substring(strOne.Length - 1, 1)) && !St.IsChineseLetter(stringTwo.Substring(1)))
                strMerge = strOne + " " + stringTwo;
            else
                strMerge = strOne + stringTwo;
            return strMerge;
        }
        #endregion

        #region 获得当前访问者的IP
        /// <summary>
        /// 获得当前访问者的IP
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string Ip = string.Empty;
            if (HttpContext.Current != null)
            {
                // 穿过代理服务器取远程用户真实IP地址
                if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null)
                    {
                        if (HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null)
                        {
                            Ip = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].ToString();
                        }
                        else
                        {
                            if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null)
                            {
                                Ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                            }
                            else
                            {
                                Ip = "";
                            }
                        }
                    }
                    else
                    {
                        Ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                    }
                }
                else if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null)
                {
                    Ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                }
                else
                {
                    Ip = "";
                }
            }

            return Ip;

        }
        #endregion

        #region 校验DataTable中是否有数据
        /// <summary>
        /// 功能：  true 有数据 false 没数据;
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool CheckDt(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion


        /// <summary>
        /// 作者:yjq
        /// 时间:2011年11月3日
        /// 功能:将obj转换成字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToString(object obj)
        {
            return obj == null ? string.Empty : obj.ToString().Trim();
        }

        /// <summary>
        /// 字符串截取,省略部分加点(点个数可控)
        /// create by : njf
        /// date:2011-7-21
        /// <param name="StringObject">待处理字符串</param>
        /// <param name="MaxLength">处理后字符串长度</param>
        /// <param name="docCount">点数</param>
        /// <returns></returns>
        public static string cutstrDot(string StringObject, int MaxLength, int docCount)
        {
            StringBuilder dotStr = new StringBuilder();
            for (int i = 0; i < docCount; i++)
            {
                dotStr.Append(".");
            }
            if (StringObject == null)
            {
                StringObject = string.Empty;
            }
            if (StringObject == string.Empty)
            {
                return StringObject;
            }
            int StringObjectLength = 0;
            for (int i = 0; i < StringObject.Length; i++)
            {
                if (Convert.ToInt32(Convert.ToChar(StringObject.Substring(i, 1))) < Convert.ToInt32(Convert.ToChar(128)))
                {
                    StringObjectLength += 1;
                }
                else
                {
                    StringObjectLength += 2;
                }
                if (StringObjectLength > MaxLength)
                {
                    break;
                }
            }
            if (StringObjectLength > MaxLength)
            {
                int WordNum = 0;
                int digit = 0;
                for (int i = 0; i < StringObject.Length; i++)
                {
                    if (Convert.ToInt32(Convert.ToChar(StringObject.Substring(i, 1))) < Convert.ToInt32(Convert.ToChar(128)))
                    {
                        digit += 1;
                    }
                    else
                    {
                        digit += 2;
                    }
                    WordNum++;
                    if (digit >= MaxLength)
                    {
                        break;
                    }
                }
                if (digit > MaxLength)
                {
                    WordNum = WordNum - 1;
                    if (WordNum < 0)
                    {
                        WordNum = 0;
                    }
                }
                StringObject = StringObject.Substring(0, WordNum) + dotStr.ToString();
                return StringObject;
            }
            else
            {
                return StringObject;
            }
        }

        /// <summary>
        /// 作者：yjq
        /// 时间：2011-04-07
        /// 功能：验证字符串是否为空 true 不为空
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public static bool CheckStr(Object ob)
        {
            if (ob == null)
            {
                return false;
            }
            string newstr = Convert.ToString(ob).Trim();
            if (string.IsNullOrEmpty(newstr))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 作者：yjq
        /// 时间：2011-04-08
        /// 功能：关闭本页，刷新父页面
        /// </summary>
        /// <returns>js</returns>
        public static string Getscript()
        {
            string result = @"<script language=javascript> 
                              window.opener.parent.document.location.reload(); 
                              window.opener=null;window.open( '', '_self');
                              window.close();
                              </script>";
            return result;
        }

        /// <summary>
        /// 作者：王成志
        /// 时间：2012-06-06
        /// 功能：获取正则匹配到的第一个匹配的第一对括号的匹配内容。
        /// </summary>
        /// <param name="text">待匹配的字符串</param>
        /// <param name="regex">匹配的正则</param>
        /// <returns></returns>
        public static string GetFirstMatch(string text, string regex)
        {
            return GetMatch(text, regex, 1);
        }

        /// <summary>
        /// 作者：王成志
        /// 时间：2012-06-06
        /// 功能：获取正则匹配到的第一个匹配的第一对括号的匹配内容。
        /// </summary>
        /// <param name="text">待匹配的字符串</param>
        /// <param name="regex">匹配的正则</param>
        /// <returns></returns>
        public static string GetMatch(string text, string regex, int index)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }
            Regex reg = new Regex(regex);
            MatchCollection mc = reg.Matches(text);
            if (mc.Count > 0 && mc[0].Groups.Count > index)
            {
                return mc[0].Groups[index].Value;
            }
            return "";
        }


        public static string GetMatch(Match m, int index)
        {
            if (m.Groups.Count > index)
            {
                return m.Groups[index].Value;
            }
            return string.Empty;
        }



        /// <summary>
        /// 作者：yjq
        /// 时间：2011-06-14
        /// 功能：返回图片code
        /// </summary>
        /// <param name="fExt">图片后缀</param>
        /// <returns></returns>
        public static ImageCodecInfo GetEncoderInfoByExtension(string fExt)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < encoders.Length; i++)
            {
                if (encoders[i].FilenameExtension.IndexOf(fExt.ToUpper()) > -1)
                {
                    return encoders[i];
                }
            }
            return null;
        }
        /// <summary>
        /// 作者：窦海超
        /// 时间：2011-06-29
        /// 功能：过滤XML中不支持的字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceXML(string str)
        {
            //str = str.Replace("\"","");
            //str = str.Replace("'", "‘");
            str = str.Replace("<", "＜");
            str = str.Replace(">", "＞");
            //str = str.Replace("&", "＆");
            return str;
        }
        public static bool CheckDs(DataSet ds)
        {
            if (ds == null) { return false; }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 作者:刘玉磊
        /// 时间:2011-09-06
        /// 功能:写日志
        /// </summary>
        /// <param name="logPath"></param>
        /// <param name="content"></param>
        public static bool WriteLog(string logPath, string content)
        {
            bool wflag = false;
            FileStream fs = null;
            if (string.IsNullOrEmpty(logPath))
            {
                //默念在根目录下，web 与应用程序 调用在各自的根目录下
                logPath = System.AppDomain.CurrentDomain.BaseDirectory;// +"\\log_" + DateTime.Now.ToString("yyyyMMdd") + "\\log.log";
            }

            //文件夹区分日期
            //logPath = logPath + "log_" + DateTime.Now.ToString("yyyyMMdd") + "\\";
            //string fileName = "log.log";\
            //文件区分日期
            logPath = logPath + "log\\";
            string fileName = "log_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            string path_file = logPath + fileName;
            try
            {

                if (!Directory.Exists(logPath))
                {
                    DirectoryInfo diFilePath = Directory.CreateDirectory(logPath);
                }
                if (!File.Exists(path_file))
                {
                    byte[] b = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(content);
                    fs = File.Create(path_file, 1024, FileOptions.None);
                    fs.Write(b, 0, b.Length);
                    fs.Flush();
                    fs.Close();
                }
                else
                {
                    fs = new FileStream(path_file, FileMode.Append);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(content);
                    sw.Flush();
                    sw.Close();
                }
                wflag = true;
            }
            catch (Exception)
            {
                wflag = false;
            }

            return wflag;
        }
        /// <summary>
        /// 作者：王成志
        /// 时间：2012-1-7
        /// 作用：获取CSV文件中的指定名称列数据。
        /// 如果不存在指定列。则返回空表。
        /// </summary>
        /// <param name="colNames"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DataTable GetDataTableFromCSV(string path, string colNames)
        {
            try
            {
               // St.WriteLog("", "从" + path + "获取文件\r\n");
                if (string.IsNullOrEmpty(colNames))
                {
                    return new DataTable();
                }//排除无效选择列
               
                CsvStreamReader reader = new CsvStreamReader(path);
                if (reader.RowCount < 1 || reader.ColCount < 1)
                {
                    return new DataTable();
                }//排除0列或0行的情况
                Dictionary<string, int> cols = new Dictionary<string, int>();
                for (int i = 0; i < reader.ColCount; i++)
                {
                    string colName = reader[1, i + 1];//取首行各列做列名。
                    if (!cols.ContainsKey(colName))
                    {
                        cols.Add(colName, i + 1);
                    }
                }//收集文件中列名（做防重处理）
                foreach (string name in colNames.Split(','))
                {
                    if (!cols.ContainsKey(name))
                    {
                        return new DataTable();
                    }
                }//验证所取列名是否完整。
                DataTable rdt = new DataTable();
                foreach (string name in colNames.Split(','))
                {
                    rdt.Columns.Add(name);
                }//构建返回表结构。
                for (int i = 1; i < reader.RowCount; i++)
                {
                    DataRow row = rdt.NewRow();
                    foreach (DataColumn col in rdt.Columns)
                    {
                        row[col] = reader[i + 1, cols[col.ColumnName]];
                    }
                    rdt.Rows.Add(row);
                }
                return rdt;
            }
            catch (Exception ex) { St.WriteLog("", ex.Message); return null; }
           
        }

        /// <summary>
        /// 过滤掉js不能处理的字符
        /// </summary>
        /// <param name="content">待处理的字符串</param>
        /// <returns>处理完的代码</returns>
        public static string saftJSStr(string content)
        {
            return content.Replace(',', '，').Replace('\'', '’').Replace('"', '”');
        }
    
    }
}
