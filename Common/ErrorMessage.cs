/*----------------------------------------------------------------
// Copyright (C) 2010 盛拓传媒 
// 文件名：ErrorMessage.cs
// 文件功能描述：错误日志处理
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace Common
{
    public class ErrorMessage
    {
        private static string FilePath = System.Configuration.ConfigurationManager.AppSettings["ErrLogPath"].ToString();
        /// <summary>
        /// 作者：yjq
        /// 时间：2011-03-29
        /// 功能：把特定消息写入日志
        /// </summary>
        /// <param name="ex">错误消息</param>
        /// <param name="MessageBody">消息主题</param>
        public static void WriteLog(string ex, string MessageBody)
        {
            StringBuilder msg = new StringBuilder("------执行 " + MessageBody + " 异常时间：" + DateTime.Now + "---\r\n");
            msg.Append(ex + "\r\n\r\n");
            string fileName =String.Format("{0}_{1}_{2}_{3}.log",DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day, DateTime.Now.Hour);


            if (String.IsNullOrEmpty(FilePath))
            {
                Common.St.SaveToFile(msg, System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\ErrLog\\" + fileName + ".log", true);
            }
            else
            {
                string LogFilePath = FilePath + fileName;
                CheckPath(LogFilePath);
                Common.St.SaveToFile(msg, LogFilePath, true);
            }
        }
        private static bool CheckPath(string fileName)
        {

            Common.St.CreateFolder(fileName);
            return true;
        }
    }
}
