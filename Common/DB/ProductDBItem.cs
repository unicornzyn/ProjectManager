/*----------------------------------------------------------------
// Copyright (C) 2010 盛拓传媒 
// 文件名：ProductDBMain
// 文件功能描述：数据底层 （分页）
//----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace Common.DB
{
    /// <summary>
    /// 数据库连接
    /// </summary>
    public class ProjectDB
    {
        private static BaseData bd = new BaseData(System.Configuration.ConfigurationManager.AppSettings["DefaultConnection"]);
        public static DataTable GetDt(string sql)
        {
            return bd.GetDt(sql);
        }
        public static DataTable GetDt(string sql, SqlParam param)
        {
            return bd.GetDt(sql, param);
        }
        public static string GetFirst(string sql)
        {
            return bd.GetFirst(sql);
        }
        public static string GetFirst(string sql, SqlParam param)
        {
            return bd.GetFirst(sql, param);
        }
        public static int SqlExecute(string sql)
        {
            return bd.SqlExecute(sql);
        }
        public static int SqlExecute(string sql, SqlParam param)
        {
            return bd.SqlExecute(sql, param);
        }
    }

}

